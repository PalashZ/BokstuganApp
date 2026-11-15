using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace BokstuganApp.UI
{
    public static class ConsoleMessages

    // Denna klassen används för att skriva ut meddelanden i konsolen med olika färger och format.
    // Varje metod tar en sträng som parameter och skriver ut den i konsolen med hjälp av Spectre.Console och har olika färger.
    {
        public static void Success(string text)
        {
            AnsiConsole.MarkupLine($"[green]{Markup.Escape(text)}[/]");
        }

        public static void Error(string text)
        {
            AnsiConsole.MarkupLine($"[red]{Markup.Escape(text)}[/]");
        }

        public static void Warning(string text)
        {
            AnsiConsole.MarkupLine($"[yellow]{Markup.Escape(text)}[/]");
        }

        public static void Info(string text)
        {
            AnsiConsole.MarkupLine($"[aqua]{Markup.Escape(text)}[/]");
        }

        public static void Hint(string text)
        {
            AnsiConsole.MarkupLine($"[grey]{Markup.Escape(text)}[/]");
        }
        

        public static void WaitForKey()
        {
            AnsiConsole.WriteLine();
            Hint("Tryck på valfri knapp för att fortsätta...");
            Console.ReadKey(true);


        }
    }
}
