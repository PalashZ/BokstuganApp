using BokstuganApp.Classes;

namespace BokstuganApp.Services;

public class ReadingStatusManager
{
    private readonly DataStore<UserReadingStatus> _store;

    public ReadingStatusManager(DataStore<UserReadingStatus> store)
    {
        _store = store;
        _store.LoadFromJson();
    }

    public ReadingStatus GetStatus(string userName, string isbn)
    {
        var entry = FindStatus(userName, isbn);
        return entry?.Status ?? ReadingStatus.Oläst;
    }

    public void SetStatus(
        string userName, string isbn, ReadingStatus status)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new ArgumentException("Användarnamnet får inte vara tomt.");
        }

        if (string.IsNullOrWhiteSpace(isbn))
        {
            throw new ArgumentException("ISBN får inte vara tomt.");
        }

        if (!Enum.IsDefined(status))
        {
            throw new ArgumentException("Lässtatusen är ogiltig.");
        }

        var entries = _store.GetAll();
        var entry = FindStatus(userName, isbn);

        if (entry == null)
        {
            entries.Add(new UserReadingStatus
            {
                UserName = userName.Trim(),
                ISBN = isbn.Trim(),
                Status = status
            });
        }
        else
        {
            entry.Status = status;
        }

        _store.SaveToJson(entries);
    }

    private UserReadingStatus? FindStatus(string userName, string isbn)
    {
        return _store.GetAll().FirstOrDefault(entry =>
            string.Equals(entry.UserName, userName.Trim(),
                StringComparison.OrdinalIgnoreCase)
            && entry.ISBN == isbn.Trim());
    }
}