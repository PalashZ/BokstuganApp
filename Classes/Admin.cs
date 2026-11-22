namespace BokstuganApp.Classes;
// Representerar en administratör med en administratörskod och en egen välkomsthälsning.
public class Admin : Person
{
    // Här kan vi se attributerna
    public string AdminCode { get; set; } = "";


    //  Detta är våran metod för att "adminusers" ska få välkomen medelande 
    public override string GetWelcomeMessage()
    {
        return $"Välkommen administratör, {Name}!";
    }
}