using System;
using BokstuganApp.Services;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using BokstuganApp.Classes;
using Spectre.Console;
using BokstuganApp.Services;

// Denna klassen är hur meny funkar för våran program.
// Det visar vilka val användaren kan göra och tar input från användaren. Det visar menyn tills användaren väljer att avsluta.
namespace BokstuganApp.UI
{
    // Enum som representerar de olika valen i menyn.
    // Varje värde motsvarar en funktion som användaren kan välja.
    public enum MenuChoice { MyBooks, AllBooks, ReturnBook, Search, Review, LogOut}

    // Denna klassen är hur meny funkar för våran program.
    public class LibraryMenu
    {
        private readonly BookManager _bookManager;

        public LibraryMenu(BookManager bookManager)
        {
            _bookManager = bookManager;
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
                        ConsoleMessages.Info("Visa när mina böcker kommer");
                        ConsoleMessages.WaitForKey();
                        break;

                    case MenuChoice.AllBooks:
                        new BookTable().Show(_bookManager.GetAllBooks());
                        ConsoleMessages.WaitForKey();
                        break;

                    case MenuChoice.Search:
                    {
                        Console.Write("Sök på titel eller författare (Enter visar alla): ");
                        string? searchText = Console.ReadLine();

                        var results = new SearchService().SearchByText(
                            LibraryMenuHelper.GetDemoBooks(), searchText);

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

                    case MenuChoice.Review:
                        ConsoleMessages.Info("Den här funktionen byggs snart!.");
                        ConsoleMessages.WaitForKey();
                        break;


                    case MenuChoice.LogOut:
                        running = false;
                        break;

                    


                   
                   
                }
            }
        }
    }
}
