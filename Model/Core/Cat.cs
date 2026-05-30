namespace Model.Core;

/// <summary>
/// Cat class representing cats in the shelter
/// </summary>
public class Cat : Pet
{
    public string FurColor { get; set; }
    public bool IsNeutered { get; set; }

    // Default claustrophobia value for cats: true (cats prefer open spaces)
    public Cat() : base()
    {
        FurColor = string.Empty;
        IsNeutered = false;
        HasClaustrophobia = true;
    }

    public Cat(string name, int age, double weight) 
        : base(name, age, weight)
    {
        FurColor = string.Empty;
        IsNeutered = false;
        HasClaustrophobia = true;
    }

    public Cat(string name, int age, double weight, string furColor, bool isNeutered)
        : base(name, age, weight)
    {
        FurColor = furColor;
        IsNeutered = isNeutered;
        HasClaustrophobia = true;
    }

    public Cat(string name, int age, double weight, string furColor, bool isNeutered, bool hasClaustrophobia)
        : base(name, age, weight, hasClaustrophobia)
    {
        FurColor = furColor;
        IsNeutered = isNeutered;
    }

    /// <summary>
    /// Override MakeSound for cat
    /// </summary>
    public override string MakeSound()
    {
        return "Meow!";
    }

    /// <summary>
    /// Override ToString for detailed cat information
    /// </summary>
    public override string ToString()
    {
        return $"Cat: {Name}, Age: {Age}, Weight: {Weight:F1}kg, Fur: {FurColor}, Neutered: {IsNeutered}, Claustrophobic: {HasClaustrophobia}";
    }
}