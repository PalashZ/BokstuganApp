using System;
using System.Collections.Generic;
using System.Text;

// Denna klassen är hur meny funkar för våran program.
// Denna klassen använder LibraryMenuHelper för att visa meny och ta input från användaren.
namespace BokstuganApp.Classes
{

    // Denna klassen är hur meny funkar för våran program.
    public class LibraryMenu
    {
        private LibraryMenuHelper helper;

        // Konstruktor för LibraryMenu klassen.
        public LibraryMenu()
        {
            helper = new LibraryMenuHelper();
        }

        // Denna metoden visar meny och tar input från användaren.

        public void ShowMenu()
        {
            bool running = true;

            helper.ShowWelcomeMessage();

            // Loop som körs tills användaren väljer att avsluta.
            // Denna loop visar menyval och tar input från användaren.
            while (running)
            {
                helper.ShowMenuOptions();

                string choice = helper.GetUserChoice();

                switch(choice)
                {
                    case "1":
                        Console.WriteLine("Visa mina böcker");
                        break;

                    case "2":
                        Console.WriteLine("Visa alla böcker");
                        break;

                    case "3":
                        Console.WriteLine("Lämna in bok");
                        break;

                    case "4":
                        Console.WriteLine("Sök efter bok");
                        break;

                    case "5":
                        Console.WriteLine("Lämna recensioner");
                        break;

                    case "6":
                        helper.ShowExitMessage();
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Ogiltigt val. Vänligen försök igen!");
                        break;
                }
            }
        }
    }
}
