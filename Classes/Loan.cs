using System;

namespace BokstuganApp.Classes
{
    // En lånehändelse: vem har lånat vilken bok, och när den ska tillbaka
    public class Loan
    {
        public string Isbn { get; set; } = "";
        public string Borrower { get; set; } = "";
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        // Sant så länge boken inte är återlämnad
        public bool IsActive
        {
            get { return ReturnDate == null; }
        }

        // Sant om lånet är aktivt och förfallodatumet har passerat
        public bool IsOverdue
        {
            get { return IsActive && DateTime.Now.Date > DueDate.Date; }
        }
    }
}
