using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Model.Core;

/// <summary>
/// Shelter class that manages animals in a shelter
/// Implements ICountable for counting animals and IFilter for filtering
/// </summary>
public partial class Shelter : ICountable, IFilter
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("capacity")]
    public int Capacity { get; set; }

    [JsonProperty("hasOpenArea")]
    public bool HasOpenArea { get; set; }

    [JsonProperty("pets")]
    public List<Pet> Pets { get; set; }

    public Shelter()
    {
        Name = string.Empty;
        Capacity = 0;
        HasOpenArea = false;
        Pets = new List<Pet>();
    }

    public Shelter(string name, int capacity, bool hasOpenArea)
    {
        Name = name;
        Capacity = capacity;
        HasOpenArea = hasOpenArea;
        Pets = new List<Pet>();
    }

    /// <summary>
    /// Override ToString for shelter information
    /// </summary>
    public override string ToString()
    {
        return $"{Name} (Capacity: {Capacity}, Open Area: {HasOpenArea}, Pets: {Count()})";
    }

    /// <summary>
    /// Count total number of animals
    /// </summary>
    public int Count()
    {
        return Pets.Count;
    }

    /// <summary>
    /// Count animals of a specific type (overloaded method)
    /// </summary>
    public int Count(Type type)
    {
        return Pets.Count(p => p.GetType() == type);
    }

    /// <summary>
    /// Overloaded Count method with predicate
    /// </summary>
    public int Count(Predicate<Pet> predicate)
    {
        return Pets.Count(p => predicate(p));
    }

    /// <summary>
    /// Get percentage of animals of a specific type
    /// </summary>
    public int Percentage(Type type)
    {
        if (Pets.Count == 0) return 0;
        return (int)Math.Round((double)Count(type) / Pets.Count * 100);
    }

    /// <summary>
    /// Filter animals by type
    /// </summary>
    public virtual List<Pet> Filter(Type type)
    {
        return Pets.Where(p => p.GetType() == type).ToList();
    }

    /// <summary>
    /// Overloaded filter method for claustrophobic animals
    /// </summary>
    public virtual List<Pet> Filter(Type type, bool claustrophobicOnly)
    {
        if (claustrophobicOnly)
        {
            return Pets.Where(p => p.GetType() == type && p.HasClaustrophobia).ToList();
        }
        return Filter(type);
    }

    /// <summary>
    /// Get all pets matching a type name string
    /// </summary>
    public List<Pet> Filter(string typeName)
    {
        return Pets.Where(p => p.GetType().Name.Equals(typeName, StringComparison.OrdinalIgnoreCase)).ToList();
    }
}