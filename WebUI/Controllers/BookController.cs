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

        public BookController(IBookRepository repository)
        {
            this.repository = repository;
        }

        public ViewResult List()
        {
            return View(repository.Books);
        }
    }
}