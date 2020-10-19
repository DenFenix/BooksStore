using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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
        [TestMethod]
        public void Can_Edit_Book()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(x => x.Books).Returns(new List<Book>
            {
                new Book {BookId =1, Name = "Book1"},
                new Book {BookId =2, Name = "Book2"},
                new Book {BookId =3, Name = "Book3"},
                new Book {BookId =4, Name = "Book4"},
                new Book {BookId =5, Name = "Book5"}
            });

            AdminController controller = new AdminController(mock.Object);
            var book1 = (Book)controller.Edit(1).ViewData.Model;
            var book2 =  controller.Edit(2).ViewData.Model as Book;
            var book3 = controller.Edit(3).ViewData.Model as Book;
            Assert.AreEqual(1, book1.BookId);
            Assert.AreEqual(1, book1.BookId);
            Assert.AreEqual(1, book1.BookId);

        }

        [TestMethod]
        public void Cannot_Edit_Nonexist_Book()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new List<Book>
            {
                new Book{BookId = 1, Name = "Book1" },
                new Book{BookId = 2, Name = "Book2" },
                new Book{BookId = 3, Name = "Book3" },
                new Book{BookId = 4, Name = "Book3" },
                new Book{BookId = 5, Name = "Book3" }
            });
            AdminController controller = new AdminController(mock.Object);

            Book resulte = controller.Edit(6).ViewData.Model as Book;

            Assert.IsNull(resulte);

        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            AdminController controller = new AdminController(mock.Object);
            Book book = new Book { Name = "Test" };
            ActionResult result = controller.Edit(book);
            mock.Verify(m => m.SaveBook(book));
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            AdminController controller = new AdminController(mock.Object);
            Book book = new Book { Name = "Test" };
            // Организация - добавление ошибки в состояние модели
            controller.ModelState.AddModelError("error", "error");
            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(book);
            // Утверждение - проверка того, что обращение к хранилищу НЕ производится
            mock.Verify(m => m.SaveBook(It.IsAny<Book>()), Times.Never());

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Books()
        {
            Book book = new Book { BookId = 2, Name = "Book2" };
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new List<Book>
            {
                new Book{BookId = 1, Name = "Book1"},
                new Book{BookId = 2, Name = "Book2"},
                new Book{BookId = 2, Name = "Book3"}
            });
            AdminController controller = new AdminController(mock.Object);
            controller.Delete(book.BookId);
            mock.Verify(m => m.DeleteBook(book.BookId));
        }
    }
}
