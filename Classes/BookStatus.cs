using System;
using System.Collections.Generic;
using System.Text;

namespace BokstuganApp.Classes
{
    internal class BookStatus
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
