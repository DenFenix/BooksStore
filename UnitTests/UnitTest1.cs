using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Controllers;
using WebUI.HTMLHelpers;
using WebUI.Models;

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

            BookListViewModel bookList = (BookListViewModel)controller.List(null,2).Model;
            var result = bookList.Books.ToList();
            var books = result.ToList();
            Assert.IsTrue(books.Count == 2);
            Assert.AreEqual(books[0].BookId, 3);
            Assert.AreEqual(books[1].BookId, 4);

        }
        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new PagingInfo {CurrentPage = 2, TotalItlems = 28, ItemsPerPage = 10 };

            Func<int, string> pageUrlDelegate = i => "Page" + i;

            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }
        [TestMethod]
        public void Can_Seed_Pagination_View_Model()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(x => x.Books).Returns(new List<Book>
            {
                new Book {BookId = 1, Name  = "Книга1"},
                 new Book {BookId = 2, Name  = "Книга2"},
                 new Book {BookId = 3, Name  = "Книга3"},
                 new Book {BookId = 4, Name  = "Книга4"},
                 new Book {BookId = 5, Name  = "Книга5"},
                 new Book {BookId = 6, Name  = "Книга6"}
            });
            BookController bookController = new BookController(mock.Object);
            bookController.pageSize = 3;

            BookListViewModel result = (BookListViewModel)bookController.List(null,2).Model;

            PagingInfo pagingInfo = result.PagingInfo;
            Assert.AreEqual(pagingInfo.CurrentPage, 2);
            Assert.AreEqual(pagingInfo.ItemsPerPage, 3);
            Assert.AreEqual(pagingInfo.TotalItlems, 6);
            Assert.AreEqual(pagingInfo.TotalPages, 2);
        }
        [TestMethod]
        public void CanFilterGames()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new List<Book>
            {
                 new Book {BookId = 1, Name  = "Книга1", Category = "Cat1"},
                 new Book {BookId = 2, Name  = "Книга2", Category = "Cat2"},
                 new Book {BookId = 3, Name  = "Книга3", Category = "Cat1"},
                 new Book {BookId = 4, Name  = "Книга4", Category = "Cat2"},
                 new Book {BookId = 5, Name  = "Книга5", Category = "Cat3"},
                 new Book {BookId = 6, Name  = "Книга6", Category = "Cat1"}
            });
            BookController bookController = new BookController(mock.Object);
            bookController.pageSize = 3;

            List<Book> result = ((BookListViewModel)bookController.List("Cat2", 1).Model).Books.ToList();
            Assert.AreEqual(result.Count, 2);
            Assert.IsTrue(result[0].Name == "Книга2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "Книга4" && result[1].Category == "Cat2");
        }
    }
}
