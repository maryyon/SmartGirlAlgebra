namespace SmartGirlAlgebra.Services;

public enum CheckStatus
{
    Correct,
    Wrong,
    Solved
}

public record CheckResult(CheckStatus Status, string? Message = null);

/// <summary>
/// Checks a single line of a student's working.
///
/// Rather than comparing equations symbolically, every line is validated against the
/// KNOWN answer: a legal algebra move keeps the equation true when x = answer. That
/// holds for linear equations and squared ones alike, so the Pythagorean level works
/// without a separate code path. Lines that are true for every x are rejected
/// separately, because they are true without pinning anything down.
/// </summary>
public static class LineChecker
{
    private const double Eps = 1e-6;
    private const double Near = 1e-4;

    private static readonly string[] Encouragements =
        ["Almost", "So close", "Not quite", "Hold up", "Deep breath"];

    public static CheckResult Check(string line, double answer)
    {
        var trimmed = (line ?? string.Empty).Trim();

        if (trimmed.Length == 0)
        {
            return new CheckResult(CheckStatus.Wrong, "Write your next line and I'll cheer you on! 📣");
        }

        var equalsCount = trimmed.Count(c => c == '=');
        if (equalsCount == 0)
        {
            return new CheckResult(CheckStatus.Wrong,
                "Every line needs an equals sign (=)! Keep both sides balanced, squad. 📣");
        }

        if (equalsCount > 1)
        {
            return new CheckResult(CheckStatus.Wrong,
                "One equals sign per line, please — this isn't a group chat. ✨");
        }

        var parts = trimmed.Split('=');
        var left = Normalize(parts[0]);
        var right = Normalize(parts[1]);

        if (left.Length == 0 || right.Length == 0)
        {
            return new CheckResult(CheckStatus.Wrong,
                "Hmm, I couldn't read that line. Check your symbols and try again! 💭");
        }

        double Difference(double x) => Evaluate(left, x) - Evaluate(right, x);

        double atAnswer;
        try
        {
            atAnswer = Difference(answer);
            if (double.IsNaN(atAnswer) || double.IsInfinity(atAnswer)) throw new FormatException();
        }
        catch
        {
            return new CheckResult(CheckStatus.Wrong,
                "Hmm, I couldn't read that line. Check your symbols and try again! 💭");
        }

        // True for every x? Charming, but it doesn't pin down x.
        double[] samples = [answer + 0.618, answer + 1.414, answer - 2.236, answer + 3.301];
        var trueEverywhere = Math.Abs(atAnswer) < Near && samples.All(s => SafeNearZero(Difference, s));
        if (trueEverywhere)
        {
            return new CheckResult(CheckStatus.Wrong,
                "That line is true no matter what — adorable, but we still need to find x! Keep going. ✨");
        }

        if (Math.Abs(atAnswer) < Near)
        {
            return IsSolvedForm(left, right, answer)
                ? new CheckResult(CheckStatus.Solved)
                : new CheckResult(CheckStatus.Correct);
        }

        // Wrong — try to name what slipped.
        if (Math.Abs(answer) > Eps && SafeNearZero(Difference, -answer))
        {
            return new CheckResult(CheckStatus.Wrong,
                $"{Encourage()}! A sign did a backflip 🤸 — check your ➕ and ➖.");
        }

        if (SafeNearZero(Difference, answer * 2) ||
            (Math.Abs(answer) > Eps && SafeNearZero(Difference, answer / 2)))
        {
            return new CheckResult(CheckStatus.Wrong,
                $"{Encourage()}! Did you divide (or multiply) BOTH sides all the way? ✨");
        }

        return new CheckResult(CheckStatus.Wrong,
            $"{Encourage()}, superstar! The two sides stopped balancing — peek at your last move. 🦅");
    }

    private static bool SafeNearZero(Func<double, double> f, double x)
    {
        try
        {
            var v = f(x);
            return !double.IsNaN(v) && Math.Abs(v) < Near;
        }
        catch
        {
            return false;
        }
    }

    private static string Encourage() =>
        Encouragements[Random.Shared.Next(Encouragements.Length)];

    /// <summary>Is this the finish line — literally "x = answer", either way round?</summary>
    private static bool IsSolvedForm(string left, string right, double answer)
    {
        if (left == "x" && IsConstant(right, out var r)) return Math.Abs(r - answer) < Near;
        if (right == "x" && IsConstant(left, out var l)) return Math.Abs(l - answer) < Near;
        return false;
    }

    private static bool IsConstant(string expr, out double value)
    {
        value = double.NaN;
        try
        {
            var a = Evaluate(expr, 0);
            var b = Evaluate(expr, 1);
            if (Math.Abs(a - b) >= Eps) return false;
            value = a;
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Turns what a student types into something the parser can read: lower case,
    /// no spaces, explicit multiplication, and ² rewritten as ^2.
    /// </summary>
    private static string Normalize(string input)
    {
        var s = input.ToLowerInvariant();
        s = string.Concat(s.Where(c => !char.IsWhiteSpace(c)));

        s = s.Replace('×', '*').Replace('÷', '/');
        s = s.Replace('–', '-').Replace('—', '-').Replace('−', '-');
        s = s.Replace("²", "^2").Replace("³", "^3");

        s = System.Text.RegularExpressions.Regex.Replace(s, @"(\d)(x|\()", "$1*$2");
        s = System.Text.RegularExpressions.Regex.Replace(s, @"(x)(\()", "$1*$2");
        s = System.Text.RegularExpressions.Regex.Replace(s, @"(\))(\d|x|\()", "$1*$2");
        s = System.Text.RegularExpressions.Regex.Replace(s, @"(x)(\d)", "$1*$2");

        return s;
    }

    private static double Evaluate(string expression, double x) =>
        new Parser(expression).Parse(x);

    /// <summary>Recursive-descent evaluator for + - * / ^ ( ) and the variable x.</summary>
    private sealed class Parser
    {
        private readonly string _s;
        private int _i;

        public Parser(string s) => _s = s;

        private char Peek => _i < _s.Length ? _s[_i] : '\0';
        private bool Eof => _i >= _s.Length;

        public double Parse(double x)
        {
            var v = Expr(x);
            if (!Eof) throw new FormatException("unexpected token");
            return v;
        }

        private double Expr(double x)
        {
            var v = Term(x);
            while (Peek is '+' or '-')
            {
                var op = _s[_i++];
                var t = Term(x);
                v = op == '+' ? v + t : v - t;
            }

            return v;
        }

        private double Term(double x)
        {
            var v = Factor(x);
            while (Peek is '*' or '/')
            {
                var op = _s[_i++];
                var f = Factor(x);
                v = op == '*' ? v * f : v / f;
            }

            return v;
        }

        // Power binds tighter than * and /, and is right-associative.
        private double Factor(double x)
        {
            var b = Base(x);
            if (Peek != '^') return b;

            _i++;
            var exp = Factor(x);
            return Math.Pow(b, exp);
        }

        private double Base(double x)
        {
            if (Peek == '+')
            {
                _i++;
                return Base(x);
            }

            if (Peek == '-')
            {
                _i++;
                return -Base(x);
            }

            if (Peek == '(')
            {
                _i++;
                var v = Expr(x);
                if (Peek != ')') throw new FormatException("missing )");
                _i++;
                return v;
            }

            if (Peek == 'x')
            {
                _i++;
                return x;
            }

            var start = _i;
            while (!Eof && (char.IsDigit(Peek) || Peek == '.')) _i++;
            if (_i == start) throw new FormatException("unexpected token");

            return double.Parse(_s[start.._i], System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
