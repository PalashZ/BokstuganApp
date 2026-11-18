using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BokstuganApp.Services;
// Denna klassen representerar en generisk datalagring som kan hålla en lista av objekt av typen T och spara den som JSON.
// Den innehåller metoder för att lägga till, ta bort, hitta och hämta alla objekt, samt spara och läsa från en JSON-fil.
// Generisk lagring: håller en lista av T och sparar den som JSON
public class DataStore<T>
{
    private readonly string _filePath;
    private List<T> _items = new List<T>();
    // Konstruktor som tar emot ett filnamn och skapar en mapp för att lagra JSON-filen.
    public DataStore(string fileName)
    {
        string folder = Path.Combine(AppContext.BaseDirectory, "Data");
        Directory.CreateDirectory(folder);
        _filePath = Path.Combine(folder, fileName);
    }
    // Metod för att lägga till ett objekt i listan. Den kontrollerar att objektet inte är null.
    public void AddItem(T item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        _items.Add(item);
    }
    // Metod för att ta bort ett objekt från listan. Den returnerar true om objektet togs bort, annars false.
    public bool RemoveItem(T item)
    {
        return _items.Remove(item);
    }

    // Metod för att hitta ett objekt i listan baserat på en predikatfunktion.
    public T? FindItem(Func<T, bool> match)
    {
        return _items.FirstOrDefault(match);
    }

    // Metod för att hämta alla objekt i listan.
    public List<T> GetAll()
    {
        return new List<T>(_items);
    }
    // Metod för att spara listan som JSON i filen. Den hanterar eventuella undantag som kan uppstå vid filoperationer.
    public void SaveToJson()
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(_filePath, JsonSerializer.Serialize(_items, options));
        }
        catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
        {
            throw new InvalidOperationException("Filen kunde inte sparas: " + ex.Message);
        }
    }
    // Metod för att läsa listan från JSON-filen. Den hanterar eventuella undantag som kan uppstå vid filoperationer och deserialisering.
    public void LoadFromJson()
    {
        if (!File.Exists(_filePath))
        {
            return; // Första starten: ingen fil än är inget fel
        }

        try
        {
            string json = File.ReadAllText(_filePath);
            _items = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }
        catch (JsonException)
        {
            throw new InvalidOperationException("Filen är skadad och lästes inte in.");
        }
    }
}