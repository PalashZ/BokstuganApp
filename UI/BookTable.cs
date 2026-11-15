using System;
using System.Collections.Generic;
using System.Text;
using BokstuganApp.Classes;
using Spectre.Console;

namespace BokstuganApp.UI
{
    // Denna klassen används för att visa en lista med böcker i en tabell i konsolen.
    // Den använder Spectre.Console för att skapa en snygg tabell med bokinformationen.
    public class BookTable
    {
        // Visar en lista med böcker i en tabell i konsolen.
        public void Show(IEnumerable<Book> books)
        {
            var list = books.ToList();

            if (list.Count == 0)
            {
                ConsoleMessages.Warning("Det finns inga böcker att visa.");
                return;
            }
            // Skapar en tabell med kolumner för titel, författare, genre och år.
            // Använder Markup.Escape för att undvika problem med specialtecken i bokinformationen.
            var table = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Grey)
                .Title("[bold]Böcker[/]");

            // Lägger till kolumnerna i tabellen med fetstil.
            table.AddColumn("[bold]Titel[/]");
            table.AddColumn("[bold]Författare[/]");
            table.AddColumn("[bold]Genre[/]");
            table.AddColumn("[bold]År[/]");

            // Lägger till varje bok som en rad i tabellen med bokinformationen.
            // Använder Markup.Escape för att undvika problem med specialtecken i bokinformationen.
            foreach (var book in list)
            {
                table.AddRow(
                    Markup.Escape(book.Title),
                    Markup.Escape(book.Author),
                    Markup.Escape(book.Genre),
                    book.Year.ToString());

            }

            AnsiConsole.Write(table);


        }
    }
}
