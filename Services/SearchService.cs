using BokstuganApp.Classes;

namespace BokstuganApp.Services;

public class SearchService
{
    // Söker i titel och författare utan skillnad på stora och små bokstäver.
    public List<Book> SearchByText(IEnumerable<Book> books, string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return books.ToList();
        }

        string searchText = text.Trim();

        return books
            .Where(book =>
                book.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                || book.Author.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}