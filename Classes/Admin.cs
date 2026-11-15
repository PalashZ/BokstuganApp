namespace BokstuganApp.Classes;
// Representerar en administratör med en administratörskod och en egen välkomsthälsning.
public class Admin : Person
{
    public string AdminCode { get; set; } = "";

    public override string GetWelcomeMessage()
    {
        return $"Välkommen administratör, {Name}!";
    }
}