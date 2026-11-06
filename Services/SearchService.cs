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

    // Filtrerar på genre utan skillnad på stora och små bokstäver.
    public List<Book> FilterByGenre(IEnumerable<Book> books, string? genre)
    {
        if (string.IsNullOrWhiteSpace(genre))
        {
            return books.ToList();
        }

        string selectedGenre = genre.Trim();

        return books
            .Where(book => string.Equals(
                book.Genre,
                selectedGenre,
                StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}