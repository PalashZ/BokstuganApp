namespace BokstuganApp.Classes;
// Basklass för användare och administratörer med gemensamma egenskaper för ID och namn.
public abstract class Person
{
    // Attributer
    public int Id { get; set; }

    public string Name { get; set; } = "";

    // Varje persontyp bestämmer sitt eget välkomstmeddelande.
    public abstract string GetWelcomeMessage();
}