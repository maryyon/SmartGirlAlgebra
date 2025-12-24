using SmartGirlAlgebra.Models;

namespace SmartGirlAlgebra.Services;

/// <summary>
/// Evaluates algebraic expressions by substituting variable values
/// </summary>
public class ExpressionEvaluator
{
    /// <summary>
    /// Evaluates an expression by substituting variable values
    /// For example: "2x + 3" with x=5 evaluates to 13
    /// </summary>
    public decimal Evaluate(Expression expression, Dictionary<string, decimal> variableValues)
    {
        decimal result = 0;

        foreach (var term in expression.Terms)
        {
            if (term.IsConstant)
            {
                result += term.Coefficient;
            }
            else
            {
                if (!variableValues.ContainsKey(term.Variable!))
                    throw new ArgumentException($"No value provided for variable '{term.Variable}'");

                result += term.Coefficient * variableValues[term.Variable!];
            }
        }

        return result;
    }

    /// <summary>
    /// Convenience method for single-variable expressions
    /// </summary>
    public decimal Evaluate(Expression expression, string variable, decimal value)
    {
        return Evaluate(expression, new Dictionary<string, decimal> { { variable, value } });
    }
}

