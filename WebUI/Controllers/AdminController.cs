using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    [Authorize]
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
        public ActionResult Edit (Book book, HttpPostedFileBase image = null)
        {
            //проверяем достоверность даных
            if (ModelState.IsValid)
            {
                if(image != null)
                {
                    book.ImageMimeType = image.ContentType;
                    book.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(book.ImageData, 0, image.ContentLength);
                }
                repository.SaveBook(book);
                TempData["message"] = string.Format($"Изменение в книге {book.Name} были сохранены");
                return RedirectToAction("Index");
            }
            else
            {
                return View(book);
            }
        }
        public ViewResult Create()
        {
            return View("Edit", new Book());
        }

        [HttpPost]
        public ActionResult Delete(int deleteId)
        {
            Book deletedBook = repository.DeleteBook(deleteId);
            if (deletedBook != null)
            {
                TempData["message"] = string.Format($"Книга {deletedBook.Name} удалена");
            }
            return RedirectToAction("Index");
        }
        
    }
}