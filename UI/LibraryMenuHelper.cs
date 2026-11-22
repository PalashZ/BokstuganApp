using BokstuganApp.Classes;

namespace BokstuganApp.UI;

// Hjälpmetoder för menytexter och projektets demo-böcker.
public static class LibraryMenuHelper
{
    // Konverterar ett menyval till text som visas i konsolen.
    public static string ToText(MenuChoice choice)
    {
        return choice switch
        {
            MenuChoice.MyBooks => "Visa mina böcker",
            MenuChoice.AllBooks => "Visa alla böcker",
            MenuChoice.RandomBook => "Slumpmässig bok",
            MenuChoice.ReturnBook => "Lämna in bok",
            MenuChoice.Search => "Sök efter bok",
            MenuChoice.Review => "Lämna recension",
            MenuChoice.Ticket => "Ticket",
            MenuChoice.Reading => "Lässtatus",
            _ => "Logga ut"
        };
    }

    // Skapar böckerna som visas i biblioteket vid första starten.
    public static List<Book> GetDemoBooks()
    {
        return new List<Book>
        {
            new Book("Art of war", "Szu Tzu", "Facklitteratur", 1910, "90000001") { Id = 90000001 },
            new Book("Omgivna av idioter", "Thomas Erikson", "Populärpsykologi", 2014, "90000002") { Id = 90000002 },
            new Book("The Hunger Game", "Suzanne Collins", "Dystopi", 2008, "90000003") { Id = 90000003 },
            new Book("När ingen lyssnar", "Diamant Salihu", "Facklitteratur", 2023, "90000004") { Id = 90000004 },
            new Book("Lilla Chicago", "Ali Khalil", "Självbiograf", 2021, "90000005") { Id = 90000005 },
            new Book("Den sista sommaren", "Karl Enn", "Facklitteratur", 2024, "90000006") { Id = 90000006 },
            new Book("Svensk gangsterrap", "Emil Arvidson", "Facklitteratur", 2023, "90000007") { Id = 90000007 },
            new Book("En plats i paradiset", "Hama Dostan", "Skönlitteratur", 2003, "90000008") { Id = 90000008 },
            new Book("Can't hurt me", "David Goggins", "Självbiograf", 2018, "90000009") { Id = 90000009 },
            new Book("Outwitting the devil", " Napoleon Hill", "Självbiograf", 2018, "90000010") { Id = 90000010 },
            new Book("The 50th law", "50 Cent", "Facklitteratur", 2010, "9000011") { Id = 90000011 },
            new Book("Tills alla dör", "Diamant Salihu", "Facklitteratur", 2021, "9000012") { Id = 90000012 },
            new Book("The great gatsby", "F.Scott Fitzgerald", "Skönlitteratur", 1925, "9000013") { Id = 90000013 }
        };
    }
}
