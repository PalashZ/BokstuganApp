using System;
using System.Collections.Generic;
using System.Text;
// Denna klassen representerar en bok i BokstuganApp.
// Den innehåller egenskaper som titel, författare, genre, utgivningsår, ISBN, ID, beskrivning, antal sidor, antal exemplar och status.
namespace BokstuganApp.Classes
{

    // Attributen  gör det möjligt att serialisera och deserialisera objekt av denna klass, vilket är användbart för att spara och läsa data från filer.
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
        public BookStatus.Bookstatus Status { get; set; } = BookStatus.Bookstatus.Tillgänglig;

        // Konstruktor som tar emot titel, författare, genre, utgivningsår och ISBN som parametrar.
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