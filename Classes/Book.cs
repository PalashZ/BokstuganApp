using System;
using System.Collections.Generic;
using System.Text;

namespace BokstuganApp.Classes
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public string ISBN { get; set; }
        public Book(string title, string author, string genre, int year, string isbn)
        {
            Title = title;
            Author = author;
            Genre = genre;
            Year = year;
            ISBN = isbn;
        }
    }
}