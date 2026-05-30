namespace Model.Core;

/// <summary>
/// Abstract base class for all pets in the shelter
/// </summary>
public abstract partial class Pet
{
    public string Name { get; set; }
    public int Age { get; set; }
    public double Weight { get; set; }
    public bool HasClaustrophobia { get; set; }

    protected Pet()
    {
        Name = string.Empty;
        HasClaustrophobia = false;
    }

    protected Pet(string name, int age, double weight)
    {
        Name = name;
        Age = age;
        Weight = weight;
        HasClaustrophobia = false;
    }

    protected Pet(string name, int age, double weight, bool hasClaustrophobia)
    {
        Name = name;
        Age = age;
        Weight = weight;
        HasClaustrophobia = hasClaustrophobia;
    }

    /// <summary>
    /// Abstract method for making a sound - must be implemented by derived classes
    /// </summary>
    public abstract string MakeSound();

    /// <summary>
    /// Override ToString for display purposes
    /// </summary>
    public override string ToString()
    {
        return $"{GetType().Name}: {Name}, Age: {Age}, Weight: {Weight:F1}kg";
    }

    /// <summary>
    /// Override Equals for comparison
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is Pet other)
        {
            return Name == other.Name && 
                   Age == other.Age && 
                   Weight == other.Weight &&
                   GetType() == other.GetType();
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Age, Weight, GetType().Name);
    }

    /// <summary>
    /// Overloaded operator for comparing pets by weight
    /// </summary>
    public static bool operator >(Pet left, Pet right)
    {
        if (left is null || right is null)
            throw new ArgumentNullException(left is null ? nameof(left) : nameof(right));
        return left.Weight > right.Weight;
    }

    public static bool operator <(Pet left, Pet right)
    {
        if (left is null || right is null)
            throw new ArgumentNullException(left is null ? nameof(left) : nameof(right));
        return left.Weight < right.Weight;
    }

    /// <summary>
    /// Generic method for creating a list of pets of a specific type
    /// </summary>
    public static List<T> CreatePetList<T>() where T : Pet, new()
    {
        return new List<T>();
    }
}