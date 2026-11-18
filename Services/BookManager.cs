using BokstuganApp.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;
using BokstuganApp.Classes;
namespace BokstuganApp.Services;

// Denna klassen hanterar logiken för att lägga till, ta bort och redigera böcker i BokstuganApp.
// Den använder sig av DataStore för att spara och läsa böcker från en JSON-fil.
// Denna klassen innehåller metoder för att hämta alla böcker, lägga till en bok, ta bort en bok och redigera en bok. Ett ställe där man kan managea alla böcker
public class BookManager
    {
    // Denna klassen hanterar logiken för att lägga till, ta bort och redigera böcker i BokstuganApp.
    private List<Book> books = new List<Book>();
        private DataStore<Book> bookStore;

        public BookManager(DataStore<Book> bookStore)
        {
            this.bookStore = bookStore;
        }

        // Ger en kopia av listan, så att ingen utifrån kan ändra den direkt
        public List<Book> GetAllBooks()
        {
            return new List<Book>(books);
        }
    // Denna metoden lägger till en bok i listan över böcker. Den kontrollerar att boken inte är null och att titeln inte är tom.
    public void AddBook(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }
            if (string.IsNullOrWhiteSpace(book.Title))
            {
                throw new ArgumentException("Bokens titel får inte vara tom.");
            }

            books.Add(book);
        }
    // Denna metoden tar bort en bok från listan över böcker. Den returnerar true om boken togs bort, annars false.
    public bool RemoveBook(Book book)
        {
            return books.Remove(book);
        }
    // Denna metoden redigerar en bok i listan över böcker. Den kontrollerar att boken finns i listan, att den nya titeln inte är tom och att det nya utgivningsåret är giltigt.
    public void EditBook(Book book, string newTitle,
            string newAuthor, string newGenre,
            int newYear, string newISBN)
        {
            if (book == null || !books.Contains(book))
            {
                throw new ArgumentException(
                    "Boken finns inte i biblioteket.");
            }

            if (string.IsNullOrWhiteSpace(newTitle))
            {
                throw new ArgumentException(
                    "Bokens titel får inte vara tom.");
            }

            if (newYear <= 0 || newYear > DateTime.Now.Year)
            {
                throw new ArgumentException(
                    "Bokens utgivningsår är ogiltigt.");
            }

            book.Title = newTitle;
            book.Author = newAuthor;
            book.Genre = newGenre;
            book.Year = newYear;
            book.ISBN = newISBN;
        }

    }


