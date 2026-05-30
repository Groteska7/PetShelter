namespace Model.Core;

/// <summary>
/// Interface for filtering animals by type
/// </summary>
public interface IFilter
{
    /// <summary>
    /// Filters animals by their type
    /// </summary>
    /// <param name="type">The type of animal to filter by</param>
    /// <returns>List of animals matching the specified type</returns>
    List<Pet> Filter(Type type);
}