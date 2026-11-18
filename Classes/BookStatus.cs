using System;
using System.Collections.Generic;
using System.Text;
// Denna klassen representerar en bok i BokstuganApp.
namespace BokstuganApp.Classes
{
    public class BookStatus
    {
        // Beskriver om en bok kan lånas just nu.
        public enum Bookstatus
        {
            Tillgänglig,  // Tillgänglig: kan lånas
            Utlånad,   // Utlånad: finns hos en användare just nu
            Reserverad    // Reserverad: kan inte lånas av andra
        }
    }
}
