namespace BokstuganApp.Classes;
// Representerar en biblioteksanvändare och ger en personlig välkomsthälsning.
public class User : Person
{
    // Hälsar användaren välkommen med det registrerade namnet.
    public override string GetWelcomeMessage()
    {
        return $"Välkommen till Bokstugan, {Name}!";
    }
}