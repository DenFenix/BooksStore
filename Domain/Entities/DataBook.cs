using Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Json
{
    public class DataBook
    {
        private List<Book> _books = new List<Book>();
        public List<Book> Books { get => _books; set => _books = value; }
        //public List<Book> Books { get; set; }
    }
}
