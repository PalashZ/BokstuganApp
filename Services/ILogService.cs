namespace BokstuganApp.Services;

public interface ILogService
{
    // Sparar en loggpost med nivå och meddelande.
    void Log(string level, string message);
}