using System;
using BokstuganApp.Services;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using BokstuganApp.Classes;
using Spectre.Console;
using System.Linq;


// Denna klassen är hur meny funkar för våran program.
// Det visar vilka val användaren kan göra och tar input från användaren. Det visar menyn tills användaren väljer att avsluta.
namespace BokstuganApp.UI
{
    // Enum som representerar de olika valen i menyn.
    // Varje värde motsvarar en funktion som användaren kan välja.
   public enum MenuChoice
{
    MyBooks, AllBooks, RandomBook, ReturnBook,
    Search, Review, Ticket, Reading, LogOut
}
    public enum BookDetailsAction { Loan, BackToBooks, BackToMenu }

    // Denna klassen är hur meny funkar för våran program.
    public class LibraryMenu
    {
        private readonly BookManager _bookManager;
        private readonly LoanManager _loanManager;
        private readonly ReviewManager _reviewManager;
        private readonly string _userName;
        private readonly DataStore<Ticket> _ticketStore;
        private readonly ReadingStatusManager _readingStatusManager;
        public LibraryMenu(BookManager bookManager, LoanManager loanManager, ReviewManager reviewManager, string userName, DataStore<Ticket> ticketStore)
        {
            _bookManager = bookManager;
            _loanManager = loanManager;
            _reviewManager = reviewManager;
            _userName = userName;
            _ticketStore = ticketStore;
            _readingStatusManager = new ReadingStatusManager(
    new DataStore<UserReadingStatus>("reading-status.json"));
        }

