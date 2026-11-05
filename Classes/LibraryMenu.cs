using System;
using System.Collections.Generic;
using System.Text;

// Denna klassen är för själva meny, det som står när du startar applicationen
namespace BokstuganApp.Classes
{
    public class LibraryMenu
    {

        // Vilka alternativer som finns
        public void ShowMenu()
        {
            Console.WriteLine("Välkommen till Bokstugan!");
            Console.WriteLine();

            Console.WriteLine("1. Visa mina böcker");
            Console.WriteLine("2. Visa alla böcker");
            Console.WriteLine("3. Lämna in bok");
            Console.WriteLine("4. Sök efter en bok");
            Console.WriteLine("5. Lämna recensioner");
            Console.WriteLine("6. Avsluta");
            Console.WriteLine();

            Console.Write("Välj ett alternativ:");
            Console.ReadLine();
           
             
        }
    }
}
