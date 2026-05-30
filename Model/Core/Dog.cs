namespace Model.Core;

/// <summary>
/// Dog class representing dogs in the shelter
/// </summary>
public class Dog : Pet
{
    public string Breed { get; set; }
    public bool IsTrained { get; set; }

    // Default claustrophobia value for dogs: false (dogs are more adaptable)
    public Dog() : base()
    {
        Breed = string.Empty;
        IsTrained = false;
        HasClaustrophobia = false;
    }

    public Dog(string name, int age, double weight) 
        : base(name, age, weight)
    {
        Breed = string.Empty;
        IsTrained = false;
        HasClaustrophobia = false;
    }

    public Dog(string name, int age, double weight, string breed, bool isTrained)
        : base(name, age, weight)
    {
        Breed = breed;
        IsTrained = isTrained;
        HasClaustrophobia = false;
    }

    public Dog(string name, int age, double weight, string breed, bool isTrained, bool hasClaustrophobia)
        : base(name, age, weight, hasClaustrophobia)
    {
        Breed = breed;
        IsTrained = isTrained;
    }

    /// <summary>
    /// Override MakeSound for dog
    /// </summary>
    public override string MakeSound()
    {
        return "Woof!";
    }

    /// <summary>
    /// Override ToString for detailed dog information
    /// </summary>
    public override string ToString()
    {
        return $"Dog: {Name}, Age: {Age}, Weight: {Weight:F1}kg, Breed: {Breed}, Trained: {IsTrained}, Claustrophobic: {HasClaustrophobia}";
    }
}