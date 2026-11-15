namespace BokstuganApp.Services;
// Definierar hur andra delar av programmet kan spara en loggpost.
public interface ILogService
{
    // Sparar en loggpost med nivå och meddelande.
    void Log(string level, string message);
}