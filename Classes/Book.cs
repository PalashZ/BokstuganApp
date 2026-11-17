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
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public int PageCount { get; set; }
        public int Copies { get; set; }
        public Book(string title, string author, string genre, int year, string isbn)
        {
            // Kontrollera att bokens titel inte är tom.
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Bokens titel får inte vara tom.");
            }

            Title = title;
            Author = author;
            Genre = genre;

            // Kontrollera att utgivningsåret är rimligt.
            if (year < 0 || year > DateTime.Now.Year)
            {
                throw new ArgumentException("Bokens utgivningsår är ogiltigt.");
            }

            Year = year;
            ISBN = isbn;
        }
    }
}