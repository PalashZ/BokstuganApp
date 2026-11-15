using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;
using BokstuganApp.Classes;

// Denna klassen används för att visa detaljerad information om en bok i konsolen.
// Den använder Spectre.Console för att skapa en snygg panel med bokinformationen.
namespace BokstuganApp.UI
{
    public class BookDetailView
    {
        // Visar detaljerad information om en bok i en panel.
        public void Show(Book book)
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
        }
    }
}

