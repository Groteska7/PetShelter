using Newtonsoft.Json;
using Model.Core;

namespace Model.Data;

/// <summary>
/// JSON serializer implementation using Newtonsoft.Json
/// </summary>
public class JsonSerializer : Serializer
{
    private readonly JsonSerializerSettings _settings;

    public JsonSerializer()
    {
        _settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };
    }

    /// <summary>
    /// Serialize an object to JSON string
    /// </summary>
    public override string Serialize<T>(T obj)
    {
        return JsonConvert.SerializeObject(obj, _settings);
    }

    /// <summary>
    /// Deserialize a JSON string to an object
    /// </summary>
    public override T? Deserialize<T>(string data)
    {
        if (string.IsNullOrEmpty(data))
            return default;

        return JsonConvert.DeserializeObject<T>(data, _settings);
    }

    /// <summary>
    /// Serialize a list of objects to JSON string
    /// </summary>
    public override string SerializeList<T>(List<T> list)
    {
        return JsonConvert.SerializeObject(list, _settings);
    }

    /// <summary>
    /// Deserialize a JSON string to a list of objects
    /// </summary>
    public override List<T>? DeserializeList<T>(string data)
    {
        if (string.IsNullOrEmpty(data))
            return default;

        return JsonConvert.DeserializeObject<List<T>>(data, _settings);
    }

    /// <summary>
    /// Serialize a shelter with its pets
    /// </summary>
    public string SerializeShelter(Shelter shelter)
    {
        return JsonConvert.SerializeObject(shelter, _settings);
    }

    /// <summary>
    /// Deserialize a shelter from JSON string
    /// </summary>
    public Shelter? DeserializeShelter(string data)
    {
        if (string.IsNullOrEmpty(data))
            return null;

        return JsonConvert.DeserializeObject<Shelter>(data, _settings);
    }
}