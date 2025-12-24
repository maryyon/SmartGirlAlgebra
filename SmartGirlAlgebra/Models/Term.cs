namespace SmartGirlAlgebra.Models;

/// <summary>
/// Represents a single term in an algebraic expression (e.g., "3x" or "5")
/// </summary>
public class Term
{
    /// <summary>
    /// The numerical coefficient (e.g., 3 in "3x")
    /// </summary>
    public decimal Coefficient { get; set; }

    /// <summary>
    /// The variable name (e.g., "x" in "3x"), or null for constants
    /// </summary>
    public string? Variable { get; set; }

    /// <summary>
    /// Creates a constant term (no variable)
    /// </summary>
    public Term(decimal coefficient)
    {
        Coefficient = coefficient;
        Variable = null;
    }

    /// <summary>
    /// Creates a term with a variable
    /// </summary>
    public Term(decimal coefficient, string variable)
    {
        Coefficient = coefficient;
        Variable = variable;
    }

    /// <summary>
    /// Returns true if this is a constant term (no variable)
    /// </summary>
    public bool IsConstant => Variable == null;

    /// <summary>
    /// Returns true if this term has the same variable as another term
    /// </summary>
    public bool HasSameVariable(Term other)
    {
        return Variable == other.Variable;
    }

    public override string ToString()
    {
        if (IsConstant)
            return Coefficient.ToString();
        
        if (Coefficient == 1)
            return Variable!;
        
        if (Coefficient == -1)
            return $"-{Variable}";
        
        return $"{Coefficient}{Variable}";
    }
}

