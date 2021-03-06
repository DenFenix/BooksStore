﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Controllers;
using WebUI.Models;

namespace UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            Book book1 = new Book { BookId = 1, Name = "Test1" };
            Book book2 = new Book { BookId = 2, Name = "Test2" };

            Cart cart = new Cart();
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 2);

            List<CartLine> result = cart.Lines.ToList();

            Assert.AreEqual(result.Count(), 2);
            Assert.AreEqual(result[0].Book, book1);
            Assert.AreEqual(result[1].Book, book2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            Book book1 = new Book { BookId = 1, Name = "Test1" };
            Book book2 = new Book { BookId = 2, Name = "Test2" };

            Cart cart = new Cart();
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 2);
            cart.AddItem(book1, 5);

            List<CartLine> result = cart.Lines.ToList();

            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(result[0].Quantity, 6);
            Assert.AreEqual(result[1].Quantity, 2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            Book book1 = new Book { BookId = 1, Name = "Test1" ,Price = 100};
            Book book2 = new Book { BookId = 2, Name = "Test2", Price = 55 };

            Cart cart = new Cart();
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 2);
            cart.AddItem(book1, 5);

            decimal result = cart.ComputeTotalValue();
            Assert.AreEqual(result, 710);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            Book book1 = new Book { BookId = 1, Name = "Test1", Price = 100 };
            Book book2 = new Book { BookId = 2, Name = "Test2", Price = 55 };

            Cart cart = new Cart();
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 2);
            cart.AddItem(book1, 5);
            cart.Clear();

            Assert.AreEqual(cart.Lines.Count(), 0);
        }

        /// <summary>
        /// Добавление в корзину
        /// </summary>
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new List<Book>
            {
                 new Book {BookId = 1, Name  = "Книга1", Category = "Cat1"}                
            }.AsQueryable);

            Cart cart = new Cart();

            CartController controller = new CartController(mock.Object, null);


            controller.AddToCart(cart, 1, null);

            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToList()[0].Book.BookId, 1);
        }

        /// <summary>
        /// После добавления игры в корзину, должно быть перенаправление на страницу корзины
        /// </summary>
        [TestMethod]
        public void Adding_Game_To_Cart_Goes_To_Cart_Screen()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new List<Book>
            {
                 new Book {BookId = 1, Name  = "Книга1", Category = "Cat1"}
            }.AsQueryable);

            Cart cart = new Cart();

            CartController controller = new CartController(mock.Object, null);

            RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        // Проверяем URL
        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            Cart cart = new Cart();
            CartController target = new CartController(null, null);

            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;
            Assert.AreEqual(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            // Организация - создание пустой корзины
            Cart cart = new Cart();
            // Организация - создание деталей о доставке
            ShippingDetails shippingDetails = new ShippingDetails();
            // Организация - создание контроллера
            CartController controller = new CartController(null, mock.Object);

            ViewResult result = controller.Checkout(cart, shippingDetails);

            // Утверждение — проверка, что заказ не был передан обработчику 
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

            // Утверждение — проверка, что метод вернул стандартное представление 
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            Cart cart = new Cart();
            cart.AddItem(new Book(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Организация — добавление ошибки в модель
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Утверждение - проверка, что заказ не передается обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Утверждение - проверка, что метод вернул стандартное представление
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            Cart cart = new Cart();
            cart.AddItem(new Book(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Утверждение - проверка, что заказ передан обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Once());

            // Утверждение - проверка, что метод возвращает представление 
            Assert.AreEqual("Completed", result.ViewName);

            // Утверждение - проверка, что представлению передается допустимая модель
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
