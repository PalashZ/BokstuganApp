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
            var colors = new[]
            {
                Color.Aqua, Color.Green, Color.Yellow, Color.Fuchsia,
                Color.Blue, Color.Red, Color.Lime, Color.Teal
            };

            var chart = new BarChart();
            chart.Width = 60;
            chart.Label = "[bold]Böcker per genre[/]";

            var groups = books
                .GroupBy(book => string.IsNullOrWhiteSpace(book.Genre) ? "Okänd genre" : book.Genre.Trim())
                .OrderBy(group => group.Key)
                .ToList();

            for (int index = 0; index < groups.Count; index++)
            {
                var group = groups[index];
                chart.AddItem(Markup.Escape(group.Key), group.Count(), colors[index % colors.Length]);
            }

            AnsiConsole.Write(chart);


        }
    }
}

