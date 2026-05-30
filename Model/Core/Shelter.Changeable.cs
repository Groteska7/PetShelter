namespace Model.Core;

/// <summary>
/// Partial class for Shelter - implements IChangeable for adding/removing pets
/// </summary>
public partial class Shelter : IChangeable
{
    /// <summary>
    /// Adds a pet to the shelter
    /// </summary>
    /// <param name="pet">The pet to add</param>
    /// <returns>True if successfully added, false if shelter is full or pet already exists</returns>
    public bool AddPet(Pet pet)
    {
        if (pet == null)
            throw new ArgumentNullException(nameof(pet));

        // Check if shelter is at capacity
        if (Pets.Count >= Capacity)
            return false;

        // Check if pet with same name and type already exists
        if (Pets.Any(p => p.Name == pet.Name && p.GetType() == pet.GetType()))
            return false;

        // Check claustrophobia constraint - animals with claustrophobia cannot be placed in closed shelters
        if (pet.HasClaustrophobia && !HasOpenArea)
            return false;

        Pets.Add(pet);
        return true;
    }

    /// <summary>
    /// Removes a pet from the shelter
    /// </summary>
    /// <param name="pet">The pet to remove</param>
    /// <returns>True if successfully removed, false if pet not found</returns>
    public bool RemovePet(Pet pet)
    {
        if (pet == null)
            return false;

        return Pets.Remove(pet);
    }

    /// <summary>
    /// Removes a pet by name and type
    /// </summary>
    public bool RemovePet(string name, Type type)
    {
        var pet = Pets.FirstOrDefault(p => p.Name == name && p.GetType() == type);
        if (pet == null)
            return false;

        return Pets.Remove(pet);
    }

    /// <summary>
    /// Checks if a specific pet is in this shelter
    /// </summary>
    public bool ContainsPet(Pet pet)
    {
        return Pets.Any(p => p.Equals(pet));
    }

    /// <summary>
    /// Gets pets that match a specific criteria using a delegate
    /// </summary>
    public List<Pet> GetPetsByCriteria(Func<Pet, bool> criteria)
    {
        return Pets.Where(criteria).ToList();
    }
}