using SmartGirlAlgebra.Models;

namespace SmartGirlAlgebra.Services;

/// <summary>
/// Parses string representations of algebraic expressions into Expression objects
/// </summary>
public class ExpressionParser
{
    /// <summary>
    /// Parses a string like "2x + 3" into an Expression object
    /// </summary>
    public Expression Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Input cannot be empty", nameof(input));

        // Remove all whitespace
        input = input.Replace(" ", "");

        var terms = new List<Term>();
        var currentTerm = "";
        var currentSign = 1; // 1 for positive, -1 for negative

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            if (c == '+' || c == '-')
            {
                // Process the current term if we have one
                if (!string.IsNullOrEmpty(currentTerm))
                {
                    terms.Add(ParseTerm(currentTerm, currentSign));
                    currentTerm = "";
                }

                // Set sign for next term
                currentSign = c == '+' ? 1 : -1;
            }
            else
            {
                currentTerm += c;
            }
        }

        // Don't forget the last term
        if (!string.IsNullOrEmpty(currentTerm))
        {
            terms.Add(ParseTerm(currentTerm, currentSign));
        }

        return new Expression(terms);
    }

    private Term ParseTerm(string termStr, int sign)
    {
        // Handle cases like "3x", "x", "5", "-x", etc.
        
        // Find where the variable starts (first letter)
        int varIndex = -1;
        for (int i = 0; i < termStr.Length; i++)
        {
            if (char.IsLetter(termStr[i]))
            {
                varIndex = i;
                break;
            }
        }

        if (varIndex == -1)
        {
            // It's a constant (no variable)
            decimal coefficient = decimal.Parse(termStr) * sign;
            return new Term(coefficient);
        }
        else if (varIndex == 0)
        {
            // It's just a variable like "x" (coefficient is 1)
            string variable = termStr;
            return new Term(1 * sign, variable);
        }
        else
        {
            // It has both coefficient and variable like "3x"
            string coeffStr = termStr.Substring(0, varIndex);
            string variable = termStr.Substring(varIndex);
            
            decimal coefficient = decimal.Parse(coeffStr) * sign;
            return new Term(coefficient, variable);
        }
    }
}

