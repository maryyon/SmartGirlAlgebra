namespace SmartGirlAlgebra.Models;

/// <summary>
/// Represents an algebraic expression made up of terms (e.g., "2x + 3")
/// </summary>
public class Expression
{
    /// <summary>
    /// The terms that make up this expression
    /// </summary>
    public List<Term> Terms { get; set; }

    public Expression()
    {
        Terms = new List<Term>();
    }

    public Expression(List<Term> terms)
    {
        Terms = terms;
    }

    /// <summary>
    /// Adds a term to this expression
    /// </summary>
    public void AddTerm(Term term)
    {
        Terms.Add(term);
    }

    public override string ToString()
    {
        if (Terms.Count == 0)
            return "0";

        var result = Terms[0].ToString();
        
        for (int i = 1; i < Terms.Count; i++)
        {
            var term = Terms[i];
            if (term.Coefficient >= 0)
                result += " + " + term.ToString();
            else
                result += " - " + new Term(-term.Coefficient, term.Variable!).ToString();
        }

        return result;
    }
}

