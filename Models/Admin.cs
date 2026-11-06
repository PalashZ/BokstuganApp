namespace BokstuganApp.Models;

public class Admin : Person
{
    public string AdminCode { get; set; } = "";

    public override string GetWelcomeMessage()
    {
        return $"Välkommen administratör, {Name}!";
    }
}