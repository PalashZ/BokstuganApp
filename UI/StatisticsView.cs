using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;
using BokstuganApp.Classes;

// Denna klassen används för att visa statistik om böcker i konsolen.
namespace BokstuganApp.UI
{
    public class StatisticsView
    {
        // Visar statistik om böcker i en tabell och ett diagram.
        public void ShowBooksPerGenre(List<Book> books)
        {
            if (books.Count == 0)
            {
                ConsoleMessages.Warning("Det finns inga böcker att räkna.");
                return;
            }
            // Skapar en tabell med kolumner för genre och antal böcker.
            var chart = new BarChart();
            chart.Width = 60;
            chart.Label = "[bold]Böcker per genre[/]";

            // Grupperar böckerna per genre och räknar hur många som finns i varje
            var groups = books.GroupBy(book => book.Genre);

            foreach (var group in groups)
            {
                chart.AddItem(group.Key, group.Count(), Color.Aqua);
            }

            AnsiConsole.Write(chart);


        }
    }
}

