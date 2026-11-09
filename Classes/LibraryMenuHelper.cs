using System;
using System.Collections.Generic;
using System.Text;

namespace BokstuganApp.Classes
{

    // Denna klassen är hur meny funkar för våran program.
    // Det visar vilka val användaren kan göra och tar input från användaren. Det visar menyn tills användaren väljer att avsluta.
    public class LibraryMenuHelper
    {

        // Denna metoden visar välkomstmeddelande till användaren.
        public void ShowWelcomeMessage()
        {
            Console.WriteLine("Välkommen till Bokstugan!");
            Console.WriteLine();
        }

        // Denna metoden visar menyval till användaren.
        public void ShowMenuOptions()
        {
            Console.WriteLine("1. Visa mina böcker");
            Console.WriteLine("2. Visa alla böcker");
            Console.WriteLine("3. Lämna in bok");
            Console.WriteLine("4. Sök efter en bok");
            Console.WriteLine("5. Lämna recensioner");
            Console.WriteLine("6. Avsluta");
            Console.WriteLine();
        }

        // Denna metoden tar input från användaren och returnerar det som en sträng.
        public string GetUserChoice()
        {
            Console.Write("Välj ett alternativ: ");
            return Console.ReadLine() ?? "";
        }

        // Denna metoden visar avslutningsmeddelande till användaren.
        public void ShowExitMessage()
        {
            Console.WriteLine();
            Console.WriteLine("Tack för besöket på Bokstugan!");
            Console.WriteLine("Ha ett bra fortsättning!");
        }
    }
}
