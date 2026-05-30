namespace Model.Core;

/// <summary>
/// Interface for adding and removing animals from a shelter
/// </summary>
public interface IChangeable
{
    /// <summary>
    /// Adds an animal to the shelter
    /// </summary>
    /// <param name="pet">The pet to add</param>
    /// <returns>True if successfully added, false otherwise</returns>
    bool AddPet(Pet pet);

    /// <summary>
    /// Removes an animal from the shelter
    /// </summary>
    /// <param name="pet">The pet to remove</param>
    /// <returns>True if successfully removed, false otherwise</returns>
    bool RemovePet(Pet pet);
}