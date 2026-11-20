using System;
using System.Collections.Generic;
using System.Linq;
using BokstuganApp.Classes;
using BokstuganApp.Services;
using Spectre.Console;

namespace BokstuganApp.UI;

public enum AdminChoice { ShowAll, Statistics, ShowTickets, ShowUsers, AddBook, EditBook, RemoveBook, RemoveUser, LogOut }

public class AdminMenu
{
    private readonly BookManager _bookManager;
    private readonly LoanManager _loanManager;
    private readonly DataStore<User> _userStore;
    private readonly DataStore<Ticket> _ticketStore;

    public AdminMenu(BookManager bookManager, LoanManager loanManager, DataStore<User> userStore, DataStore<Ticket> ticketStore)
    {
        _bookManager = bookManager;
        _loanManager = loanManager;
        _userStore = userStore;
        _ticketStore = ticketStore;
    }

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

            switch (choice)
            {
                case AdminChoice.ShowAll: ShowAllBooks(); break;
                case AdminChoice.Statistics: ShowStatistics(); break;
                case AdminChoice.ShowTickets: ShowTickets(); break;
                case AdminChoice.ShowUsers: ShowUsers(); break;
                case AdminChoice.AddBook: AddBook(); break;
                case AdminChoice.EditBook: EditBook(); break;
                case AdminChoice.RemoveBook: RemoveBook(); break;
                case AdminChoice.RemoveUser: RemoveUser(); break;
                case AdminChoice.LogOut: running = false; break;
            }
        }
    }

    private void ShowStatistics()
    {
        new StatisticsView().ShowBooksPerGenre(_bookManager.GetAllBooks());
        ConsoleMessages.WaitForKey();
    }

    private void ShowTickets()
    {
        const string back = "Tillbaka till adminmenyn";

        while (true)
        {
            var tickets = _ticketStore.GetAll().OrderByDescending(ticket => ticket.CreatedAt).ToList();
            if (tickets.Count == 0)
            {
                ConsoleMessages.Info("Det finns inga inkomna tickets.");
                ConsoleMessages.WaitForKey();
                return;
            }

            var choices = tickets.Select((ticket, index) => new
                {
                    Ticket = ticket,
                    Label = $"{ticket.CreatedAt:yyyy-MM-dd HH:mm} — {ticket.UserName} ({index + 1})"
                })
                .ToDictionary(item => item.Label, item => item.Ticket);
            choices.Add(back, null!);

            string selected = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Välj en ticket att öppna:")
                .HighlightStyle(new Style(Color.Aqua))
                .AddChoices(choices.Keys));
            if (selected == back)
                return;

            var ticket = choices[selected];
            AnsiConsole.Write(new Panel(
                    $"[bold]Från:[/] {Markup.Escape(ticket.UserName)}\n" +
                    $"[bold]Datum:[/] {ticket.CreatedAt:yyyy-MM-dd HH:mm}\n\n" +
                    Markup.Escape(ticket.Message))
                .Header("Ticket")
                .Border(BoxBorder.Rounded)
                .BorderColor(Color.Aqua)
                .Padding(1, 1));

            string action = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Vad vill du göra?")
                .AddChoices("Ta bort ticket", "Tillbaka till ticketlistan"));

            if (action == "Ta bort ticket" && InputHelper.Confirm("Vill du verkligen ta bort den här ticketen?"))
            {
                _ticketStore.RemoveItem(ticket);
                _ticketStore.SaveToJson();
                ConsoleMessages.Success("Ticketen har tagits bort.");
            }
        }
    }

    private void ShowAllBooks()
    {
        var books = _bookManager.GetAllBooks();
        if (books.Count == 0)
        {
            ConsoleMessages.Warning("Det finns inga böcker.");
            ConsoleMessages.WaitForKey();
            return;
        }

        var table = new Table().AddColumn("Bok").AddColumn("Författare").AddColumn("Lånad av").AddColumn("Förfallodatum");
        foreach (var book in books)
        {
            var loan = _loanManager.GetActiveLoansForBook(book.ISBN);
            table.AddRow(
                Markup.Escape(book.Title),
                Markup.Escape(book.Author),
                loan == null ? "Tillgänglig" : Markup.Escape(loan.Borrower),
                loan == null ? "—" : loan.DueDate.ToString("yyyy-MM-dd"));
        }
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
        new StatisticsView().ShowBooksPerGenre(books);

        var activeLoans = books
            .Select(book => new { Book = book, Loan = _loanManager.GetActiveLoansForBook(book.ISBN) })
            .Where(item => item.Loan != null)
            .ToList();
        if (activeLoans.Count > 0)
        {
            var choices = activeLoans.Select((item, index) => new
            {
                item.Loan,
                Choice = $"Markera {item.Book.Title} som återlämnad — {item.Loan!.Borrower} ({index + 1})"
            }).ToDictionary(item => item.Choice, item => item.Loan!);
            const string back = "Tillbaka till adminmenyn";
            choices.Add(back, null!);
            string selected = AnsiConsole.Prompt<string>(new SelectionPrompt<string>()
                .Title("Välj ett lån att avsluta, eller gå tillbaka:")
                .AddChoices(choices.Keys));
            if (selected != back)
            {
                var loan = choices[selected];
                if (InputHelper.Confirm($"Markera {loan.Isbn} som återlämnad av {loan.Borrower}?"))
                {
                    _loanManager.ReturnBook(loan.Isbn, loan.Borrower);
                    ConsoleMessages.Success("Lånet har avslutats och boken är tillgänglig igen.");
                }
            }
        }
        else
        {
            ConsoleMessages.Info("Inga böcker är utlånade just nu.");
            ConsoleMessages.WaitForKey();
        }
    }

    private void ShowUsers()
    {
        var users = _userStore.GetAll().OrderBy(user => user.Name, StringComparer.OrdinalIgnoreCase).ToList();
        if (users.Count == 0)
            ConsoleMessages.Warning("Det finns inga registrerade användare ännu.");
        else
        {
            var table = new Table().AddColumn("Användare").AddColumn("Aktiva lån");
            foreach (var user in users)
                table.AddRow(Markup.Escape(user.Name), _loanManager.GetActiveLoansForUser(user.Name).Count.ToString());
            AnsiConsole.Write(table);
        }
        ConsoleMessages.WaitForKey();
    }

    private void RemoveUser()
    {
        var users = _userStore.GetAll().OrderBy(user => user.Name, StringComparer.OrdinalIgnoreCase).ToList();
        if (users.Count == 0)
        {
            ConsoleMessages.Warning("Det finns inga användare att ta bort.");
            ConsoleMessages.WaitForKey();
            return;
        }

        var choices = users.Select((user, index) => new { User = user, Choice = $"{user.Name} ({index + 1})" })
            .ToDictionary(item => item.Choice, item => item.User);
        const string back = "Tillbaka till adminmenyn";
        choices.Add(back, null!);
        var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Vilken användare vill du ta bort?")
            .AddChoices(choices.Keys));
        if (choice == back) return;

        var selectedUser = choices[choice];
        var activeLoanCount = _loanManager.GetActiveLoansForUser(selectedUser.Name).Count;
        if (!InputHelper.Confirm($"Ta bort {selectedUser.Name} och radera användarens lånehistorik? Aktiva lån: {activeLoanCount}."))
            return;

        _loanManager.RemoveLoansForUser(selectedUser.Name);
        _userStore.RemoveItem(selectedUser);
        _userStore.SaveToJson();
        ConsoleMessages.Success($"Användaren {selectedUser.Name} och dess lån har tagits bort.");
        ConsoleMessages.WaitForKey();
    }

    private void AddBook()
    {
        var action = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Skapa en ny bok?")
            .AddChoices("Fortsätt", "Tillbaka till adminmenyn"));
        if (action == "Tillbaka till adminmenyn") return;

        string title = InputHelper.AskText("Titel");
        string author = InputHelper.AskText("Författare");
        string genre = InputHelper.AskText("Genre");
        int year = InputHelper.AskInt("År", 1000, DateTime.Now.Year);
        string isbn = InputHelper.AskText("ISBN");
        try
        {
            _bookManager.AddBook(new Book(title, author, genre, year, isbn));
            ConsoleMessages.Success("Boken har lagts till.");
        }
        catch (ArgumentException ex) { ConsoleMessages.Error(ex.Message); }
        ConsoleMessages.WaitForKey();
    }

    private void EditBook()
    {
        Book? book = ChooseBook("Vilken bok vill du ändra?");
        if (book == null) return;
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
        catch (ArgumentException ex) { ConsoleMessages.Error(ex.Message); }
        ConsoleMessages.WaitForKey();
    }

    private void RemoveBook()
    {
        Book? book = ChooseBook("Vilken bok vill du ta bort?");
        if (book == null) return;
        if (InputHelper.Confirm("Vill du verkligen ta bort \"" + book.Title + "\"?"))
            ConsoleMessages.Success(_bookManager.RemoveBook(book) ? "Boken har tagits bort." : "Boken fanns inte.");
        ConsoleMessages.WaitForKey();
    }

    private Book? ChooseBook(string title)
    {
        var books = _bookManager.GetAllBooks();
        if (books.Count == 0)
        {
            ConsoleMessages.Warning("Det finns inga böcker.");
            return null;
        }
        var choices = books.Select((item, index) => new { Book = item, Choice = $"{item.Title} — {item.Author} ({index + 1})" })
            .ToDictionary(item => item.Choice, item => item.Book);
        const string back = "Tillbaka till adminmenyn";
        choices.Add(back, null!);
        var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("[yellow]" + title + "[/]")
            .AddChoices(choices.Keys));
        return choice == back ? null : choices[choice];
    }

    private static string ToText(AdminChoice choice) => choice switch
    {
        AdminChoice.ShowAll => "Visa alla böcker",
        AdminChoice.Statistics => "Statistik",
        AdminChoice.ShowTickets => "Visa tickets",
        AdminChoice.ShowUsers => "Visa användare",
        AdminChoice.AddBook => "Lägg till bok",
        AdminChoice.EditBook => "Ändra bok",
        AdminChoice.RemoveBook => "Ta bort bok",
        AdminChoice.RemoveUser => "Ta bort användare",
        _ => "Logga ut"
    };
}
