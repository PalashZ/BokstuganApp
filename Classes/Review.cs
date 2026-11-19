using System;

namespace BokstuganApp.Classes
{
    // En recension: betyg och kommentar på en bok.
    public class Review
    {
        public string ISBN { get; set; } = "";
        public string Reviewer { get; set; } = "";
        public int Rating { get; set; }
        public string Comment { get; set; } = "";
        public DateTime Date { get; set; }
    }
}