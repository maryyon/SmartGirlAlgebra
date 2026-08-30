namespace SmartGirlAlgebra.Models;

/// <summary>
/// One step of working that she has to PRODUCE, not pick.
///
/// There are no choices here on purpose. Four options can be guessed at one in
/// four, and a fixed set of problems can be memorised outright — which is what
/// was happening. Typing the answer removes both.
/// </summary>
public class TypedStep
{
    /// <summary>The instruction, typed out on screen so she can watch the setup.</summary>
    public string Say { get; set; } = "";

    /// <summary>What she is being asked to enter.</summary>
    public string Ask { get; set; } = "";

    /// <summary>The canonical answer.</summary>
    public string Answer { get; set; } = "";

    /// <summary>Other forms that are just as correct (0.5 for 1/2, and so on).</summary>
    public string[] Accept { get; set; } = [];

    /// <summary>Gentlest first. The last one may give it away; stuck is not a lesson.</summary>
    public string[] Hints { get; set; } = [];

    public string Yes { get; set; } = "";

    /// <summary>Greyed text in the box, e.g. "a number" or "a fraction like 3/4".</summary>
    public string Placeholder { get; set; } = "answer";
}

/// <summary>One problem, generated fresh, worked all the way through.</summary>
public class WorkedProblem
{
    public string Setup { get; set; } = "";
    public List<TypedStep> Steps { get; set; } = [];
    public string Recap { get; set; } = "";
}

/// <summary>
/// One skill in the semester. The problems are generated, not stored, so the
/// numbers are different every time and there is nothing to memorise.
/// </summary>
public class SkillDef
{
    public int Id { get; set; }

    /// <summary>Which course this belongs to. A version follows exactly one.</summary>
    public string Track { get; set; } = "core";

    public int Unit { get; set; }
    public string Title { get; set; } = "";
    public string Subtitle { get; set; } = "";

    /// <summary>Shown to a grown-up, not to her. "4th", "5th", "pre-algebra".</summary>
    public string Grade { get; set; } = "";

    /// <summary>How many problems she must work correctly before it counts as learned.</summary>
    public int ToPass { get; set; } = 3;
}

/// <summary>The words a version dresses its problems in.</summary>
public class Flavor
{
    public string Place { get; set; } = "the studio";
    public string People { get; set; } = "artists";
    public string Person { get; set; } = "artist";
    public string Things { get; set; } = "paint tubes";
    public string Thing { get; set; } = "paint tube";
    public string Boxes { get; set; } = "shelves";
    public string Box { get; set; } = "shelf";
    public string Event { get; set; } = "the art show";
    public string Money { get; set; } = "a poster";

    public static readonly Flavor Studio = new();

    public static readonly Flavor Court = new()
    {
        Place = "the gym",
        People = "players",
        Person = "player",
        Things = "points",
        Thing = "point",
        Boxes = "quarters",
        Box = "quarter",
        Event = "the tournament",
        Money = "a ticket"
    };

    public static readonly Flavor Stage = new()
    {
        Place = "the theater",
        People = "dancers",
        Person = "dancer",
        Things = "tickets",
        Thing = "ticket",
        Boxes = "rows",
        Box = "row",
        Event = "the recital",
        Money = "a ticket"
    };
}

