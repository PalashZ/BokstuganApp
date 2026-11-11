namespace BokstuganApp.Classes;

public abstract class Person
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    // Varje persontyp bestämmer sitt eget välkomstmeddelande.
    public abstract string GetWelcomeMessage();
}