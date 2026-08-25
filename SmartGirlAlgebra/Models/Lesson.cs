namespace SmartGirlAlgebra.Models;

/// <summary>One panel of teaching: an idea, said plainly, with one worked example.</summary>
public class TeachStep
{
    public string Heading { get; set; } = "";
    public string Body { get; set; } = "";

    /// <summary>The idea shown as maths. Optional — not every panel needs one.</summary>
    public string Example { get; set; } = "";
}

/// <summary>
/// A single question. Four options rather than typing, because these come thick and
/// fast and the point is whether she understood the idea, not whether she can work
/// a keyboard.
/// </summary>
public class Exercise
{
    /// <summary>The funny bit. Carries the question so it never reads like a test.</summary>
    public string Prompt { get; set; } = "";

    public string Question { get; set; } = "";
    public string[] Choices { get; set; } = [];
    public string Correct { get; set; } = "";

    /// <summary>Shown after answering, right or wrong. This is where the teaching lands.</summary>
    public string Explain { get; set; } = "";
}

/// <summary>
/// One idea, taught then applied.
///
/// The practice set doubles as an unannounced check: she is never told she is being
/// assessed, and nothing on screen says "test". If she does not reach the bar on
/// first attempts, the reinforce set teaches the same idea with different examples
/// and she goes round again. She only moves on once she has actually got it.
/// </summary>
public class Lesson
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Subtitle { get; set; } = "";

    public TeachStep[] Teach { get; set; } = [];
    public Exercise[] Practice { get; set; } = [];

    /// <summary>
    /// Same idea, different examples. Served when the practice set says she has not
    /// got it yet — so a second pass is never the identical questions again.
    /// </summary>
    public Exercise[] Reinforce { get; set; } = [];

    /// <summary>How many first-attempt correct answers count as having got it.</summary>
    public int MasteryRequired { get; set; } = 3;
}
