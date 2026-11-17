using BokstuganApp.Classes;
using System;
using System.Collections.Generic;
using System.Text;
namespace BokstuganApp.Services
{
    internal class BookManager
    {
        private List<Book> books = new List<Book>();
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
            public IReadOnlyList<Book> GetAllBooks()
        {
            return books.AsReadOnly();
        }
            public bool RemoveBook(Book book)
        {
            return books.Remove(book);
        }

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
}

