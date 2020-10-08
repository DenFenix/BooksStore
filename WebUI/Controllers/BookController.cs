using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;

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

        public ViewResult List(string category, int page = 1)
        {
            //return View(repository.Books
            //    .OrderBy(book=>book.BookId)
            //    .Skip((page-1)*pageSize)
            //    .Take(pageSize));
            BookListViewModel bookListViewModel = new BookListViewModel();
            bookListViewModel.Books = repository.Books
                .Where(p=>p.Category ==null || p.Category == category)
                .OrderBy(book=>book.BookId)
                .Skip((page-1)*pageSize)
                .Take(pageSize);

            bookListViewModel.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItlems =category == null?
                repository.Books.Count() :
                repository.Books.Where(book=>book.Category == category).Count()
            };
            bookListViewModel.CurrentCategory = category;
            return View(bookListViewModel);
        }
    }
}