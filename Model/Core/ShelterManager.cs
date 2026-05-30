using Model.Data;

namespace Model.Core;

/// <summary>
/// Manager class for handling shelter data operations
/// </summary>
public class ShelterManager
{
    private readonly string _dataDirectory;
    private readonly JsonSerializer _jsonSerializer;
    private List<Shelter> _shelters;

    public ShelterManager()
    {
        _dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        _jsonSerializer = new JsonSerializer();
        _shelters = new List<Shelter>();

        if (!Directory.Exists(_dataDirectory))
        {
            Directory.CreateDirectory(_dataDirectory);
        }
    }

    public List<Shelter> Shelters => _shelters;

    /// <summary>
    /// Load shelters from JSON files or create default ones
    /// </summary>
    public void LoadOrCreateShelters()
    {
        _shelters.Clear();

        var jsonFiles = Directory.GetFiles(_dataDirectory, "*.json");

        if (jsonFiles.Length > 0)
        {
            foreach (var file in jsonFiles)
            {
                try
                {
                    var content = File.ReadAllText(file);
                    var shelter = _jsonSerializer.DeserializeShelter(content);
                    if (shelter != null)
                    {
                        _shelters.Add(shelter);
                    }
                }
                catch (Exception)
                {
                    // Skip corrupted files
                }
            }
        }

        if (_shelters.Count == 0)
        {
            CreateDefaultShelters();
        }
    }

    /// <summary>
    /// Create default shelters with sample pets
    /// </summary>
    private void CreateDefaultShelters()
    {
        _shelters = new List<Shelter>
        {
            CreateSunriseShelter(),
            CreateHappyPawsShelter(),
            CreateGreenValleyShelter()
        };

        SaveAllShelters();
    }

    /// <summary>
    /// Create the first shelter - Sunrise Shelter (with open area)
    /// </summary>
    private Shelter CreateSunriseShelter()
    {
        var shelter = new Shelter("Sunrise Shelter", 20, true);

        var pets = new List<Pet>
        {
            new Cat("Whiskers", 3, 4.5, "Orange", true, false),
            new Cat("Luna", 2, 3.8, "Gray", false),
            new Dog("Buddy", 5, 25.0, "Labrador", true),
            new Dog("Max", 4, 30.0, "German Shepherd", true, true),
            new Rabbit("Flopsy", 1, 2.5, 12.0, "Long"),
            new Cat("Shadow", 4, 5.0, "Black", true),
            new Dog("Bella", 2, 18.0, "Beagle", false)
        };

        foreach (var pet in pets)
        {
            shelter.AddPet(pet);
        }

        return shelter;
    }

    /// <summary>
    /// Create the second shelter - Happy Paws (without open area)
    /// </summary>
    private Shelter CreateHappyPawsShelter()
    {
        var shelter = new Shelter("Happy Paws Shelter", 15, false);

        var pets = new List<Pet>
        {
            new Dog("Rocky", 3, 28.0, "Bulldog", false),
            new Dog("Charlie", 6, 22.0, "Poodle", true),
            new Cat("Mittens", 1, 3.2, "White", false),
            new Cat("Garfield", 5, 6.5, "Orange", true),
            new Dog("Duke", 4, 35.0, "Rottweiler", true)
        };

        foreach (var pet in pets)
        {
            shelter.AddPet(pet);
        }

        return shelter;
    }

    /// <summary>
    /// Create the third shelter - Green Valley (with open area)
    /// </summary>
    private Shelter CreateGreenValleyShelter()
    {
        var shelter = new Shelter("Green Valley Shelter", 25, true);

        var pets = new List<Pet>
        {
            new Rabbit("Thumper", 2, 3.0, 15.0, "Short"),
            new Rabbit("Cottontail", 1, 2.8, 10.0, "Rex"),
            new Cat("Simba", 3, 4.8, "Golden", false),
            new Dog("Cooper", 2, 20.0, "Golden Retriever", true, true),
            new Cat("Chloe", 4, 4.2, "Siamese", true),
            new Rabbit("Snowball", 3, 3.5, 14.0, "Long"),
            new Dog("Daisy", 1, 15.0, "Border Collie", false),
            new Cat("Oliver", 2, 5.5, "Tabby", false, true)
        };

        foreach (var pet in pets)
        {
            shelter.AddPet(pet);
        }

        return shelter;
    }

    /// <summary>
    /// Save all shelters to JSON files
    /// </summary>
    public void SaveAllShelters()
    {
        foreach (var shelter in _shelters)
        {
            SaveShelter(shelter);
        }
    }

    /// <summary>
    /// Save a specific shelter to a JSON file
    /// </summary>
    public void SaveShelter(Shelter shelter)
    {
        var fileName = $"{SanitizeFileName(shelter.Name)}.json";
        var filePath = Path.Combine(_dataDirectory, fileName);
        _jsonSerializer.SaveToFile(shelter, filePath);
    }

    /// <summary>
    /// Get a shelter by name
    /// </summary>
    public Shelter? GetShelterByName(string name)
    {
        return _shelters.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Get all pets from all shelters matching filter criteria
    /// </summary>
    public List<Pet> GetFilteredPets(string? shelterName, Type? petType, bool openAreaOnly)
    {
        var result = new List<Pet>();

        var sheltersToSearch = _shelters;

        if (openAreaOnly)
        {
            sheltersToSearch = sheltersToSearch.Where(s => s.HasOpenArea).ToList();
        }

        if (!string.IsNullOrEmpty(shelterName))
        {
            sheltersToSearch = sheltersToSearch.Where(s => s.Name.Equals(shelterName, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        foreach (var shelter in sheltersToSearch)
        {
            IEnumerable<Pet> pets = shelter.Pets;

            if (petType != null)
            {
                pets = pets.Where(p => p.GetType() == petType);
            }

            result.AddRange(pets);
        }

        return result;
    }

    /// <summary>
    /// Find which shelter contains a specific pet
    /// </summary>
    public Shelter? FindShelterForPet(Pet pet)
    {
        return _shelters.FirstOrDefault(s => s.ContainsPet(pet));
    }

    private static string SanitizeFileName(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }
        return name;
    }
}