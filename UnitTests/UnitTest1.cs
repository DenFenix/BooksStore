using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Controllers;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CanPaginate()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>()
            {
                 new Book {BookId = 1, Name  = "Книга1"},
                 new Book {BookId = 2, Name  = "Книга2"},
                 new Book {BookId = 3, Name  = "Книга3"},
                 new Book {BookId = 4, Name  = "Книга4"},
                 new Book {BookId = 5, Name  = "Книга5"},
                 new Book {BookId = 6, Name  = "Книга6"}
            });
            BookController controller = new BookController(mock.Object);
            controller.pageSize = 2;

            IEnumerable<Book> result = (IEnumerable<Book>)controller.List(2).Model;

            var books = result.ToList();
            Assert.IsTrue(books.Count == 2);
            Assert.AreEqual(books[0].BookId, 3);
            Assert.AreEqual(books[1].BookId, 4);

        }
    }
}
