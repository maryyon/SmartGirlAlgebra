using SmartGirlAlgebra.Models;

namespace SmartGirlAlgebra.Services;

/// <summary>
/// Parses string representations of equations into Equation objects
/// </summary>
public class EquationParser
{
    private readonly ExpressionParser _expressionParser = new();

    /// <summary>
    /// Parses a string like "2x + 3 = 11" into an Equation object
    /// </summary>
    public Equation Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Input cannot be empty", nameof(input));

        // Split on the equals sign
        var parts = input.Split('=');
        
        if (parts.Length != 2)
            throw new ArgumentException("Equation must have exactly one equals sign", nameof(input));

        var leftSide = _expressionParser.Parse(parts[0].Trim());
        var rightSide = _expressionParser.Parse(parts[1].Trim());

        return new Equation(leftSide, rightSide);
    }
}

