namespace Model.Data;

/// <summary>
/// Abstract base class for serialization operations
/// </summary>
public abstract class Serializer
{
    /// <summary>
    /// Abstract method for serializing an object to string
    /// </summary>
    public abstract string Serialize<T>(T obj);

    /// <summary>
    /// Abstract method for deserializing a string to an object
    /// </summary>
    public abstract T? Deserialize<T>(string data);

    /// <summary>
    /// Abstract method for serializing a list of objects
    /// </summary>
    public abstract string SerializeList<T>(List<T> list);

    /// <summary>
    /// Abstract method for deserializing a list of objects
    /// </summary>
    public abstract List<T>? DeserializeList<T>(string data);

    /// <summary>
    /// Save data to a file
    /// </summary>
    public void SaveToFile<T>(T obj, string filePath)
    {
        string content = Serialize(obj);
        File.WriteAllText(filePath, content);
    }

    /// <summary>
    /// Load data from a file
    /// </summary>
    public T? LoadFromFile<T>(string filePath)
    {
        if (!File.Exists(filePath))
            return default;

        string content = File.ReadAllText(filePath);
        return Deserialize<T>(content);
    }

    /// <summary>
    /// Generic method for saving a list to file
    /// </summary>
    public void SaveListToFile<T>(List<T> list, string filePath)
    {
        string content = SerializeList(list);
        File.WriteAllText(filePath, content);
    }

    /// <summary>
    /// Generic method for loading a list from file
    /// </summary>
    public List<T>? LoadListFromFile<T>(string filePath)
    {
        if (!File.Exists(filePath))
            return default;

        string content = File.ReadAllText(filePath);
        return DeserializeList<T>(content);
    }
}