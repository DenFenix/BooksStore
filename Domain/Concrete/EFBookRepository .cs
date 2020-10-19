using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class EFBookeRepository : IBookRepository
    {
        EFDbContext context = new EFDbContext();
        public IEnumerable<Book> Books { get { return context.Books; }}

        public Book DeleteBook(int bookId)
        {
            Book dbEntry = context.Books.Find(bookId);
            if(dbEntry != null)
            {
                context.Books.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public void SaveBook(Book book)
        {
            if (book.BookId == 0)
                context.Books.Add(book);
            else
            {
                Book dbEntrty = context.Books.Find(book.BookId);
                if(dbEntrty!=null)
                {
                    dbEntrty.Name = book.Name;
                    dbEntrty.Author = book.Author;
                    dbEntrty.Description = book.Description;
                    dbEntrty.Price = book.Price;
                    dbEntrty.Category = book.Category;
                }
            }
            context.SaveChanges();
        }
    }
}
