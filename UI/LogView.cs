using BokstuganApp.Services;

namespace BokstuganApp.UI;

public class LogView
{
    private readonly LogService _logService;

    public LogView(LogService logService)
    {
        _logService = logService;
    }

    public void Show()
    {
        var entries = _logService.GetRecent(10);

        Console.WriteLine("Senaste loggposterna");

        if (entries.Count == 0)
        {
            Console.WriteLine("Inga loggposter att visa.");
            return;
        }

        foreach (var entry in entries)
        {
            Console.WriteLine(
                $"{entry.Timestamp:yyyy-MM-dd HH:mm:ss} [{entry.Level}] {entry.Message}");
        }
    }
}