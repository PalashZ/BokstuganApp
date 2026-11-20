using BokstuganApp.Classes;
using BokstuganApp.UI;
using System;
using System.Collections.Generic;

namespace BokstuganApp.Services;

// Denna klassen hanterar logiken för att lägga till, ta bort och redigera böcker i BokstuganApp.
// Den använder sig av DataStore för att spara och läsa böcker från en JSON-fil.
// Ett ställe där man kan managea alla böcker.
public class BookManager
{
    // Lagringen som håller böckerna och sparar/läser dem från JSON.
    // All data ligger här. Klassen har ingen egen lista.
    private readonly DataStore<Book> bookStore;

    // Konstruktör för klassen "BookManager"
    public BookManager(DataStore<Book> bookStore)
    {
        this.bookStore = bookStore;

        // Läser in böcker som sparades förra gången programmet kördes.
        // Finns ingen fil än blir listan bara tom.
        this.bookStore.LoadFromJson();

        // Lägg till det versionshanterade grundpaketet även i befintliga lokala bibliotek.
        // Kontrollerar både ID och ISBN så en bok inte läggs in två gånger.
        var books = this.bookStore.GetAll();
        bool addedSeedBooks = false;

       // "foreach" används här för att kunna se och användas ab böckernas id/ISBN
        foreach (var seedBook in LibraryMenuHelper.GetDemoBooks())
        {
            if (books.Any(book => book.Id == seedBook.Id || book.ISBN == seedBook.ISBN))
                continue;

            this.bookStore.AddItem(seedBook);
            books.Add(seedBook);
            addedSeedBooks = true;
        }

        if (addedSeedBooks)
            this.bookStore.SaveToJson();
    }

    // DataStore.GetAll() ger redan en kopia av listan,
    // så ingen utifrån kan ändra den direkt
    public List<Book> GetAllBooks()
    {
        return bookStore.GetAll();
    }

    // Denna metoden lägger till en bok. Den kontrollerar att boken inte är null och att titeln inte är tom.
    // Efter att boken lagts till sparas listan till JSON-filen.
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

        bookStore.AddItem(book);
        bookStore.SaveToJson();
    }

    // Denna metoden tar bort en bok. Den returnerar true om boken togs bort, annars false.
    // Listan sparas bara om något faktiskt togs bort.
    public bool RemoveBook(Book book)
    {
        bool removed = bookStore.RemoveItem(book);
        if (removed)
        {
            bookStore.SaveToJson();
        }
        return removed;
    }

    // Denna metoden redigerar en bok. Den kontrollerar att boken finns, att den nya titeln inte är tom
    // och att det nya utgivningsåret är giltigt. Efter ändringen sparas listan till JSON-filen.
    public void EditBook(Book book, string newTitle,
        string newAuthor, string newGenre,
        int newYear, string newISBN)
    {
        if (book == null || !bookStore.GetAll().Contains(book))
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

        bookStore.SaveToJson();
    }
}
