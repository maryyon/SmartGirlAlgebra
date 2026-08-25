namespace SmartGirlAlgebra.Models;

public class WalkStep
{
    public string Title { get; set; } = "";
    public string Body { get; set; } = "";
    public string Math { get; set; } = "";
}

/// <summary>
/// One problem. Serves both play modes — the step-by-step whiteboard uses
/// Equation, Chips and Walkthrough; the tap-the-answer mode uses Choices.
/// Loaded from a version's JSON file, never hardcoded.
/// </summary>
public class Level
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Tagline { get; set; } = "";
    public string Difficulty { get; set; } = "";

    /// <summary>Tile colour on the level list. Comes from the version's palette.</summary>
    public string Color { get; set; } = "";

    /// <summary>The story wrapped around the problem.</summary>
    public string Cheer { get; set; } = "";

    public string Question { get; set; } = "";
    public string[] Hints { get; set; } = [];

    // ---- step-by-step mode ----

    public string Equation { get; set; } = "";
    public string VariableMeaning { get; set; } = "";

    /// <summary>The finish line as written, e.g. "x = 9".</summary>
    public string Answer { get; set; } = "";

    /// <summary>The answer as a number, which is what every line is checked against.</summary>
    public double AnswerValue { get; set; }

    /// <summary>Symbols offered as tap-targets above the keyboard.</summary>
    public string[] Chips { get; set; } = [];

    public WalkStep[] Walkthrough { get; set; } = [];

    // ---- tap-the-answer mode ----

    /// <summary>
    /// The options shown as buttons. Presented in the order given, so wrong
    /// answers should be plausible near-misses rather than obvious filler.
    /// </summary>
    public string[] Choices { get; set; } = [];

    /// <summary>Which of Choices is right. Compared as text, so "5" must match "5".</summary>
    public string CorrectChoice { get; set; } = "";
}
