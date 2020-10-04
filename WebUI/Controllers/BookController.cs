using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class BookController : Controller
    {
        private IBookRepository repository;
        //на одной странице 5 товаров
        public int pageSize = 3;

        public BookController(IBookRepository repository)
        {
            this.repository = repository;
        }

        public ViewResult List(int page = 1)
        {
            return View(repository.Books
                .OrderBy(book=>book.BookId)
                .Skip((page-1)*pageSize)
                .Take(pageSize));
        }
    }
}