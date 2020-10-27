using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Json
{
    public class JSONBookRepository : IBookRepository
    {
        DataBook context;

        string jsonResult;
        string path = /*"user.json"*/Directory.GetCurrentDirectory();
        public JSONBookRepository()
        {
            try
            {
                context = JsonSerializer.Deserialize<DataBook>(Read());
            }
            catch (JsonException)
            {
                context = null;
            }

            if (context == null)
                context = new DataBook();
        }

        public IEnumerable<Book> Books { get { return context.Books; } }

        public Book DeleteBook(int bookId)
        {
            Book dbEntry = context.Books.FirstOrDefault(b => b.BookId == bookId);

            if (dbEntry != null)
            {
                context.Books.Remove(dbEntry);
                string jSONString = JsonSerializer.Serialize<DataBook>(context);
                Write(jSONString);
            }
            return dbEntry;
        }

        public void SaveBook(Book book)
        {
            if (book.BookId == 0)
            {
                try
                {
                    book.BookId = context.Books.Max(x => x.BookId);
                }
                catch(InvalidOperationException)
                {
                    book.BookId = 1;
                }    
                context.Books.Add(book);
            }            
            else
            {
                Book dbEntrty = context.Books.FirstOrDefault(b=>b.BookId == book.BookId);
                dbEntrty.Name = book.Name;
                dbEntrty.Author = book.Author;
                dbEntrty.Description = book.Description;
                dbEntrty.Price = book.Price;
                dbEntrty.Category = book.Category;
                dbEntrty.ImageData = book.ImageData;
                dbEntrty.ImageMimeType = book.ImageMimeType;
            }
            string jSONString = JsonSerializer.Serialize<DataBook>(context);
            Write(jSONString);
        }

        private string Read()
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                using (StreamReader streamReader = new StreamReader(fs))
                {
                    jsonResult = streamReader.ReadToEnd();
                }
            }

            return jsonResult;
        }

        public void Write(string jSONString)
        {
            using (var streamWriter = File.CreateText(path))
            {
                streamWriter.Write(jSONString);
            }
        }
    }
}
