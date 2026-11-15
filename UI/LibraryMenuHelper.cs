using BokstuganApp.UI;
using System;
using System.Collections.Generic;
using System.Text;
using BokstuganApp.Classes;
using Spectre.Console;
using System.Reflection.Metadata.Ecma335;

namespace BokstuganApp.UI
{

    // Denna klassen är en hjälparklass för LibraryMenu som innehåller metoder för att konvertera enum-värden till text och för att skapa demo-böcker.
    // Denna klassen används för att visa menyvalen i konsolen och för att skapa en lista med demo-böcker som kan användas för testning.
    public static class LibraryMenuHelper
    {
        // Konverterar ett MenuChoice enum-värde till en sträng som kan visas i konsolen.
        public static string ToText(MenuChoice choice)
            {
                switch (choice)
                {
                    case MenuChoice.MyBooks:
                        return "Visa mina böcker";
                    case MenuChoice.AllBooks:
                        return "Visa alla böcker";
                    case MenuChoice.ReturnBook:
                        return "Lämna in bok";
                    case MenuChoice.Search:
                        return "Sök efter bok";
                    case MenuChoice.Review:
                        return "Lämna recension";
                    default:
                        return "Logga ut";
                }


            }
        // Skapar en lista med demo-böcker som kan användas för testning.
        // Varje bok har titel, författare, genre, år och ISBN.
        public static List<Book> GetDemoBooks()
            {
                return new List<Book>
                {
                    new Book("Art of war", "Szu Tzu", "Facklitteratur", 1910, "90000001" ),
                    new Book("Omgivna av idioter", "Thomas Erikson", "Populärpsykologi", 2014, "90000002"),
                    new Book("The Hungry Game", "Suzanne Collins", "Dystopi", 2008, "90000003")
                };
            }
        
    }
}
