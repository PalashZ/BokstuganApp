using BokstuganApp.Classes;
using BokstuganApp.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace BokstuganApp.UI
// Denna klassen används för att visa startskärmen och laddningsskärmen i konsolen.
// Den använder Spectre.Console för att skapa en snygg startskärm med banner och progress bar.
// Deena klassen hjälper till med att hantera användarens val av roll (användare eller admin) och visar en spinner medan data sparas vid avslutning.
{
    public enum Role { User, Admin, Exit }
    public class StartScreen
    // Denna klassen används för att visa startskärmen och laddningsskärmen i konsolen.
    {
        private readonly DataStore<Book> _bookStore = new DataStore<Book>("books.json");
        private readonly BookManager _bookManager;

        public StartScreen()
        {
            _bookManager = new BookManager(_bookStore);
        }
        public void ShowBanner()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Bokstugan").Centered().Color(Color.Aqua));
            AnsiConsole.WriteLine();

        }

        public Role SelectRole()
        // Denna metoden visar en meny där användaren kan välja sin roll (användare, admin eller avsluta).
        {
            return AnsiConsole.Prompt(
               new SelectionPrompt<Role>()
                    .Title("[yellow]Vem är du?[/]")
                    .HighlightStyle(new Style(Color.Aqua))
                    .AddChoices(Role.User, Role.Admin, Role.Exit)
                    .UseConverter(role => role switch
                    {
                        Role.User => "Användare",
                        Role.Admin => "Admin",
                        _ => "Avsluta"

                    }));
        }

        public void LoadWithProgress()
        // Denna metoden visar en progress bar medan data laddas in i konsolen.
        {
            AnsiConsole.Progress().Start(ctx =>
            {
                var task = ctx.AddTask("[[grren]Läser in böcker[/]");
                while (!ctx.IsFinished)
                {
                    task.Increment(5);
                    Thread.Sleep(40);
                }
            });
        }

        public void SaveWithSpinner(Action save)
        // Denna metoden visar en spinner medan data sparas i konsolen.
        {
            AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .Start("Sparar...", _ => save());
        }

        public class LoadingScreen
        {
            // Visas när programmet startar: banner och en progress bar med flera steg
            // Visas när programmet avslutas: spinner medan data sparas, sedan ett tack
            // Denna klassen används för att visa en laddningsskärm vid start och en stängningsskärm vid avslutning av programmet.
            public void ShowStartup()
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(new FigletText("Bokstugan").Centered().Color(Color.Aqua));
                AnsiConsole.Write(new Rule("[grey]Startar...[/]"));
                AnsiConsole.WriteLine();

                string[] steps =
                {
                  "Läser in böcker",
                  "Läser in användare",
                  "Läser in lån",
                  "Förbereder menyn"
                };

                AnsiConsole.Progress()
                    .AutoClear(false)
                    .Columns(
                     new TaskDescriptionColumn(),
                     new ProgressBarColumn(),
                     new PercentageColumn(),
                     new SpinnerColumn())
                    .Start(ctx =>
                    {
                        foreach (string step in steps)
                        {
                            var task = ctx.AddTask("[green]" + step + "[/]");

                            while (!task.IsFinished)
                            {
                                task.Increment(10);
                                Thread.Sleep(60);
                            }
                        }
                    });

                Thread.Sleep(400);
            }

            // Visas när programmet avslutas: spinner medan data sparas, sedan ett tack
            public void ShowShutdown()
            {
                AnsiConsole.Clear();

                AnsiConsole.Status()
                    .Spinner(Spinner.Known.Dots)
                    .SpinnerStyle(Style.Parse("aqua"))
                    .Start("Sparar data...", ctx =>
                    {
                        Thread.Sleep(800);
                        ctx.Status("Stänger ner...");
                        Thread.Sleep(600);
                    });

                AnsiConsole.Write(new FigletText("Tack!").Centered().Color(Color.Aqua));
                AnsiConsole.Write(new Rule("[grey]Tack för besöket på Bokstugan[/]"));
                AnsiConsole.WriteLine();

                Thread.Sleep(1200);


            }
        }

    }
}
