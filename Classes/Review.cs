using System;

namespace BokstuganApp.Classes
{
    // En recension: betyg och kommentar på en bok.
    public class Review
    {
        public int Id { get; set; }
        public int BookId { get; set; }              // Vilken bok recensionen gäller (vi sparar bara id:t)
        public string UserName { get; set; } = "";   // Vem som skrev recensionen
        public int Rating { get; set; }              // Betyg 1-5
        public string Comment { get; set; } = "";    // Kommentar (får vara tom)
        public DateTime CreatedDate { get; set; }    // När recensionen skrevs
    }
}