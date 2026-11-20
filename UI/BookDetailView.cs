using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;
using BokstuganApp.Classes;
using System.Linq;

// Denna klassen används för att visa detaljerad information om en bok i konsolen.
// Den använder Spectre.Console för att skapa en snygg panel med bokinformationen.
namespace BokstuganApp.UI
{
    public class BookDetailView
    {
        // Visar detaljerad information om en bok i en panel.
        public void Show(Book book, IEnumerable<Review> reviews)
        {

            // Skapar en sträng med bokinformationen som ska visas i panelen.
            string text =
                "[bold]" + Markup.Escape(book.Title) + "[/]\n" +
                "Författare: " + Markup.Escape(book.Author) + "\n" +
                "Genre: " + Markup.Escape(book.Genre) + "\n" +
                "År: " + book.Year + "\n" +
                "ISBN: " + Markup.Escape(book.ISBN);

            // Skapar en panel med bokinformationen och skriver ut den i konsolen.
            var panel = new Panel(text);
            panel.Header = new PanelHeader("Bokinformation");
            panel.Border = BoxBorder.Rounded;
            panel.Padding = new Padding(1, 1);
            AnsiConsole.Write(panel);

            var latestReviews = reviews.ToList();
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule("[aqua]De tre senaste recensionerna[/]").LeftJustified());

            if (latestReviews.Count == 0)
            {
                AnsiConsole.MarkupLine("[grey]Den här boken har inga recensioner ännu.[/]");
                return;
            }

            foreach (var review in latestReviews)
            {
                var reviewText = new Markup(
                    $"[yellow]{review.Rating}/5[/]  [bold]{Markup.Escape(review.Reviewer)}[/]  [grey]{review.Date:yyyy-MM-dd}[/]\n" +
                    Markup.Escape(review.Comment));
                AnsiConsole.Write(new Panel(reviewText)
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Grey)
                    .Padding(1, 0));
            }


        }
    }
}

