namespace BokstuganApp.Services;
// Representerar en loggpost med tidpunkt, nivå och meddelande.
public class LogEntry
{
    public DateTime Timestamp { get; set; }

    public string Level { get; set; } = "";

    public string Message { get; set; } = "";
}