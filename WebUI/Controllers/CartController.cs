using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    /// <summary>
    /// Контроллер для работы с корзиной
    /// </summary>
    public class CartController : Controller
    {
        private IBookRepository repository;
        private IOrderProcessor orderProcessor;

        public CartController(IBookRepository repository, IOrderProcessor orderProcessor)
        {
            this.repository = repository;
            this.orderProcessor = orderProcessor;
        }

        //Метод для виджета
        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }


        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if(cart.Lines.Count() ==0)
            {
                ModelState.AddModelError("", "Извините, ваша корзина пуста");
            }
            if(ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

   

        public RedirectToRouteResult AddToCart(Cart cart, int bookId, string returnUrl)
        {
            Book book = repository.Books.FirstOrDefault(b => b.BookId == bookId);
            if (book != null)
            {
                cart.AddItem(book, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int bookId, string returnUrl)
        {
            Book book = repository.Books.FirstOrDefault(b => b.BookId == bookId);
            if(book!= null)
            {
                cart.RemoveLine(book);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        ///// <summary>
        ///// Для сохранения и извлечения объектов Cart применяется средство состояния сеанса ASP.NET.
        ///// </summary>
        ///// <returns></returns>
        //public Cart GetCart()
        //{
        //    //Ихвлекаем знаечение этого ключа
        //    Cart cart = (Cart)Session["Cart"];
        //    if(cart == null)
        //    {
        //        cart = new Cart();
        //        //устанавливаем значение для определенного ключа в сессии
        //        Session["Cart"] = cart;
        //    }
        //    return cart;
        //}

    }
}