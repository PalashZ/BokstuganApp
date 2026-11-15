using BokstuganApp.Services;
using Spectre.Console;

namespace BokstuganApp.UI;

public class LogView
// Denna klassen används för att visa loggposter i konsolen.
// Den använder LogService för att hämta de senaste loggposterna och Spectre.Console för att skapa en snygg tabell med logginformationen.
// Logservice är en tjänst som hanterar loggning i applikationen och lagrar loggposter i minnet.
{
    private readonly LogService _logService;

    public LogView(LogService logService)
    {
        _logService = logService;
    }
    // Visar de senaste loggposterna i en tabell i konsolen.
    // Om det inte finns några loggposter att visa, skrivs ett varningsmeddelande ut i konsolen.
    public void Show()
    {
        var entries = _logService.GetRecent(10);

        if (entries.Count == 0)
        {
            ConsoleMessages.Warning("Inga loggposter att visa.");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .Title("[bold]Senaste loggposterna[/]");

        table.AddColumn("Tid");
        table.AddColumn("Nivå");
        table.AddColumn("Meddelande");
        // Lägger till varje loggpost som en rad i tabellen med tid, nivå och meddelande.
        // Använder färger för att markera loggnivåerna: rött för fel, gult för varningar och grönt för information.
        foreach (var entry in entries)
        {
            string level = entry.Level.ToString();
            string color = level switch
            {
                "Error" => "red",
                "Warning" => "yellow",
                _ => "green"
            };

            table.AddRow(
                entry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                $"[{color}]{level}[/]",
                Markup.Escape(entry.Message));
        }

        AnsiConsole.Write(table);
    }
}