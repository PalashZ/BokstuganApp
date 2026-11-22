using BokstuganApp.UI;

namespace BokstuganApp;

// Startar bibliotekets huvudmeny.
internal class Program
{
    private static void Main(string[] args)
    {
        var menu = new LibraryMenyShow();
        menu.Run();
    }
}
