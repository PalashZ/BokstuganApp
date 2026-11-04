using System.Text.Json;

namespace BokstuganApp.Services;

public class LogService
{
    public void Log(string level, string message)
    {
        string filePath = "log.json";

        List<LogEntry> entries = new List<LogEntry>();

        // Läs tidigare loggposter om filen finns.
        if (File.Exists(filePath))
        {
            string existingJson = File.ReadAllText(filePath);

            entries = JsonSerializer.Deserialize<List<LogEntry>>(existingJson)
                ?? throw new JsonException("Loggfilen innehåller ingen lista.");
        }

        // Skapa den nya loggposten.
        LogEntry entry = new LogEntry
        {
            Timestamp = DateTime.Now,
            Level = level,
            Message = message
        };

        entries.Add(entry);

        // Omvandla hela listan till JSON och spara den.
        string json = JsonSerializer.Serialize(
            entries,
            new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(filePath, json);
    }
}