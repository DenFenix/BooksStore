using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class AdminController : Controller
    {
        IBookRepository repository;

        public AdminController (IBookRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Books);
        }

        public ViewResult Edit(int bookId)
        {
            Book book = repository.Books.FirstOrDefault(b => b.BookId == bookId);
            return View(book);
        }
        
        /// <summary>
        /// Перегруженный edit
        /// </summary>
        /// <param name="book"></param>
        /// <returns>перенаправляет бразуер на индекс</returns>
        [HttpPost]
        public ActionResult Edit (Book book)
        {
            //проверяем достоверность даных
            if (ModelState.IsValid)
            {
                repository.SaveBook(book);
                TempData["message"] = string.Format($"Изменение в книге {book.Name} были сохранены");
                return RedirectToAction("Index");
            }
            else
            {
                return View(book);
            }
        }
    }
}