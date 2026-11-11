using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BokstuganApp.Classes
{
    internal class Loan
    {
        // Ett lån: vem som lånat vilken bok, och när den ska tillbaka.
        public class Loan_Books
        {
            public int Id { get; set; }
            public int BookId { get; set; }              // Vilken bok (vi sparar bara id:t)
            public string UserName { get; set; } = "";   // Vem som lånat
            public DateTime LoanDate { get; set; }       // När boken lånades
            public DateTime DueDate { get; set; }        // När den senast ska lämnas tillbaka
            public DateTime? ReturnDate { get; set; }    // null = inte återlämnad än
            public int ExtensionCount { get; set; }      // Hur många gånger lånet förlängts

            // Räknas ut automatiskt, sparas inte i JSON
            [JsonIgnore]
            public bool IsReturned => ReturnDate.HasValue;

            [JsonIgnore]
            public bool IsOverdue => !IsReturned && DateTime.Now.Date > DueDate.Date;
        }
    }
}
