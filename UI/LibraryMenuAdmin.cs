using System;
using System.Collections.Generic;
using System.Text;
using BokstuganApp.Classes;
using BokstuganApp.Services;
using Spectre.Console;
namespace BokstuganApp.UI;
// Denna klassen är menyn för admin. Den hanterar visning, tillägg, ändring och borttagning av böcker.
public enum AdminChoice { ShowAll, AddBook, EditBook, RemoveBook, LogOut }

// Menyn för admin: visa, lägga till, ändra och ta bort böcker
public class AdminMenu
{
    private readonly BookManager _bookManager;

    public AdminMenu(BookManager bookManager) => _bookManager = bookManager;

    // Denna metoden visar menyn tills användaren väljer att logga ut. Den använder Spectre.Console för att skapa en snygg meny med färger och val.
    public void ShowMenu()
    {
        bool running = true;

        while (running)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[aqua]Adminmeny[/]").LeftJustified());
            AnsiConsole.WriteLine();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<AdminChoice>()
                    .Title("[yellow]Vad vill du göra?[/]")
                    .HighlightStyle(new Style(Color.Aqua))
                    .AddChoices(Enum.GetValues<AdminChoice>())
                    .UseConverter(ToText));
            // Använder en switch-sats för att hantera användarens val och anropa rätt funktion.

            switch (choice)
            {
                case AdminChoice.ShowAll:
                    new BookTable().Show(_bookManager.GetAllBooks());
                    ConsoleMessages.WaitForKey();
                    break;
                case AdminChoice.AddBook:
                    AddBook();
                    break;
                case AdminChoice.EditBook:
                    EditBook();
                    break;
                case AdminChoice.RemoveBook:
                    RemoveBook();
                    break;
                case AdminChoice.LogOut:
                    running = false;
                    break;
            }
        }
    }

    // Frågar efter alla fält och lägger till boken
    private void AddBook()
    {
        string title = InputHelper.AskText("Titel");
        string author = InputHelper.AskText("Författare");
        string genre = InputHelper.AskText("Genre");
        int year = InputHelper.AskInt("År", 1000, DateTime.Now.Year);
        string isbn = InputHelper.AskText("ISBN");

        // Använder try-catch för att hantera eventuella undantag som kan uppstå vid tillägg av boken. Om ett undantag uppstår, visas ett felmeddelande i konsolen.
        try
        {
            _bookManager.AddBook(new Book(title, author, genre, year, isbn));
            ConsoleMessages.Success("Boken har lagts till.");
        }
        catch (ArgumentException ex)
        {
            ConsoleMessages.Error(ex.Message);
        }

        ConsoleMessages.WaitForKey();
    }

    // Låter admin välja en bok och skriva in nya värden
    private void EditBook()
    {
        Book? book = ChooseBook("Vilken bok vill du ändra?");
        if (book == null)
        {
            ConsoleMessages.WaitForKey();
            return;
        }
        // Frågar efter nya värden för boken och använder try-catch för att hantera eventuella undantag som kan uppstå vid ändring av boken.
        // Om ett undantag uppstår, visas ett felmeddelande i konsolen.
        string title = InputHelper.AskText("Ny titel");
        string author = InputHelper.AskText("Ny författare");
        string genre = InputHelper.AskText("Ny genre");
        int year = InputHelper.AskInt("Nytt år", 1000, DateTime.Now.Year);
        string isbn = InputHelper.AskText("Nytt ISBN");

        try
        {
            _bookManager.EditBook(book, title, author, genre, year, isbn);
            ConsoleMessages.Success("Boken har ändrats.");
        }
        catch (ArgumentException ex)
        {
            ConsoleMessages.Error(ex.Message);
        }

        ConsoleMessages.WaitForKey();
    }

    // Låter admin välja en bok och bekräfta borttagningen
    private void RemoveBook()
    {
        Book? book = ChooseBook("Vilken bok vill du ta bort?");
        if (book == null)
        {
            ConsoleMessages.WaitForKey();
            return;
        }

        if (InputHelper.Confirm("Vill du verkligen ta bort \"" + book.Title + "\"?"))
        {
            if (_bookManager.RemoveBook(book))
            {
                ConsoleMessages.Success("Boken har tagits bort.");
            }
            else
            {
                ConsoleMessages.Warning("Boken fanns inte.");
            }
        }

        ConsoleMessages.WaitForKey();
    }

    // Visar en pilmeny med alla böcker. Ger null om det inte finns några
    private Book? ChooseBook(string title)
    {
        var books = _bookManager.GetAllBooks();

        if (books.Count == 0)
        {
            ConsoleMessages.Warning("Det finns inga böcker.");
            return null;
        }

        return AnsiConsole.Prompt(
            new SelectionPrompt<Book>()
                .Title("[yellow]" + title + "[/]")
                .AddChoices(books)
                .UseConverter(b => Markup.Escape(b.Title + " (" + b.Author + ")")));
    }

    // Konverterar AdminChoice till en sträng som kan visas i menyn. Använder en switch-sats för att returnera rätt text beroende på valet.
    private static string ToText(AdminChoice choice)
    {
        switch (choice)
        {
            case AdminChoice.ShowAll: return "Visa alla böcker";
            case AdminChoice.AddBook: return "Lägg till bok";
            case AdminChoice.EditBook: return "Ändra bok";
            case AdminChoice.RemoveBook: return "Ta bort bok";
            default: return "Logga ut";
        }
    }
}