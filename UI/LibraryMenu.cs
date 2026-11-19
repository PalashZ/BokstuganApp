using System;
using BokstuganApp.Services;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using BokstuganApp.Classes;
using Spectre.Console;
using BokstuganApp.Classes.BokstuganApp.Classes;
using System.Linq;


// Denna klassen är hur meny funkar för våran program.
// Det visar vilka val användaren kan göra och tar input från användaren. Det visar menyn tills användaren väljer att avsluta.
namespace BokstuganApp.UI
{
    // Enum som representerar de olika valen i menyn.
    // Varje värde motsvarar en funktion som användaren kan välja.
    public enum MenuChoice { MyBooks, AllBooks, LoanBook, ReturnBook, Search, Review, LogOut }

    // Denna klassen är hur meny funkar för våran program.
    public class LibraryMenu
    {
        private readonly BookManager _bookManager;
        private readonly LoanManager _loanManager;
        private readonly ReviewManager _reviewManager;
        private readonly string _userName;

        public LibraryMenu(BookManager bookManager, LoanManager loanManager, ReviewManager reviewManager, string userName)
        {
            _bookManager = bookManager;
            _loanManager = loanManager;
            _reviewManager = reviewManager;
            _userName = userName;
        }

        private Book ChooseBook(string title, List<Book> books)
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<Book>()
                    .Title(title)
                    .AddChoices(books)
                    .UseConverter(book => book.Title)
            );
        }

        // Denna metoden visar menyn tills användaren väljer att logga ut.
        public void ShowMenu()
        {
            bool running = true;

            while (running)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(new Rule("[aqua]Användaremeny[/]").LeftJustified());
                AnsiConsole.WriteLine();

                // Skapar en meny med valen från MenuChoice enum och visar den i konsolen.
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<MenuChoice>()
                        .Title("[yellow]Vad vill du göra?[/]")
                        .HighlightStyle(new Style(Color.Aqua))
                        .AddChoices(Enum.GetValues<MenuChoice>())
                        .UseConverter(LibraryMenuHelper.ToText));
                // Använder en switch-sats för att hantera användarens val och anropa rätt funktion.
                switch (choice)
                {
                    case MenuChoice.MyBooks:
                        {
                            var myLoans = _loanManager.GetActiveLoansForUser(_userName);

                            if (myLoans.Count == 0)
                            {
                                ConsoleMessages.Warning("Du har inga aktiva lån.");
                            }
                            else
                            {
                                var table = new Table();

                                table.AddColumn("Boktitel");
                                table.AddColumn("Förfallodatum");
                                table.AddColumn("Status");

                                foreach (var loan in myLoans)
                                {
                                    var book = _bookManager.GetAllBooks()
                                        .FirstOrDefault(b => b.ISBN == loan.Isbn);

                                    string title = book != null ? book.Title : loan.Isbn;
                                    string status = loan.IsOverdue ? "Försenad" : "I tid";

                                    table.AddRow(
                                        Markup.Escape(title),
                                        loan.DueDate.ToString("yyyy-MM-dd"),
                                        status
                                    );
                                }

                                AnsiConsole.Write(table);
                            }

                            ConsoleMessages.WaitForKey();
                            break;
                        }

                    case MenuChoice.AllBooks:
                        new BookTable().Show(_bookManager.GetAllBooks());
                        ConsoleMessages.WaitForKey();
                        break;

                    case MenuChoice.LoanBook:
                        {
                            var availableBooks = _bookManager.GetAllBooks()
                                .Where(b => !_loanManager.IsLoaned(b.ISBN))
                                .ToList();

                            if (availableBooks.Count == 0)
                            {
                                ConsoleMessages.Warning("Det finns inga lediga böcker.");
                            }
                            else
                            {
                                var book = ChooseBook("Vilken bok vill du låna?", availableBooks);

                                try
                                {
                                    var loan = _loanManager.LoanBook(book, _userName);

                                    ConsoleMessages.Success(
                                        "Du har lånat " + book.Title +
                                        "! Förfallodatum: " +
                                        loan.DueDate.ToString("yyyy-MM-dd"));
                                }
                                catch (ArgumentException ex)
                                {
                                    ConsoleMessages.Error(ex.Message);
                                }
                            }

                            ConsoleMessages.WaitForKey();
                            break;
                        }

                    case MenuChoice.Search:
                    {
                        Console.Write("Sök på titel eller författare (Enter visar alla): ");
                        string? searchText = Console.ReadLine();

                        var results = new SearchService().SearchByText(
                            _bookManager.GetAllBooks(), searchText);
                        if (results.Count == 0)
                        {
                            ConsoleMessages.Warning("Inga böcker hittades.");
                        }
                        else
                        {
                            new BookTable().Show(results);
                        }

                        ConsoleMessages.WaitForKey();
                        break;

                    }
                    case MenuChoice.ReturnBook:
                        {
                            var activeLoans = _loanManager.GetActiveLoansForUser(_userName);

                            if (activeLoans.Count == 0)
                            {
                                ConsoleMessages.Warning("Du har inga böcker att lämna in.");
                            }
                            else
                            {
                                var loan = AnsiConsole.Prompt(
                                    new SelectionPrompt<Loan>()

                                        .Title("[yellow]Vilken bok vill du lämna in?[/]")
                                        .AddChoices(activeLoans)
                                        .UseConverter(l => Markup.Escape(
                                            l.Isbn + " (förfaller " +
                                            l.DueDate.ToString("yyyy-MM-dd") + ")")));

                                bool wasLate = loan.IsOverdue;

                                try
                                {
                                    _loanManager.ReturnBook(loan.Isbn, _userName);

                                    if (wasLate)
                                    {
                                        ConsoleMessages.Warning(
                                            "Boken har lämnats in, men den var försenad.");
                                    }
                                    else
                                    {
                                        ConsoleMessages.Success(
                                            "Boken har lämnats in. Tack!");
                                    }
                                }
                                catch (ArgumentException ex)
                                {
                                    ConsoleMessages.Error(ex.Message);
                                }
                            }

                            ConsoleMessages.WaitForKey();
                            break;
                        }

                    case MenuChoice.Review:
                        {
                            var borrowedBooks = _bookManager.GetAllBooks()
                                .Where(b => _loanManager.HasBorrowed(b.ISBN, _userName))
                                .ToList();

                            if (borrowedBooks.Count == 0)
                            {
                                ConsoleMessages.Warning("Du har inte lånat några böcker, så det finns inget att recensera.");
                            }
                            else
                            {
                                var book = ChooseBook("Vilken bok vill du recensera?", borrowedBooks);
                                int rating = InputHelper.AskInt("Betyg (1-5)", 1, 5);
                                string comment = InputHelper.AskText("Kommentar");

                                try
                                {
                                    _reviewManager.AddReview(book.ISBN, _userName, rating, comment);
                                    ConsoleMessages.Success("Tack för din recension!");
                                }
                                catch (ArgumentException ex)
                                {
                                    ConsoleMessages.Error(ex.Message);
                                }
                            }

                            ConsoleMessages.WaitForKey();
                            break;
                        }


                    case MenuChoice.LogOut:
                        running = false;
                        break;

                   
                }

            }

        }

        private static Book book(string title, List<Book> books)
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<Book>()
                    .Title("[yellow]" + title + "[/]")
                    .AddChoices(books)
                    .UseConverter(b => Markup.Escape(b.Title + " (" + b.Author + ")")));
        }

        // Hittar titeln utifrån ISBN, annars visas ISBN
        private string TitleFor(string isbn)
        {
            var book = _bookManager.GetAllBooks().FirstOrDefault(b => b.ISBN == isbn);
            return book == null ? isbn : book.Title;
        }
    }
}
