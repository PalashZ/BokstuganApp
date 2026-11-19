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

        // Denna klassen använder sig av readonly fält för att skapa instanser av StartScreen och LoadingScreen som används för att visa start- och stängningsskärmarna.
        private readonly StartScreen _startScreen = new StartScreen();
        private readonly LoadingScreen _loadingScreen = new LoadingScreen();
        private readonly DataStore<Book> _bookStore = new DataStore<Book>("books.json");
        private readonly BookManager _bookManager;
        private readonly LoanManager _loanManager;
        private readonly ReviewManager _reviewManager;

        public LibraryMenyShow()
        {
            _bookManager = new BookManager(_bookStore);
            _loanManager = new LoanManager();
            _reviewManager = new ReviewManager(_loanManager);
        }


        public void Run()
        // Denna metoden kör huvudmenyn för BokstuganApp. Den visar startskärmen, laddningsskärmen och hanterar användarens val av roll (användare eller admin).
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            _loadingScreen.ShowStartup();

            bool running = true;
            // Använder en while-loop för att visa startskärmen och hantera användarens val tills användaren väljer att avsluta.
            while (running)
            {
                _startScreen.ShowBanner();

                switch(_startScreen.SelectRole())
                {
                    case Role.User:
                        {
                            string userName = InputHelper.AskText("Vad heter du?");
                            new LibraryMenu(_bookManager, _loanManager, _reviewManager, userName).ShowMenu();
                            break;
                        }

                    case Role.Admin:
                        new AdminMenu(_bookManager).ShowMenu();
                        break;

                    case Role.Exit:
                        running = false;
                        break;
                }
            }
            _loadingScreen.ShowShutdown();


            
        }
    }
}
