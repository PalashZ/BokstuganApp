using System;
using System.Collections.Generic;
using System.Text;
using BokstuganApp.Classes;
using System.Linq;

namespace BokstuganApp.Services
{
    // Hanterar recensioner. Regeln: man kan bara recensera böcker man lånat
    public class ReviewManager
    {
        private readonly List<Review> _reviews = new List<Review>();
        private readonly LoanManager _loanManager;

        public ReviewManager(LoanManager loanManager)
        {
            _loanManager = loanManager;
        }

        

        // Lägger till en recension efter att reglerna kontrollerats
        public void AddReview(string isbn, string reviewer, int rating, string comment)
        {
            if (rating < 1 || rating > 5)
            {
                throw new ArgumentException("Betyget måste vara mellan 1 och 5.");
            }

            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new ArgumentException("Kommentaren får inte vara tom.");
            }

            if (!_loanManager.HasBorrowed(isbn, reviewer))
            {
                throw new ArgumentException("Du kan bara recensera böcker du har lånat.");
            }

            bool alreadyReviewed = _reviews.Any(r =>
                r.ISBN == isbn &&
                r.Reviewer.Equals(reviewer, StringComparison.OrdinalIgnoreCase));

            if (alreadyReviewed)
            {
                throw new ArgumentException("Du har redan recenserat den här boken.");
            }

            _reviews.Add(new Review
            {
                ISBN = isbn,
                Reviewer = reviewer,
                Rating = rating,
                Comment = comment,
                Date = DateTime.Now
            });
        }

        // Alla recensioner för en bok
        public List<Review> GetReviewsForBook(string isbn)
        {
            return _reviews.Where(r => r.ISBN == isbn).ToList();
        }

        // Snittbetyg för en bok, 0 om det saknas recensioner
        public double GetAverageRating(string isbn)
        {
            var reviews = GetReviewsForBook(isbn);
            return reviews.Count == 0 ? 0 : reviews.Average(r => r.Rating);
        }
    }
}