/// <summary>
/// A semester, in order, starting a grade BELOW where she is and finishing
/// above it. Six units, four skills each. Nothing unlocks until the one before
/// it has been worked correctly.
/// </summary>
public static class Curriculum
{
    /// <summary>Every skill in every track.</summary>
    public static readonly SkillDef[] All =
    [
        // ---- Unit 1 — where we start: fourth grade, on purpose -------------
        new() { Id = 101, Track = "core", Unit = 1, Grade = "4th", Title = "Place value",          Subtitle = "What each digit is worth" },
        new() { Id = 102, Track = "core", Unit = 1, Grade = "4th", Title = "Adding big numbers",    Subtitle = "Carrying, one column at a time" },
        new() { Id = 103, Track = "core", Unit = 1, Grade = "4th", Title = "Taking away",           Subtitle = "Borrowing, one column at a time" },
        new() { Id = 104, Track = "core", Unit = 1, Grade = "4th", Title = "Times a single digit",  Subtitle = "Breaking it into parts" },

        // ---- Unit 2 — bigger numbers ---------------------------------------
        new() { Id = 201, Track = "core", Unit = 2, Grade = "4th", Title = "Factors",               Subtitle = "What divides into it evenly" },
        new() { Id = 202, Track = "core", Unit = 2, Grade = "4th", Title = "Multiples",             Subtitle = "Counting up in steps" },
        new() { Id = 203, Track = "core", Unit = 2, Grade = "4th", Title = "Two digits times two",  Subtitle = "Four pieces, then add them" },
        new() { Id = 204, Track = "core", Unit = 2, Grade = "4th", Title = "Long division",         Subtitle = "One digit at a time, with a remainder" },

        // ---- Unit 3 — fractions arrive -------------------------------------
        new() { Id = 301, Track = "core", Unit = 3, Grade = "4th", Title = "Equal fractions",       Subtitle = "Same amount, different numbers" },
        new() { Id = 302, Track = "core", Unit = 3, Grade = "4th", Title = "Simplest form",         Subtitle = "Cutting it down to size" },
        new() { Id = 303, Track = "core", Unit = 3, Grade = "4th", Title = "Which is bigger",       Subtitle = "Comparing with a common bottom" },
        new() { Id = 304, Track = "core", Unit = 3, Grade = "4th", Title = "Same-bottom fractions", Subtitle = "Adding and taking away" },

        // ---- Unit 4 — fractions and decimals -------------------------------
        new() { Id = 401, Track = "core", Unit = 4, Grade = "5th", Title = "Different bottoms",     Subtitle = "Make them match first" },
        new() { Id = 402, Track = "core", Unit = 4, Grade = "5th", Title = "Times a fraction",      Subtitle = "Across the top, across the bottom" },
        new() { Id = 403, Track = "core", Unit = 4, Grade = "5th", Title = "Decimal place value",   Subtitle = "Tenths, hundredths, thousandths" },
        new() { Id = 404, Track = "core", Unit = 4, Grade = "5th", Title = "Adding decimals",       Subtitle = "Line up the points" },

        // ---- Unit 5 — operations -------------------------------------------
        new() { Id = 501, Track = "core", Unit = 5, Grade = "5th", Title = "Times a decimal",       Subtitle = "Multiply, then count the places" },
        new() { Id = 502, Track = "core", Unit = 5, Grade = "5th", Title = "Sharing decimals",      Subtitle = "Dividing by a whole number" },
        new() { Id = 503, Track = "core", Unit = 5, Grade = "5th", Title = "Order of operations",   Subtitle = "What gets done first" },
        new() { Id = 504, Track = "core", Unit = 5, Grade = "5th", Title = "Powers of ten",         Subtitle = "Moving the point" },

        // ---- Unit 6 — into algebra, above grade ----------------------------
        new() { Id = 601, Track = "core", Unit = 6, Grade = "5th",         Title = "Working out expressions", Subtitle = "Putting a number in for the letter" },
        new() { Id = 602, Track = "core", Unit = 6, Grade = "pre-algebra", Title = "Words into maths",        Subtitle = "Writing what it says" },
        new() { Id = 603, Track = "core", Unit = 6, Grade = "pre-algebra", Title = "One-step equations",      Subtitle = "Undoing one thing" },
        new() { Id = 604, Track = "core", Unit = 6, Grade = "pre-algebra", Title = "Two-step equations",      Subtitle = "Undoing two things, in order" },

        // ================= Algebra 1, for a ninth grader ====================
        new() { Id = 1101, Track = "algebra1", Unit = 1, Grade = "9th", Title = "Positives and negatives", Subtitle = "Minus a minus is a plus" },
        new() { Id = 1102, Track = "algebra1", Unit = 1, Grade = "9th", Title = "Order with powers",       Subtitle = "What gets done first" },
        new() { Id = 1103, Track = "algebra1", Unit = 1, Grade = "9th", Title = "Evaluating expressions",  Subtitle = "Substituting, signs and all" },
        new() { Id = 1104, Track = "algebra1", Unit = 1, Grade = "9th", Title = "Like terms",              Subtitle = "What can and cannot be joined" },

        new() { Id = 1201, Track = "algebra1", Unit = 2, Grade = "9th", Title = "Two-step equations",      Subtitle = "Undo it in reverse order" },
        new() { Id = 1202, Track = "algebra1", Unit = 2, Grade = "9th", Title = "x on both sides",         Subtitle = "Get them onto one side" },
        new() { Id = 1203, Track = "algebra1", Unit = 2, Grade = "9th", Title = "Brackets",                Subtitle = "Multiply everything inside" },
        new() { Id = 1204, Track = "algebra1", Unit = 2, Grade = "9th", Title = "Rearranging formulas",    Subtitle = "Solving when the answer is letters" },

        new() { Id = 1301, Track = "algebra1", Unit = 3, Grade = "9th", Title = "Inequalities",            Subtitle = "The sign flips on a negative" },
        new() { Id = 1302, Track = "algebra1", Unit = 3, Grade = "9th", Title = "Proportions",             Subtitle = "Cross-multiply" },
        new() { Id = 1303, Track = "algebra1", Unit = 3, Grade = "9th", Title = "Percent",                 Subtitle = "Out of a hundred" },
        new() { Id = 1304, Track = "algebra1", Unit = 3, Grade = "9th", Title = "Unit rate",               Subtitle = "How much for one" },

        new() { Id = 1401, Track = "algebra1", Unit = 4, Grade = "9th", Title = "Slope from two points",   Subtitle = "Rise over run" },
        new() { Id = 1402, Track = "algebra1", Unit = 4, Grade = "9th", Title = "y = mx + b",              Subtitle = "Reading a line off its equation" },
        new() { Id = 1403, Track = "algebra1", Unit = 4, Grade = "9th", Title = "Writing the equation",    Subtitle = "From a point and a slope" },
        new() { Id = 1404, Track = "algebra1", Unit = 4, Grade = "9th", Title = "Intercepts",              Subtitle = "Where it crosses the axes" },

        new() { Id = 1501, Track = "algebra1", Unit = 5, Grade = "9th", Title = "Systems by substitution", Subtitle = "Put one into the other" },
        new() { Id = 1502, Track = "algebra1", Unit = 5, Grade = "9th", Title = "Systems by elimination",  Subtitle = "Add them and cancel" },
        new() { Id = 1503, Track = "algebra1", Unit = 5, Grade = "9th", Title = "Exponent rules",          Subtitle = "Same base, add the powers" },
        new() { Id = 1504, Track = "algebra1", Unit = 5, Grade = "9th", Title = "Scientific notation",     Subtitle = "Big numbers, short form" },

        new() { Id = 1601, Track = "algebra1", Unit = 6, Grade = "9th", Title = "Multiplying brackets",    Subtitle = "Four products, then tidy" },
        new() { Id = 1602, Track = "algebra1", Unit = 6, Grade = "9th", Title = "Factorising",             Subtitle = "Expanding, backwards" },
        new() { Id = 1603, Track = "algebra1", Unit = 6, Grade = "9th", Title = "Solving quadratics",      Subtitle = "Two answers, not one" },
        new() { Id = 1604, Track = "algebra1", Unit = 6, Grade = "9th", Title = "Pythagoras",              Subtitle = "a² + b² = c²" },
    ];

    public static SkillDef? ById(int id) => All.FirstOrDefault(s => s.Id == id);

    /// <summary>The course one version follows, in order.</summary>
    public static SkillDef[] Skills(string track) =>
        All.Where(s => s.Track == (string.IsNullOrWhiteSpace(track) ? "core" : track)).ToArray();

    public static int[] Units(string track) =>
        Skills(track).Select(s => s.Unit).Distinct().OrderBy(u => u).ToArray();

    public static SkillDef[] InUnit(string track, int unit) =>
        Skills(track).Where(s => s.Unit == unit).ToArray();

    /// <summary>What each unit is called on the way through.</summary>
    public static string UnitName(string track, int unit) => track switch
    {
        "algebra1" => unit switch
        {
            1 => "Foundations",
            2 => "Solving equations",
            3 => "Inequalities and rates",
            4 => "Lines and slope",
            5 => "Systems and exponents",
            6 => "Quadratics",
            _ => $"Unit {unit}"
        },
        _ => unit switch
        {
            1 => "Warm-up",
            2 => "Bigger numbers",
            3 => "Fractions",
            4 => "Fractions and decimals",
            5 => "Working with decimals",
            6 => "Into algebra",
            _ => $"Unit {unit}"
        }
    };
}
