using BokstuganApp.Classes;
using BokstuganApp.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;
using static BokstuganApp.UI.StartScreen;

namespace BokstuganApp.UI
{
    // Denna klassen är huvudmenyn för BokstuganApp. Den hanterar startskärmen, laddningsskärmen och användarens val av roll (användare eller admin).
    // Denna klassen använder StartScreen och LoadingScreen för att visa start- och stängningsskärmarna.
    public class LibraryMenyShow
    {
        // Lagring: en låda per datatyp
        private readonly DataStore<Book> _bookStore = new DataStore<Book>("books.json");
        private readonly DataStore<Loan> _loanStore = new DataStore<Loan>("loans.json");
        private readonly DataStore<Review> _reviewStore = new DataStore<Review>("reviews.json");
        private readonly DataStore<User> _userStore = new DataStore<User>("users.json");
        private readonly DataStore<Ticket> _ticketStore = new DataStore<Ticket>("tickets.json");

        // Managers: skapas efter lagringen, eftersom de behöver den
        private BookManager _bookManager = null!;
        private LoanManager _loanManager = null!;
        private ReviewManager _reviewManager = null!;

        private readonly StartScreen _startScreen = new StartScreen();
        private readonly LoadingScreen _loadingScreen = new LoadingScreen();

        public void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            _loadingScreen.ShowStartup();

            bool dataLoaded = false;
            try
            {
                // Managers read the saved books, loans and reviews when they are created.
                _bookManager = new BookManager(_bookStore);
                _loanManager = new LoanManager(_loanStore);
                _reviewManager = new ReviewManager(_reviewStore, _loanManager);
                _userStore.LoadFromJson();
                _ticketStore.LoadFromJson();

                // Ta med användarnamn från tidigare lånehistorik.
                foreach (var borrower in _loanStore.GetAll()
                    .Select(loan => loan.Borrower)
                    .Where(name => !string.IsNullOrWhiteSpace(name)))
                {
                    RegisterUser(borrower);
                }

                dataLoaded = true;

                bool running = true;

                // Visar rollvalet om och om igen tills programmet avslutas
                while (running)
                {
                    _startScreen.ShowBanner();

                    // Startar rätt meny beroende på vilken roll som valdes

                    switch (_startScreen.SelectRole())
                    {
                        case Role.User:
                        {
                            string userName = InputHelper.AskUserName("Vad heter du?");
                            RegisterUser(userName);
                            new LibraryMenu(_bookManager, _loanManager, _reviewManager, userName, _ticketStore).ShowMenu();
                            break;
                        }

                        case Role.Admin:
                            if (AuthenticateAdmin())
                                new AdminMenu(_bookManager, _loanManager, _userStore, _ticketStore).ShowMenu();
                            break;

                        case Role.Exit:
                            running = false;
                            break;
                    }
                }
            }

            // Fångar oväntas fel och visar dem i rött så att programmet inte kraschar
            catch (Exception ex)
            {
                ConsoleMessages.Error(ex.Message);
                ConsoleMessages.WaitForKey();
            }
            finally
            {
                _loadingScreen.ShowShutdown(() =>
                {
                    if (!dataLoaded) return;

                    // Här visar alla lagringar son sparas till JSON
                    try
                    {
                        _bookStore.SaveToJson();
                        _loanStore.SaveToJson();
                        _reviewStore.SaveToJson();
                        _userStore.SaveToJson();
                        _ticketStore.SaveToJson();
                    }
                    catch (Exception ex)
                    {
                        ConsoleMessages.Error("Kunde inte spara all data: " + ex.Message);
                    }
                });
            }

        }

        // Registrerar en ny användare om namnet inte redan finns
        private void RegisterUser(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || _userStore.GetAll()
                .Any(user => string.Equals(user.Name, name.Trim(), StringComparison.OrdinalIgnoreCase)))
                return;

            var users = _userStore.GetAll();
            _userStore.AddItem(new User { Id = users.Count == 0 ? 1 : users.Max(user => user.Id) + 1, Name = name.Trim() });
        }

        // Frågar efter adminlösenordet. Ger true om der stämmer
        private static bool AuthenticateAdmin()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[aqua]Admininloggning[/]").LeftJustified());
            string password = AnsiConsole.Prompt(
                new TextPrompt<string>("Lösenord:").Secret());

            if (password == "nbi")
                return true;

            ConsoleMessages.Error("Fel lösenord.");
            ConsoleMessages.WaitForKey();
            return false;
        }
    }
}
