using System.Globalization;
using System.Text;
using SmartGirlAlgebra.Models;

namespace SmartGirlAlgebra.Services;

/// <summary>
/// Decides whether what she typed is the right answer.
///
/// It has to be generous about FORM and strict about VALUE. "3/4", "3 / 4" and
/// "0.75" are the same answer; "$12.50", "12.50" and "12.5" are the same amount.
/// Marking a child wrong because she typed a dollar sign teaches her nothing
/// except that the machine is fussy.
///
/// It must never be generous about the number itself. That is the whole point.
/// </summary>
public static class Answers
{
    public static bool Matches(string? typed, TypedStep step)
    {
        if (string.IsNullOrWhiteSpace(typed)) return false;

        var given = Normalise(typed);
        if (given.Length == 0) return false;

        if (given == Normalise(step.Answer)) return true;

        foreach (var alt in step.Accept)
        {
            if (given == Normalise(alt)) return true;
        }

        // Same number written differently: 12.50 vs 12.5, 1,200 vs 1200.
        if (SameNumber(given, Normalise(step.Answer))) return true;

        foreach (var alt in step.Accept)
        {
            if (SameNumber(given, Normalise(alt))) return true;
        }

        // A fraction and its decimal are the same answer.
        if (SameFraction(given, Normalise(step.Answer))) return true;

        return false;
    }

    /// <summary>
    /// Strips the things that are about typing rather than maths: spaces,
    /// currency, commas in long numbers, a trailing full stop, capitals.
    /// </summary>
    private static string Normalise(string s)
    {
        var sb = new StringBuilder(s.Length);

        foreach (var c in s.Trim().ToLowerInvariant())
        {
            if (char.IsWhiteSpace(c)) continue;
            if (c is '$' or '£' or ',' or '"') continue;

            // Written either way on a keyboard.
            if (c == '×') { sb.Append('x'); continue; }   // ×
            if (c == '÷') { sb.Append('/'); continue; }   // ÷
            if (c == '−') { sb.Append('-'); continue; }   // −
            if (c == '*') { sb.Append('x'); continue; }

            sb.Append(c);
        }

        var result = sb.ToString();

        while (result.EndsWith('.') && result.Count(ch => ch == '.') == 1 && result.Length > 1)
        {
            result = result[..^1];
        }

        return result;
    }

    private static bool SameNumber(string a, string b)
    {
        if (!decimal.TryParse(a, NumberStyles.Any, CultureInfo.InvariantCulture, out var x)) return false;
        if (!decimal.TryParse(b, NumberStyles.Any, CultureInfo.InvariantCulture, out var y)) return false;

        return x == y;
    }

    /// <summary>
    /// 3/4 and 0.75 are the same answer. 6/8 and 3/4 are NOT — when the question
    /// asks for simplest form, accepting the unsimplified version would mark the
    /// wrong answer right and quietly teach the opposite of the lesson.
    ///
    /// So: fraction against decimal, compare the value. Fraction against
    /// fraction, it has to be the fraction that was asked for.
    /// </summary>
    private static bool SameFraction(string a, string b)
    {
        var aIsFraction = a.Contains('/');
        var bIsFraction = b.Contains('/');

        if (aIsFraction == bIsFraction) return false;

        var x = AsValue(a);
        var y = AsValue(b);

        if (x is null || y is null) return false;

        return Math.Abs(x.Value - y.Value) < 0.000001m;
    }

    private static decimal? AsValue(string s)
    {
        var slash = s.IndexOf('/');

        if (slash > 0 && slash < s.Length - 1)
        {
            var top = s[..slash];
            var bottom = s[(slash + 1)..];

            if (decimal.TryParse(top, NumberStyles.Any, CultureInfo.InvariantCulture, out var t) &&
                decimal.TryParse(bottom, NumberStyles.Any, CultureInfo.InvariantCulture, out var b) &&
                b != 0)
            {
                return t / b;
            }

            return null;
        }

        return decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : null;
    }
}
