using BokstuganApp.UI;
using Spectre.Console;
using BokstuganApp.Classes;
// Denna klassen är huvudprogrammet för BokstuganApp. Den startar huvudmenyn för BokstuganApp.
// I Program.cs försöker vi hålla det enkelt och fokuserat på att starta huvudmenyn. Vi skapar en instans av LibraryMenyShow och kör dess Run-metod.
namespace BokstuganApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var menu = new LibraryMenyShow();
            menu.Run();
        }
    }
}
