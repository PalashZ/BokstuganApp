using System;
using System.Collections.Generic;
using System.Text.Json;

namespace BokstuganApp.Services;
// Denna klassen representerar en generisk datalagring som kan hålla en lista av objekt av typen T och spara den som JSON.
// Den innehåller metoder för att lägga till, ta bort, hitta och hämta alla objekt, samt spara och läsa från en JSON-fil.
// Generisk lagring: håller en lista av T och sparar den som JSON
public class DataStore<T>
{
    private readonly string _filePath;
    private List<T> _items = new List<T>();

    public DataStore(string fileName)
    {
        string folder = Path.Combine(AppContext.BaseDirectory, "Data");
        Directory.CreateDirectory(folder);
        _filePath = Path.Combine(folder, fileName);
    }

    public void AddItem(T item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        _items.Add(item);
    }

    public bool RemoveItem(T item)
    {
        return _items.Remove(item);
    }

    public List<T> GetAll()
    {
        return new List<T>(_items);
    }

    // Sparar den egna listan. Ingen parameter behövs
    public void SaveToJson()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(_filePath, JsonSerializer.Serialize(_items, options));
    }

    // Ersätter lagringens lista innan den sparas.
    public void SaveToJson(List<T> items)
    {
        _items = new List<T>(items);
        SaveToJson();
    }

    // Läser in listan från filen. Saknas filen börjar vi med en tom lista
    public void LoadFromJson()
    {
        if (!File.Exists(_filePath))
        {
            return;
        }

        string json = File.ReadAllText(_filePath);
        _items = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
    }
}
