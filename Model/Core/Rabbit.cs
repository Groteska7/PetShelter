namespace Model.Core;

/// <summary>
/// Rabbit class representing rabbits in the shelter
/// </summary>
public class Rabbit : Pet
{
    public double EarLength { get; set; }  // Length of ears in cm
    public string FurType { get; set; }     // e.g., "Short", "Long", "Rex"

    // Default claustrophobia value for rabbits: true (rabbits need space to hop)
    public Rabbit() : base()
    {
        EarLength = 0;
        FurType = string.Empty;
        HasClaustrophobia = true;
    }

    public Rabbit(string name, int age, double weight) 
        : base(name, age, weight)
    {
        EarLength = 0;
        FurType = string.Empty;
        HasClaustrophobia = true;
    }

    public Rabbit(string name, int age, double weight, double earLength, string furType)
        : base(name, age, weight)
    {
        EarLength = earLength;
        FurType = furType;
        HasClaustrophobia = true;
    }

    public Rabbit(string name, int age, double weight, double earLength, string furType, bool hasClaustrophobia)
        : base(name, age, weight, hasClaustrophobia)
    {
        EarLength = earLength;
        FurType = furType;
    }

    /// <summary>
    /// Override MakeSound for rabbit
    /// </summary>
    public override string MakeSound()
    {
        return "*soft squeak*";
    }

    /// <summary>
    /// Override ToString for detailed rabbit information
    /// </summary>
    public override string ToString()
    {
        return $"Rabbit: {Name}, Age: {Age}, Weight: {Weight:F1}kg, Ear Length: {EarLength:F1}cm, Fur: {FurType}, Claustrophobic: {HasClaustrophobia}";
    }
}