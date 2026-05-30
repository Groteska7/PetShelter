namespace Model.Core;

/// <summary>
/// Interface for counting animals in a shelter
/// </summary>
public interface ICountable
{
    /// <summary>
    /// Returns the total number of animals
    /// </summary>
    int Count();

    /// <summary>
    /// Returns the number of animals of a specific type
    /// </summary>
    /// <param name="type">The type of animal to count</param>
    int Count(Type type);

    /// <summary>
    /// Returns the percentage of animals of a specific type from the total
    /// </summary>
    /// <param name="type">The type of animal</param>
    /// <returns>Percentage as integer (0-100)</returns>
    int Percentage(Type type);
}