        private Book? ChooseBook(string title, List<Book> books)
        {
            var choices = books
                .Select((item, index) => new { Book = item, Choice = $"{item.Title} — {item.Author} ({index + 1})" })
                .ToDictionary(item => item.Choice, item => item.Book);
            const string backChoice = "Tillbaka";
            choices.Add(backChoice, null!);

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title(title)
                    .AddChoices(choices.Keys)
            );
            return choice == backChoice ? null : choices[choice];
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
                                table.AddColumn("Lånad under namn");
                                table.AddColumn("Lånetid");
                                table.AddColumn("Lånad från");
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
                                        Markup.Escape(loan.Borrower),
                                        (loan.DueDate.Date - loan.LoanDate.Date).Days + " dagar",
                                        loan.LoanDate.ToString("yyyy-MM-dd"),
                                        loan.DueDate.ToString("yyyy-MM-dd"),
                                        status
                                    );
                                }

                                AnsiConsole.Write(table);
                            }

                            ConsoleMessages.WaitForKey();
                            break;
                        }

                        // Detta visar alla böcker för användraren
                    case MenuChoice.AllBooks:
                        BrowseBooks();
                        break;

                        // Väljer en ledig bok slumpmässigt och visar den som ett boktips till användaren
                    case MenuChoice.RandomBook:
                    {
                            // Detta hämtar bara de böcker som inte är utlånad
                        var books = _bookManager.GetAllBooks()
                            .Where(book => !_loanManager.IsLoaned(book.ISBN))
                            .ToList();
                        if (books.Count == 0)
                        {
                            ConsoleMessages.Warning("Det finns inga tillgängliga böcker att välja mellan.");
                            ConsoleMessages.WaitForKey();
                            break;
                        }

                        var selectedBook = Random.Shared.GetItems(books.ToArray(), 1)[0];
                        AnsiConsole.Clear();
                        AnsiConsole.MarkupLine("[aqua]Ditt slumpmässiga boktips:[/]");
                        ShowBookDetails(selectedBook);
                        break;
                    }

                        // Söker bland böckerna på titel eller författare för att hitta boken
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

                        // Låter användaren lämna in en av sina lånade böcker
                    case MenuChoice.ReturnBook:
                        {
                            var activeLoans = _loanManager.GetActiveLoansForUser(_userName);

                            if (activeLoans.Count == 0)
                            {
                                ConsoleMessages.Warning("Du har inga böcker att lämna in.");
                            }
                            else
                            {

                                // Skapar valen i menyn, det som behövs för att skapa en bok
                                var loanChoices = activeLoans
                                    .Select((item, index) => new
                                    {
                                        Loan = item,
                                        Choice = $"{TitleFor(item.Isbn)} (förfaller {item.DueDate:yyyy-MM-dd}) ({index + 1})"
                                    })
                                    .ToDictionary(item => item.Choice, item => item.Loan);
                                const string backChoice = "Tillbaka till menyn";
                                loanChoices.Add(backChoice, null!);

                                // Visar en pilmeny där användaren väljer vilken bok som ska lämnas in
                                var loanChoice = AnsiConsole.Prompt(
                                    new SelectionPrompt<string>()
                                        .Title("[yellow]Vilken bok vill du lämna in?[/]")
                                        .AddChoices(loanChoices.Keys));
                                if (loanChoice == backChoice) break;
                                var loan = loanChoices[loanChoice];

                                bool wasLate = loan.IsOverdue;

                                // Försöker lämna in boken och fångar fel så att programmmet inte kraschar
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

                        // Låter användaren recensera en bok som användaren har lånat/läst

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
                                // Låter användaren välja vilken bok som ska recenseras
                                var book = ChooseBook("Vilken bok vill du recensera?", borrowedBooks);
                                if (book == null) break;
                                int rating = InputHelper.AskInt("Betyg (1-5)", 1, 5);
                                string comment = InputHelper.AskText("Kommentar");

                                // Försöker spara recensionen och fångar regelfel
                                // Det hade stoppat och visat fel om någon lämnar dubbla recension
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
case MenuChoice.Reading:
{
    var books = _bookManager.GetAllBooks();

    if (books.Count == 0)
    {
        ConsoleMessages.Warning("Det finns inga böcker.");
        ConsoleMessages.WaitForKey();
        break;
    }

    var selectedBook = ChooseBook("Välj bok för lässtatus:", books);
    if (selectedBook == null)
    {
        break;
    }

    var currentStatus = _readingStatusManager.GetStatus(
        _userName, selectedBook.ISBN);

    Console.WriteLine(
        $"Din lässtatus för {selectedBook.Title}: {currentStatus}");

    string action = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Vad vill du göra?")
            .AddChoices("Markera som läst", "Markera som oläst", "Tillbaka"));

    if (action == "Tillbaka")
    {
        break;
    }

    var status = action == "Markera som läst"
        ? ReadingStatus.Läst
        : ReadingStatus.Oläst;

    _readingStatusManager.SetStatus(
        _userName, selectedBook.ISBN, status);

    ConsoleMessages.Success("Lässtatusen har sparats.");
    ConsoleMessages.WaitForKey();
    break;
}
                        // Avslutar menyloopen så att användaren kommer tillbaka till rollvalet
                    case MenuChoice.LogOut:
                        running = false;
                        break;

                        // Låter användaren skicka en ticket till admin
                    case MenuChoice.Ticket:
                        SubmitTicket();
                        break;


                }

            }

        }

        // Hjälper till för att skicka ticket
        private void SubmitTicket()
        {
            const string sendChoice = "Skicka en ticket";
            const string backChoice = "Tillbaka till menyn";

            //  visar en pilmeny, skicka ticket eller gå tillbaka
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Ticket[/]")
                    .HighlightStyle(new Style(Color.Aqua))
                    .AddChoices(sendChoice, backChoice));

            if (choice == backChoice)
            {
                return;
            }

            // Rad 299-310, handlar om hur ticket flödet ska funka
            string message = InputHelper.AskText("Beskriv felet eller önskad bok");
            var tickets = _ticketStore.GetAll();
            _ticketStore.AddItem(new Ticket
            {
                Id = tickets.Count == 0 ? 1 : tickets.Max(ticket => ticket.Id) + 1,
                UserName = _userName,
                Message = message.Trim(),
                CreatedAt = DateTime.Now
            });
            _ticketStore.SaveToJson();
            ConsoleMessages.Success("Din ticket har skickats till admin.");
            ConsoleMessages.WaitForKey();
        }

        private static Book book(string title, List<Book> books)
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<Book>()
                    .Title("[yellow]" + title + "[/]")
                    .AddChoices(books)
                    .UseConverter(b => Markup.Escape(b.Title + " (" + b.Author + ")")));
        }

        // Visar alla böcker och låter användaren välja en bok för att läsa mer
        private void BrowseBooks()
        {
            var books = _bookManager.GetAllBooks();
            if (books.Count == 0)
            {
                ConsoleMessages.Warning("Det finns inga böcker att visa.");
                ConsoleMessages.WaitForKey();
                return;
            }

            var bookChoices = books
                .Select((item, index) => new { Book = item, Choice = $"{item.Title} — {item.Author} ({index + 1})" })
                .ToDictionary(item => item.Choice, item => item.Book);
            bookChoices.Add("Tillbaka till menyn", null!);

            // Styr loopen så att användaren kan titta på flera böcker i rad
            // Visar också ett stapeldigram över antal böcker per genre
            bool browsing = true;
            while (browsing)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(new Rule("[aqua]Alla böcker[/]").LeftJustified());
                new StatisticsView().ShowBooksPerGenre(books);

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Välj en bok för att läsa mer:")
                        .AddChoices(bookChoices.Keys));

                if (choice == "Tillbaka till menyn")
                {
                    browsing = false;
                    continue;
                }
                // VIsar bokens detaljer och ser vad som användaren vill göra sedan
                var selectedBook = bookChoices[choice];
                var action = ShowBookDetails(selectedBook);
                if (action == BookDetailsAction.BackToMenu)
                    browsing = false;
            }
        }

        // Visar en bok med recensioner och låter användaren låna den eller gå tillbaka
        private BookDetailsAction ShowBookDetails(Book selectedBook)
        {
            bool isAvailable = !_loanManager.IsLoaned(selectedBook.ISBN);
            new BookDetailView().Show(selectedBook, _reviewManager.GetLatestReviewsForBook(selectedBook.ISBN));
            AnsiConsole.MarkupLine(isAvailable
                ? "[green]Boken är tillgänglig.[/]"
                : "[red]Boken är utlånad just nu.[/]");

            var actions = new List<BookDetailsAction>();
            if (isAvailable)
                actions.Add(BookDetailsAction.Loan);
            actions.Add(BookDetailsAction.BackToBooks);
            actions.Add(BookDetailsAction.BackToMenu);

            var action = AnsiConsole.Prompt(
                new SelectionPrompt<BookDetailsAction>()
                    .Title("Vad vill du göra?")
                    .AddChoices(actions)
                    .UseConverter(item => item switch
                    {
                        BookDetailsAction.Loan => "Låna boken",
                        BookDetailsAction.BackToBooks => "Tillbaka till alla böcker",
                        _ => "Tillbaka till menyn"
                    }));

            if (action == BookDetailsAction.Loan)
            {

                // Försöker låna boken och fångar regelfel
                try
                {
                    var loan = _loanManager.LoanBook(selectedBook, _userName);
                    ConsoleMessages.Success(
                        $"Du har lånat {selectedBook.Title} under namnet {loan.Borrower}. " +
                        $"Lånetid: {LoanManager.LoanDays} dagar. Förfallodatum: {loan.DueDate:yyyy-MM-dd}");
                    ConsoleMessages.WaitForKey();
                }
                catch (ArgumentException ex)
                {
                    ConsoleMessages.Error(ex.Message);
                    ConsoleMessages.WaitForKey();
                }
                return BookDetailsAction.BackToBooks;
            }

            return action;
        }

        // Hittar titeln utifrån ISBN, annars visas ISBN
        private string TitleFor(string isbn)
        {
            var book = _bookManager.GetAllBooks().FirstOrDefault(b => b.ISBN == isbn);
            return book == null ? isbn : book.Title;
        }
    }
}
