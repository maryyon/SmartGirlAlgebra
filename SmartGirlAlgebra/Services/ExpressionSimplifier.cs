using SmartGirlAlgebra.Models;

namespace SmartGirlAlgebra.Services;

/// <summary>
/// Simplifies algebraic expressions by combining like terms
/// </summary>
public class ExpressionSimplifier
{
    /// <summary>
    /// Simplifies an expression by combining like terms
    /// For example: "2x + 3x + 5" becomes "5x + 5"
    /// </summary>
    public Expression Simplify(Expression expression)
    {
        if (expression.Terms.Count == 0)
            return new Expression();

        // Group terms by variable
        var termGroups = new Dictionary<string, decimal>();

        foreach (var term in expression.Terms)
        {
            string key = term.Variable ?? ""; // Empty string for constants
            
            if (termGroups.ContainsKey(key))
                termGroups[key] += term.Coefficient;
            else
                termGroups[key] = term.Coefficient;
        }

        // Convert back to terms, filtering out zero coefficients
        var simplifiedTerms = new List<Term>();
        
        foreach (var group in termGroups)
        {
            if (group.Value != 0) // Skip terms with zero coefficient
            {
                if (string.IsNullOrEmpty(group.Key))
                    simplifiedTerms.Add(new Term(group.Value)); // Constant
                else
                    simplifiedTerms.Add(new Term(group.Value, group.Key)); // Variable term
            }
        }

        // Sort: variables first (alphabetically), then constants
        simplifiedTerms = simplifiedTerms
            .OrderBy(t => t.IsConstant ? 1 : 0)
            .ThenBy(t => t.Variable ?? "")
            .ToList();

        return new Expression(simplifiedTerms);
    }
}

