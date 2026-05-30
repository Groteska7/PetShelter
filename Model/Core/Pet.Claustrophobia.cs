namespace Model.Core;

/// <summary>
/// Partial class for Pet - claustrophobia-specific constructor
/// </summary>
public abstract partial class Pet
{
    /// <summary>
    /// Constructor with claustrophobia parameter
    /// </summary>
    /// <param name="name">Pet name</param>
    /// <param name="age">Pet age</param>
    /// <param name="weight">Pet weight</param>
    /// <param name="claustrophobia">Whether the pet has claustrophobia</param>
    protected Pet(string name, int age, double weight, string claustrophobia)
    {
        Name = name;
        Age = age;
        Weight = weight;
        HasClaustrophobia = ParseClaustrophobia(claustrophobia);
    }

    /// <summary>
    /// Parses claustrophobia value from string
    /// </summary>
    private static bool ParseClaustrophobia(string claustrophobia)
    {
        return !string.IsNullOrEmpty(claustrophobia) &&
               (claustrophobia.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                claustrophobia.Equals("да", StringComparison.OrdinalIgnoreCase) ||
                claustrophobia.Equals("1"));
    }
}