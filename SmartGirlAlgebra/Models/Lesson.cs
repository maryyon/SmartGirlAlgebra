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
/// A picture of the problem: things arranged in equal groups.
///
/// Saying "5 groups of 2" to a seven-year-old and expecting her to hold it in her
/// head is the mistake. She sees the groups, and she sees them appear one at a
/// time as the working goes along.
/// </summary>
public class Picture
{
    /// <summary>How many groups there are in total.</summary>
    public int Groups { get; set; }

    /// <summary>How many things in each group.</summary>
    public int Each { get; set; }

    /// <summary>What the things are. One emoji.</summary>
    public string Icon { get; set; } = "⭐";

    /// <summary>
    /// How many groups are visible at this point in the working. Zero means none
    /// yet; -1 means all of them. The picture fills in as she gets each step.
    /// </summary>
    public int Show { get; set; } = -1;

    /// <summary>Label under the picture, e.g. "1 row". Optional.</summary>
    public string Caption { get; set; } = "";
}

/// <summary>
/// One step of working, which she has to get right before there is a next one.
///
/// There is no skipping and no guessing your way through: a wrong answer offers a
/// hint rather than the answer, and she tries again. The hints get more direct
/// until the last one all but says it — because being stuck is not a lesson.
/// </summary>
public class GuideStep
{
    /// <summary>Typed out slowly on screen so she can watch it being set up.</summary>
    public string Say { get; set; } = "";

    /// <summary>What the picture looks like while this step is being asked.</summary>
    public Picture? Picture { get; set; }

    public string Ask { get; set; } = "";
    public string[] Choices { get; set; } = [];
    public string Correct { get; set; } = "";

    /// <summary>
    /// Offered one at a time, only when she asks. Ordered gentlest first; the last
    /// one is allowed to give it away.
    /// </summary>
    public string[] Hints { get; set; } = [];

    /// <summary>Said once she has it right. Short.</summary>
    public string Yes { get; set; } = "";
}

/// <summary>One problem, worked all the way through, one step at a time.</summary>
public class GuideProblem
{
    /// <summary>The story, typed out before any question is asked.</summary>
    public string Setup { get; set; } = "";

    public GuideStep[] Steps { get; set; } = [];

    /// <summary>The whole thing, said plainly, once she has finished it.</summary>
    public string Recap { get; set; } = "";
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

    /// <summary>
    /// Problems worked through step by step. When a lesson has these, they replace
    /// the tap-an-answer set entirely — she is walked through the working rather
    /// than asked to pick from four and hope.
    /// </summary>
    public GuideProblem[] Walk { get; set; } = [];
}
