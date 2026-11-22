namespace BokstuganApp.Classes;
// Denna klassen för för att kunna skapa tickets, att användaren kan skriva önskade böcker eller frågor
public class Ticket
{
    // Attributer för klassen Ticket
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
