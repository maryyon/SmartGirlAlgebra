namespace SmartGirlAlgebra.Models;

public record WalkStep(string Title, string Body, string Math);

public class Level
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
    public string Tagline { get; init; } = "";
    public string Difficulty { get; init; } = "";

    /// <summary>Brightest red at level 1, deepest at level 5.</summary>
    public string Color { get; init; } = "";

    public string Cheer { get; init; } = "";
    public string Question { get; init; } = "";
    public string Equation { get; init; } = "";
    public string VariableMeaning { get; init; } = "";
    public string Answer { get; init; } = "";
    public double AnswerValue { get; init; }

    /// <summary>Symbols offered as tap-targets above the keyboard.</summary>
    public string[] Chips { get; init; } = [];

    public string[] Hints { get; init; } = [];
    public WalkStep[] Walkthrough { get; init; } = [];
}

public static class Levels
{
    private static readonly string[] DefaultChips = ["x", "=", "+", "−", "×", "÷", "(", ")"];

    public static readonly IReadOnlyList<Level> All =
    [
        new Level
        {
            Id = 1,
            Name = "Baby-Step Bootcamp",
            Tagline = "One move and you're out. Easy peasy pom-pom squeezy.",
            Difficulty = "Warm-up",
            Color = "#ff2740",
            Cheer = "You dug around in your glittery cheer bag, found some cash, then Coach handed you $4 more. Now you're clutching $10. 💰",
            Question = "How much money was hiding in the bag?",
            Equation = "x + 4 = 10",
            VariableMeaning = "x = dollars in the bag",
            Answer = "x = 6",
            AnswerValue = 6,
            Chips = ["x", "=", "+", "−"],
            Hints =
            [
                "The x wants to be ALONE. That + 4 is a clingy friend — time to say bye. 🎀",
                "Subtract 4 from BOTH sides. Fair is fair!",
                "10 − 4 = 6. Boom. x = 6. 💅"
            ],
            Walkthrough =
            [
                new WalkStep("Meet the goal 📣", "We want x by itself. Right now it's stuck with a “+ 4.”", "x + 4 = 10"),
                new WalkStep("Ghost the + 4 👻", "Subtract 4 from BOTH sides so it stays balanced. The + 4 vanishes and 10 − 4 = 6.", "x = 6")
            ]
        },

        new Level
        {
            Id = 2,
            Name = "Two-Step Toe Touch",
            Tagline = "Multiply AND add? Double trouble, you got this.",
            Difficulty = "Getting spicy",
            Color = "#df1a30",
            Cheer = "Our squad sold spirit ribbons! Each ribbon is $2, and a sponsor tossed $5 in the jar to start us off. We raised $23 total. 🎀",
            Question = "How many ribbons did the squad sell?",
            Equation = "2x + 5 = 23",
            VariableMeaning = "x = ribbons sold",
            Answer = "x = 9",
            AnswerValue = 9,
            Chips = DefaultChips,
            Hints =
            [
                "Kick out the lonely number first. That + 5 has GOT to go. 📣",
                "Subtract 5 from both sides → you should get 2x = 18.",
                "Now divide both sides by 2 to set x free! 18 ÷ 2 = 9. ✨"
            ],
            Walkthrough =
            [
                new WalkStep("Read the play 📣", "x is squished by a “times 2” and a “plus 5.” Peel them off one at a time.", "2x + 5 = 23"),
                new WalkStep("Clear the + 5 🎀", "Subtract 5 from BOTH sides. 23 − 5 = 18.", "2x = 18"),
                new WalkStep("Split it evenly ✨", "x is times 2, so divide BOTH sides by 2. 18 ÷ 2 = 9.", "x = 9")
            ]
        },

        new Level
        {
            Id = 3,
            Name = "Both-Sides Beyoncé",
            Tagline = "x is on the left AND the right. The drama.",
            Difficulty = "Certified tricky",
            Color = "#b31226",
            Cheer = "Our mascot eats 3 tacos every quarter but dropped 4 in the mud. That somehow equals 1 taco a quarter PLUS the 8 it swiped from the band. 🌮",
            Question = "How many quarters (x) are we even talking about?",
            Equation = "3x − 4 = x + 8",
            VariableMeaning = "x = quarters played",
            Answer = "x = 6",
            AnswerValue = 6,
            Chips = DefaultChips,
            Hints =
            [
                "Get all the x's on ONE side. Herd those cats. 🐈",
                "Subtract x from both sides → 2x − 4 = 8.",
                "Add 4 to both sides → 2x = 12, then divide by 2. x = 6! 💃"
            ],
            Walkthrough =
            [
                new WalkStep("Round up the x's 🐈", "Subtract x from BOTH sides so every x lives on the left.", "2x − 4 = 8"),
                new WalkStep("Free the number 🎀", "Add 4 to BOTH sides. 8 + 4 = 12.", "2x = 12"),
                new WalkStep("Divide and conquer ✨", "Divide BOTH sides by 2. 12 ÷ 2 = 6.", "x = 6")
            ]
        },

        new Level
        {
            Id = 4,
            Name = "Parenthesis Pyramid",
            Tagline = "Distribute like you're handing out snacks.",
            Difficulty = "Big brain",
            Color = "#7d0c1a",
            Cheer = "Coach yells: “Triple every (x minus 1) flyer!” That has to match one x plus the 7 backup cheerleaders on the bench. 📣",
            Question = "How many flyers (x) does each stunt group need?",
            Equation = "3(x − 1) = x + 7",
            VariableMeaning = "x = flyers per group",
            Answer = "x = 5",
            AnswerValue = 5,
            Chips = DefaultChips,
            Hints =
            [
                "First, share the 3 with EVERYONE inside the parentheses. 🍿",
                "3(x − 1) becomes 3x − 3. Now it's 3x − 3 = x + 7.",
                "Get x's together, numbers together → 2x = 10 → x = 5! ✨"
            ],
            Walkthrough =
            [
                new WalkStep("Distribute the 3 🍿", "Multiply the 3 by each thing inside: 3·x and 3·(−1).", "3x − 3 = x + 7"),
                new WalkStep("Round up the x's 🐈", "Subtract x from BOTH sides.", "2x − 3 = 7"),
                new WalkStep("Free the number, then split 🎀", "Add 3 to both sides → 2x = 10, then divide by 2.", "x = 5")
            ]
        },

        new Level
        {
            Id = 5,
            Name = "Pythagoras Goes to Prom",
            Tagline = "Squares, triangles, and one legendary theorem. 🏆",
            Difficulty = "Final boss",
            Color = "#490810",
            Cheer = "We're hanging a giant triangular banner. Its two straight sides (the legs) are 6 ft and 8 ft. We need the slanty side — the hypotenuse — for the glitter fringe. ✨ Pythagoras says: leg² + leg² = hypotenuse².",
            Question = "How long is the slanty side, x?",
            Equation = "x² = 6² + 8²",
            VariableMeaning = "x = hypotenuse (ft)",
            Answer = "x = 10",
            AnswerValue = 10,
            Chips = ["x", "²", "=", "+", "^", "(", ")"],
            Hints =
            [
                "Square the legs first: 6² means 6 × 6, and 8² means 8 × 8. 📐",
                "6² = 36 and 8² = 64. Add them → x² = 100.",
                "What number times itself is 100? That's the square root. x = 10! 🏆"
            ],
            Walkthrough =
            [
                new WalkStep("Meet Pythagoras 📐", "For a right triangle, the two legs squared and added equal the hypotenuse squared: leg² + leg² = hyp².", "x² = 6² + 8²"),
                new WalkStep("Square the legs ✖️", "6² = 6 × 6 = 36 and 8² = 8 × 8 = 64. Add them up.", "x² = 100"),
                new WalkStep("Un-square it (square root) 🏆", "x² = 100, so x is the number that times itself makes 100. That's 10. Glitter fringe secured!", "x = 10")
            ]
        }
    ];

    public static Level? ById(int id) => All.FirstOrDefault(l => l.Id == id);
}
