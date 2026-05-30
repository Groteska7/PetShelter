using System.Xml;
using System.Xml.Serialization;
using Model.Core;

namespace Model.Data;

/// <summary>
/// XML serializer implementation using System.Xml.Serialization
/// </summary>
public class XmlSerializer : Serializer
{
    private readonly XmlWriterSettings _writerSettings;

    public XmlSerializer()
    {
        _writerSettings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "  ",
            NewLineChars = "\r\n",
            NewLineHandling = NewLineHandling.Replace,
            OmitXmlDeclaration = false,
            Encoding = System.Text.Encoding.UTF8
        };
    }

    /// <summary>
    /// Serialize an object to XML string
    /// </summary>
    public override string Serialize<T>(T obj)
    {
        if (obj == null)
            return string.Empty;

        using var stringWriter = new StringWriter();
        using var xmlWriter = XmlWriter.Create(stringWriter, _writerSettings);

        var serializer = new XmlSerializerFactory().CreateSerializer(typeof(T));
        serializer.Serialize(xmlWriter, obj);
        xmlWriter.Flush();

        return stringWriter.ToString();
    }

    /// <summary>
    /// Deserialize an XML string to an object
    /// </summary>
    public override T? Deserialize<T>(string data)
    {
        if (string.IsNullOrEmpty(data))
            return default;

        using var stringReader = new StringReader(data);
        using var xmlReader = XmlReader.Create(stringReader);

        var serializer = new XmlSerializerFactory().CreateSerializer(typeof(T));
        return (T?)serializer.Deserialize(xmlReader);
    }

    /// <summary>
    /// Serialize a list of objects to XML string
    /// </summary>
    public override string SerializeList<T>(List<T> list)
    {
        if (list == null || list.Count == 0)
            return "<ArrayOf" + typeof(T).Name + " />";

        using var stringWriter = new StringWriter();
        using var xmlWriter = XmlWriter.Create(stringWriter, _writerSettings);

        var serializer = new XmlSerializerFactory().CreateSerializer(typeof(List<T>));
        serializer.Serialize(xmlWriter, list);
        xmlWriter.Flush();

        return stringWriter.ToString();
    }

    /// <summary>
    /// Deserialize an XML string to a list of objects
    /// </summary>
    public override List<T>? DeserializeList<T>(string data)
    {
        if (string.IsNullOrEmpty(data))
            return default;

        using var stringReader = new StringReader(data);
        using var xmlReader = XmlReader.Create(stringReader);

        var serializer = new XmlSerializerFactory().CreateSerializer(typeof(List<T>));
        return (List<T>?)serializer.Deserialize(xmlReader);
    }

    /// <summary>
    /// Serialize a shelter with its pets
    /// </summary>
    public string SerializeShelter(Shelter shelter)
    {
        return Serialize(shelter);
    }

    /// <summary>
    /// Deserialize a shelter from XML string
    /// </summary>
    public Shelter? DeserializeShelter(string data)
    {
        return Deserialize<Shelter>(data);
    }
}