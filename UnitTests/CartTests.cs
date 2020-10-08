using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
