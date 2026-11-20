
using System;
using System.Collections.Generic;
using System.Linq;
using BokstuganApp.Classes;

namespace BokstuganApp.Services
{
    // Hanterar lån: låna ut, lämna in och söka bland lån
    public class LoanManager
    {
        // Attributer 
        public const int LoanDays = 14;
        public const int MaxLoansPerUser = 3;

        private readonly DataStore<Loan> _loanStore;
        private readonly List<Loan> _loans;

        public LoanManager(DataStore<Loan> loanStore)
        {
            _loanStore = loanStore;
            _loanStore.LoadFromJson();
            _loans = _loanStore.GetAll();
        }

        // Lånar ut en bok om den är ledig och användaren inte har för många lån
        public Loan LoanBook(Book book, string borrower)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            if (string.IsNullOrWhiteSpace(borrower))
            {
                throw new ArgumentException("Namnet får inte vara tomt.");
            }

            if (IsLoaned(book.ISBN))
            {
                throw new ArgumentException("Boken är redan utlånad.");
            }

            if (GetActiveLoansForUser(borrower).Count >= MaxLoansPerUser)
            {
                throw new ArgumentException(
                    "Du kan låna högst " + MaxLoansPerUser + " böcker samtidigt.");
            }

            var loan = new Loan
            {
                Isbn = book.ISBN,
                Borrower = borrower,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(LoanDays)
            };

            _loans.Add(loan);
            _loanStore.SaveToJson(_loans);

            return loan;
        }

        // Stänger ett lån. Ger fel om användaren inte har lånat boken
        public Loan ReturnBook(string isbn, string borrower)
        {
            var loan = _loans.FirstOrDefault(l =>
                l.IsActive &&
                l.Isbn == isbn &&
                l.Borrower.Equals(borrower, StringComparison.OrdinalIgnoreCase));

            if (loan == null)
            {
                throw new ArgumentException("Du har inte lånat den här boken.");
            }

            loan.ReturnDate = DateTime.Now;
            _loanStore.SaveToJson(_loans);

            return loan;
        }

        // Sant om någon har boken just nu
        public bool IsLoaned(string isbn)
        {
            return _loans.Any(l => l.IsActive && l.Isbn == isbn);
        }

        // Pågående lån för en viss bok, eller null om boken är ledig
        public Loan? GetActiveLoansForBook(string isbn)
        {
            return _loans.FirstOrDefault(l => l.IsActive && l.Isbn == isbn);
        }

        // Användarens pågående lån
        public List<Loan> GetActiveLoansForUser(string borrower)
        {
            return _loans
                .Where(l => l.IsActive &&
                            l.Borrower.Equals(borrower, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // Sant om användaren någon gång har lånat boken (även om den lämnats in)
        public void RemoveLoansForUser(string borrower)
        {
            _loans.RemoveAll(loan => loan.Borrower.Equals(borrower, StringComparison.OrdinalIgnoreCase));
            _loanStore.SaveToJson(_loans);
        }

        public bool HasBorrowed(string isbn, string borrower)
        {
            return _loans.Any(l =>
                l.Isbn == isbn &&
                l.Borrower.Equals(borrower, StringComparison.OrdinalIgnoreCase));
        }
    }
}
