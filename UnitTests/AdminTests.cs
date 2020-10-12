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
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Games()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(x=>x.Books).Returns(new List<Book>
            {
                new Book {BookId =1, Name = "Book1"},
                new Book {BookId =2, Name = "Book2"},
                new Book {BookId =3, Name = "Book3"},
                new Book {BookId =4, Name = "Book4"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            //Действие
            List<Book> result = ((IEnumerable<Book>)controller.Index().ViewData.Model).ToList();

            Assert.AreEqual(result.Count, 4);
            Assert.AreEqual("Book1", result[0].Name);
            Assert.AreEqual("Book2", result[1].Name);
            Assert.AreEqual("Book3", result[2].Name);
        }
    }
}
