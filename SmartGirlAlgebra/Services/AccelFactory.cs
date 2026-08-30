using SmartGirlAlgebra.Models;

namespace SmartGirlAlgebra.Services;

/// <summary>
/// The accelerated course: sixth grade in, tenth grade out.
///
/// Built for a quick learner who gets bored before he gets stuck, so it moves
/// fast and it argues back. Most steps carry a WHY, because for this one being
/// right is the easy half — saying why it works is the half worth having.
///
/// Same hard rules as everywhere: generated numbers, typed answers, no choices
/// on the maths itself.
/// </summary>
public static class AccelFactory
{
    public static WorkedProblem Make(int skillId, Random r, Flavor f) => skillId switch
    {
        2101 => Ratios(r, f),
        2102 => UnitRate(r, f),
        2103 => PercentOf(r, f),
        2104 => Negatives(r, f),

        2201 => Distribute(r, f),
        2202 => TwoStep(r, f),
        2203 => Inequality(r, f),
        2204 => Formula(r, f),

        2301 => Proportion(r, f),
        2302 => PercentChange(r, f),
        2303 => Scale(r, f),
        2304 => Probability(r, f),

        2401 => Slope(r, f),
        2402 => LineEquation(r, f),
        2403 => Systems(r, f),
        2404 => FunctionRule(r, f),

        2501 => Exponents(r, f),
        2502 => Scientific(r, f),
        2503 => Roots(r, f),
        2504 => Pythagoras(r, f),

        2601 => Expand(r, f),
        2602 => SolveQuadratic(r, f),
        2603 => Sequence(r, f),
        2604 => Averages(r, f),

        _ => Ratios(r, f)
    };

    private static int P(Random r, int lo, int hi) => r.Next(lo, hi + 1);
    private static int Gcd(int a, int b) { while (b != 0) (a, b) = (b, a % b); return Math.Abs(a); }
    private static string Sg(int n) => n < 0 ? $"({n})" : n.ToString();

    private static TypedStep S(string say, string ask, string answer, string[] hints, string yes,
                               string placeholder = "a number", string why = "",
                               string[]? whyWrong = null, params string[] accept)
        => new()
        {
            Say = say, Ask = ask, Answer = answer, Hints = hints, Yes = yes,
            Placeholder = placeholder, Why = why, WhyWrong = whyWrong ?? [], Accept = accept
        };

    // ============================================================== unit one

    private static WorkedProblem Ratios(Random r, Flavor f)
    {
        int a = P(r, 2, 9), b = P(r, 2, 9), k = P(r, 3, 8);
        var g = Gcd(a, b);

        return new WorkedProblem
        {
            Setup = $"A recipe for {f.Event} uses {a * k} cups of flour to {b * k} cups of stock.",
            Steps =
            [
                S("A ratio is only useful in lowest terms. Find what divides both.",
                  $"What is the biggest number dividing both {a * k} and {b * k}?", k.ToString(),
                  [
                      $"Test the factors of {a * k}.",
                      $"Both divide by {k}.",
                      $"It is {k}."
                  ],
                  "That's the common factor.",
                  "a number",
                  "Dividing both parts by the same number keeps the ratio the same.",
                  ["Ratios can be simplified by subtracting the same from both.",
                   "The bigger number decides what the ratio simplifies to."]),

                S("Divide both parts by it.",
                  $"Write the ratio in simplest form. (like 3:4)", $"{a}:{b}",
                  [
                      $"{a * k} ÷ {k} and {b * k} ÷ {k}.",
                      $"{a} and {b}.",
                      $"{a}:{b}."
                  ],
                  $"{a}:{b} — same recipe, smaller numbers.", "like 3:4",
                  "", null,
                  $"{a} : {b}", $"{a}/{b}"),

                S("Now scale it the other way, which is where ratios earn their keep.",
                  $"If you use {a * 10} cups of flour, how much stock?", (b * 10).ToString(),
                  [
                      $"{a * 10} is {a * 10 / a} times {a}.",
                      $"So the stock is {b} × {a * 10 / a}.",
                      $"It is {b * 10}."
                  ],
                  "Both sides scale together — that's what a ratio means.")
            ],
            Recap = $"{a * k}:{b * k} = {a}:{b}\nScaled up: {a * 10}:{b * 10}\n\nDivide or multiply BOTH parts by the same thing."
        };
    }

    private static WorkedProblem UnitRate(Random r, Flavor f)
    {
        int per = P(r, 4, 25), n = P(r, 3, 12);
        var total = per * n;
        var far = P(r, 15, 40);

        return new WorkedProblem
        {
            Setup = $"On the road trip you covered {total} miles on {n} gallons.",
            Steps =
            [
                S("A unit rate is per ONE. Divide by however many you had.",
                  $"What is {total} ÷ {n}?", per.ToString(),
                  [
                      $"{total} shared over {n} gallons.",
                      $"{n} × ? = {total}.",
                      $"{per} miles per gallon."
                  ],
                  $"{per} miles a gallon.",
                  "a number",
                  "Dividing by the number of units gives the amount for exactly one.",
                  ["Multiplying gives the rate; dividing gives the total.",
                   "The unit rate is always the larger of the two numbers."]),

                S("Rates predict. That is the whole point of them.",
                  $"How far on {far} gallons?", (per * far).ToString(),
                  [
                      $"{per} miles each, {far} gallons.",
                      $"{per} × {far}.",
                      $"{per * far} miles."
                  ],
                  "Rate times amount."),

                S("And backwards, which is the direction people get wrong.",
                  $"How many gallons for {per * 30} miles?", "30",
                  [
                      $"Divide by the rate: {per * 30} ÷ {per}.",
                      "Total ÷ rate = how many.",
                      "30 gallons."
                  ],
                  "Both directions — that's fluency.")
            ],
            Recap = $"{total} ÷ {n} = {per} mpg.\nrate × gallons = miles, and miles ÷ rate = gallons."
        };
    }

    private static WorkedProblem PercentOf(Random r, Flavor f)
    {
        int pct = new[] { 12, 15, 18, 24, 35, 45, 60, 72 }[r.Next(8)];
        int whole = P(r, 5, 40) * 20;
        var part = whole * pct / 100;

        return new WorkedProblem
        {
            Setup = $"{whole} people RSVP'd to the family reunion.\n{pct}% asked for the vegetarian plate.",
            Steps =
            [
                S("Find one per cent first. It makes every other percentage easy.",
                  $"What is 1% of {whole}?", (whole / 100.0).ToString("0.##"),
                  [
                      "One per cent is one hundredth.",
                      $"{whole} ÷ 100.",
                      $"It is {whole / 100.0:0.##}."
                  ],
                  "That's your building block.", "a number",
                  "Per cent means per hundred, so 1% is always the whole divided by 100.",
                  ["1% is the whole divided by 10.",
                   "1% is found by subtracting 100 from the whole."]),

                S($"Now scale it up to {pct}%.",
                  $"What is {pct}% of {whole}?", part.ToString(),
                  [
                      $"{whole / 100.0:0.##} × {pct}.",
                      $"Or {whole} × 0.{pct:00}.",
                      $"It is {part}."
                  ],
                  $"{part} vegetarian plates."),

                S("And the ones you still have to cook for.",
                  $"How many are NOT vegetarian?", (whole - part).ToString(),
                  [
                      $"{whole} − {part}.",
                      "Whole minus part.",
                      $"{whole - part}."
                  ],
                  "Both plates accounted for.")
            ],
            Recap = $"1% of {whole} = {whole / 100.0:0.##}, so {pct}% = {part}.\nThe rest is {whole - part}."
        };
    }

    private static WorkedProblem Negatives(Random r, Flavor f)
    {
        int a = P(r, -15, -3), b = P(r, -12, -2);

        return new WorkedProblem
        {
            Setup = $"Two plays lost {Math.Abs(a)} and {Math.Abs(b)} yards.\n\nWork out {a} × {b}",
            Steps =
            [
                S("Multiply the numbers first, and worry about the sign after.",
                  $"What is {Math.Abs(a)} × {Math.Abs(b)}?", (Math.Abs(a) * Math.Abs(b)).ToString(),
                  [
                      "Ignore the minus signs for a second.",
                      $"{Math.Abs(a)} × {Math.Abs(b)}.",
                      $"It is {Math.Abs(a) * Math.Abs(b)}."
                  ],
                  "Now the sign."),

                S("Two negatives multiplied.",
                  $"What is {a} × {b}?", (a * b).ToString(),
                  [
                      "A negative times a negative is positive.",
                      "The signs cancel each other out.",
                      $"{a} × {b} = {a * b}."
                  ],
                  "Positive. Signs cancelled.", "a number",
                  "Multiplying by a negative flips the sign, so doing it twice flips it back.",
                  ["Two negatives make a bigger negative.",
                   "The sign always follows the larger number."]),

                S("Now the one people actually get wrong.",
                  $"What is {a} + {b}?", (a + b).ToString(),
                  [
                      "This is ADDING, not multiplying. The rule is different.",
                      $"Two debts added together make a bigger debt.",
                      $"{a} + {b} = {a + b}."
                  ],
                  "Adding and multiplying follow different rules — that's the trap.")
            ],
            Recap = $"{a} × {b} = {a * b}  (signs cancel)\n{a} + {b} = {a + b}  (debts stack)\n\nDifferent operations, different rules."
        };
    }

    // ============================================================== unit two

    private static WorkedProblem Distribute(Random r, Flavor f)
    {
        int a = P(r, 2, 8), b = P(r, 2, 9), c = P(r, 2, 9);

        return new WorkedProblem
        {
            Setup = $"Simplify:\n\n{a}({b}x + {c})",
            Steps =
            [
                S("The number outside multiplies EVERY term inside. Not just the first.",
                  $"What is {a} × {b}? (the x term's number)", (a * b).ToString(),
                  [
                      $"{a} × {b}.",
                      "That is the coefficient of x.",
                      $"{a * b}."
                  ],
                  $"So {a * b}x.", "a number",
                  "Multiplication distributes over addition — each term inside gets multiplied.",
                  ["Only the first term inside the bracket gets multiplied.",
                   "The bracket means you add before multiplying, always."]),

                S("Now the other term.",
                  $"What is {a} × {c}?", (a * c).ToString(),
                  [
                      $"{a} × {c}.",
                      "The constant inside also gets hit.",
                      $"{a * c}."
                  ],
                  "Both terms multiplied."),

                S("Write it out.",
                  "What does it simplify to? (like 6x + 8)", $"{a * b}x+{a * c}",
                  [
                      $"You have {a * b}x and {a * c}.",
                      "x term first.",
                      $"{a * b}x + {a * c}."
                  ],
                  "Expanded.", "like 6x + 8", "", null,
                  $"{a * b}x + {a * c}")
            ],
            Recap = $"{a}({b}x + {c}) = {a * b}x + {a * c}\n\nEverything inside gets multiplied."
        };
    }

    private static WorkedProblem TwoStep(Random r, Flavor f)
    {
        int m = P(r, 3, 9), x = P(r, 3, 15), b = P(r, 4, 30);
        var total = m * x + b;

        return new WorkedProblem
        {
            Setup = $"You cook {m} portions per tray plus {b} made ahead. You need {total}.\n\n{m}x + {b} = {total}",
            Steps =
            [
                S("Undo in reverse order to how it was built. Adding came last, so it goes first.",
                  $"What is {total} − {b}?", (m * x).ToString(),
                  [
                      $"Take {b} off both sides.",
                      $"{total} − {b}.",
                      $"{m}x = {m * x}."
                  ],
                  $"Now {m}x = {m * x}.", "a number",
                  "You undo operations in the opposite order they were applied.",
                  ["You always divide before subtracting in an equation.",
                   "The order you undo things in makes no difference."]),

                S("Now the multiplying.",
                  $"Divide by {m}. What is x?", x.ToString(),
                  [
                      $"{m * x} ÷ {m}.",
                      $"{m} × ? = {m * x}.",
                      $"x = {x}."
                  ],
                  $"x = {x}."),

                S("Check it, every time.",
                  $"What is {m} × {x} + {b}?", total.ToString(),
                  [
                      $"{m} × {x} = {m * x}.",
                      $"Plus {b}.",
                      $"{total} — back where you started."
                  ],
                  "Checked.")
            ],
            Recap = $"{m}x + {b} = {total}\n  −{b}:  {m}x = {m * x}\n  ÷{m}:  x = {x}\n  check ✓"
        };
    }

    private static WorkedProblem Inequality(Random r, Flavor f)
    {
        int m = P(r, 2, 6), x = P(r, 3, 12), b = P(r, 3, 18);
        var rhs = m * x + b;

        return new WorkedProblem
        {
            Setup = $"Tickets cost ${m} each plus a ${b} booking fee. You have ${rhs} at most.\n\n{m}x + {b} ≤ {rhs}",
            Steps =
            [
                S("Solve it exactly like an equation, until a negative shows up.",
                  $"Take {b} off both sides. What is {rhs} − {b}?", (m * x).ToString(),
                  [
                      $"{rhs} − {b}.",
                      "Nothing unusual yet.",
                      $"{m}x ≤ {m * x}."
                  ],
                  "Same as an equation so far."),

                S($"Now divide by {m}. It is POSITIVE, so nothing flips.",
                  "What is x, at most?", x.ToString(),
                  [
                      $"{m * x} ÷ {m}.",
                      "Dividing by a positive leaves the sign alone.",
                      $"x ≤ {x}."
                  ],
                  $"x ≤ {x}.", "a number",
                  "Dividing by a positive keeps the order of the two sides the same.",
                  ["Inequality signs always flip when you divide.",
                   "Inequalities never flip under any operation."]),

                S("Now the part everyone forgets. Suppose you had divided by a NEGATIVE.",
                  "What happens to the ≤ sign then? (flips or stays)", "flips",
                  [
                      "Try it: 2 < 4. Divide both by −1.",
                      "−2 is GREATER than −4, so the order reversed.",
                      "It flips."
                  ],
                  "That is the one rule to never forget.", "flips or stays",
                  "Multiplying or dividing by a negative reverses the order of every number line.",
                  ["The sign flips whenever you divide by anything.",
                   "The sign flips only when you subtract."],
                  "it flips", "flip", "reverses", "it reverses")
            ],
            Recap = $"{m}x + {b} ≤ {rhs}  →  x ≤ {x}\n\nDivide by a positive: sign stays. By a negative: sign flips."
        };
    }

    private static WorkedProblem Formula(Random r, Flavor f)
    {
        int rate = P(r, 40, 70), hours = P(r, 2, 9);
        var dist = rate * hours;

        return new WorkedProblem
        {
            Setup = $"Distance = speed × time, and you drove {dist} miles in {hours} hours.\n\nd = st",
            Steps =
            [
                S("Rearrange the formula BEFORE putting numbers in. It is cleaner and it is reusable.",
                  "Make s the subject. What does s equal? (like d/t)", "d/t",
                  [
                      "s is multiplied by t.",
                      "Divide both sides by t.",
                      "s = d/t."
                  ],
                  "Rearranged.", "like d/t",
                  "Dividing both sides by the same thing keeps the equation true.",
                  ["You can move a letter across the equals and it keeps its operation.",
                   "Letters can be cancelled whenever they appear twice."]),

                S("Now put the numbers in.",
                  $"What is {dist} ÷ {hours}?", rate.ToString(),
                  [
                      $"{dist} ÷ {hours}.",
                      $"{hours} × ? = {dist}.",
                      $"{rate} mph."
                  ],
                  $"{rate} mph."),

                S("Same formula, different unknown.",
                  $"At that speed, how long for {rate * 12} miles?", "12",
                  [
                      "t = d/s this time.",
                      $"{rate * 12} ÷ {rate}.",
                      "12 hours."
                  ],
                  "One formula, three questions.")
            ],
            Recap = $"d = st  →  s = d/t = {dist}/{hours} = {rate} mph\nand t = d/s."
        };
    }

    // ============================================================ unit three

    private static WorkedProblem Proportion(Random r, Flavor f)
    {
        int a = P(r, 2, 9), b = P(r, 3, 12), k = P(r, 3, 9);

        return new WorkedProblem
        {
            Setup = $"The recipe serves {a} and needs {b} cups.\nYou are cooking for {a * k}.",
            Steps =
            [
                S("Set it up as two equal ratios. Same thing on top on both sides.",
                  $"{a * k} is how many times {a}?", k.ToString(),
                  [
                      $"{a * k} ÷ {a}.",
                      "That is the scale factor.",
                      $"{k} times."
                  ],
                  $"Everything scales by {k}.", "a number",
                  "Both parts of a ratio must be multiplied by the same factor to stay equivalent.",
                  ["Only the part you know needs scaling.",
                   "Adding the difference to both parts keeps a ratio equal."]),

                S("So scale the other side by the same amount.",
                  $"How many cups for {a * k} people?", (b * k).ToString(),
                  [
                      $"{b} × {k}.",
                      "Same factor as the people.",
                      $"{b * k} cups."
                  ],
                  $"{b * k} cups."),

                S("Sense check — always.",
                  $"You scaled people by {k}. Did the cups scale by the same {k}? (yes or no)", "yes",
                  [
                      $"{b} became {b * k}.",
                      $"{b * k} ÷ {b} = {k}.",
                      "Yes — same factor."
                  ],
                  "That check catches most proportion mistakes.", "yes or no")
            ],
            Recap = $"{a}/{b} = {a * k}/{b * k}\nScale factor {k} applied to both."
        };
    }

    private static WorkedProblem PercentChange(Random r, Flavor f)
    {
        int start = P(r, 4, 25) * 20;
        int pct = new[] { 10, 20, 25, 50 }[r.Next(4)];
        var change = start * pct / 100;
        var end = start + change;

        return new WorkedProblem
        {
            Setup = $"Ticket sales for {f.Event} went from {start} to {end}.",
            Steps =
            [
                S("Percent change is always measured against where you STARTED.",
                  $"What is the actual increase, {end} − {start}?", change.ToString(),
                  [
                      $"{end} − {start}.",
                      "The raw change first.",
                      $"{change}."
                  ],
                  "That's the change."),

                S("Now compare it to the starting amount, not the finishing one.",
                  $"What is {change} ÷ {start}, as a decimal?", (pct / 100.0).ToString("0.##"),
                  [
                      $"{change} ÷ {start}.",
                      "Change over ORIGINAL.",
                      $"{pct / 100.0:0.##}."
                  ],
                  "Nearly there.", "a decimal",
                  "Percent change compares the change to the original amount, not the new one.",
                  ["Percent change compares the change to the final amount.",
                   "Percent change is the difference between the two numbers."]),

                S("Turn it into a percentage.",
                  $"What percent did sales rise? (just the number)", pct.ToString(),
                  [
                      $"{pct / 100.0:0.##} × 100.",
                      "Multiply the decimal by 100.",
                      $"{pct}%."
                  ],
                  $"Up {pct}%.")
            ],
            Recap = $"({end} − {start})/{start} = {change}/{start} = {pct / 100.0:0.##} = {pct}%\n\nAlways divide by where you started."
        };
    }

    private static WorkedProblem Scale(Random r, Flavor f)
    {
        int scale = new[] { 20, 25, 50, 100 }[r.Next(4)];
        int onMap = P(r, 3, 12);
        var real = onMap * scale;

        return new WorkedProblem
        {
            Setup = $"On the theme-park map, 1 inch = {scale} feet.\nThe walk to the coaster measures {onMap} inches.",
            Steps =
            [
                S("A scale is just a rate in disguise.",
                  $"How many real feet is the walk?", real.ToString(),
                  [
                      $"{onMap} inches, {scale} feet each.",
                      $"{onMap} × {scale}.",
                      $"{real} feet."
                  ],
                  $"{real} feet.", "a number",
                  "A scale is a fixed ratio, so map length times the scale gives real length.",
                  ["You divide by the scale to go from map to real.",
                   "Scale only works for areas, not lengths."]),

                S("Now backwards.",
                  $"A ride is {scale * 7} feet away. How many inches on the map?", "7",
                  [
                      $"Divide by the scale: {scale * 7} ÷ {scale}.",
                      "Real ÷ scale = map.",
                      "7 inches."
                  ],
                  "Both directions."),

                S("The trap question.",
                  $"If 1 inch = {scale} feet, does 2 inches = {scale * 2} feet? (yes or no)", "yes",
                  [
                      "Scales are proportional.",
                      "Double the map distance, double the real one.",
                      "Yes."
                  ],
                  "Lengths scale directly. Areas do not — that's the next trap.", "yes or no")
            ],
            Recap = $"{onMap} in × {scale} ft/in = {real} ft\nand {scale * 7} ft ÷ {scale} = 7 in."
        };
    }

    private static WorkedProblem Probability(Random r, Flavor f)
    {
        int good = P(r, 2, 8), bad = P(r, 2, 9);
        var total = good + bad;
        var g = Gcd(good, total);

        return new WorkedProblem
        {
            Setup = $"A cooler holds {good} sports drinks and {bad} waters.\nYou grab one without looking.",
            Steps =
            [
                S("Probability is what you want over everything there is.",
                  $"How many drinks are in the cooler altogether?", total.ToString(),
                  [
                      $"{good} + {bad}.",
                      "Everything counts, not just what you want.",
                      $"{total}."
                  ],
                  "That's the denominator."),

                S("Now the chance of a sports drink.",
                  $"Write it as a fraction in simplest form. (like 3/4)", $"{good / g}/{total / g}",
                  [
                      $"{good} out of {total}.",
                      g > 1 ? $"Both divide by {g}." : "Check whether it simplifies — this one does not.",
                      $"{good / g}/{total / g}."
                  ],
                  "Wanted over total.", "a fraction like 3/4",
                  "Probability is favourable outcomes divided by all equally likely outcomes.",
                  ["Probability is wanted divided by unwanted.",
                   "Probability compares the two groups to each other."],
                  $"{good}/{total}"),

                S("The two chances must account for everything.",
                  $"The chance of water is {bad}/{total}. What do the two probabilities add to?", "1",
                  [
                      $"{good}/{total} + {bad}/{total}.",
                      $"{good} + {bad} = {total}, so it is {total}/{total}.",
                      "It is 1."
                  ],
                  "Everything that can happen adds to 1. Always.")
            ],
            Recap = $"P(sports) = {good}/{total} = {good / g}/{total / g}\nP(water) = {bad}/{total}\nThey add to 1."
        };
    }

    // ============================================================= unit four

    private static WorkedProblem Slope(Random r, Flavor f)
    {
        int x1 = P(r, 0, 5), y1 = P(r, 0, 20), run = P(r, 2, 5), m = P(r, 2, 9);
        int x2 = x1 + run, y2 = y1 + m * run;

        return new WorkedProblem
        {
            Setup = $"After game {x1} he had {y1} catches; after game {x2}, {y2}.\nHis rate is steady.",
            Steps =
            [
                S("Slope is rise over run — change in the up direction over change in the across direction.",
                  $"What is the rise, {y2} − {y1}?", (y2 - y1).ToString(),
                  [
                      $"{y2} − {y1}.",
                      "The vertical change.",
                      $"{y2 - y1}."
                  ],
                  "Rise found."),

                S("Now the run, in the same order.",
                  $"What is {x2} − {x1}?", run.ToString(),
                  [
                      $"{x2} − {x1}.",
                      "Same order, or the sign comes out wrong.",
                      $"{run}."
                  ],
                  "Run found."),

                S("Divide.",
                  "What is the slope — catches per game?", m.ToString(),
                  [
                      $"{y2 - y1} ÷ {run}.",
                      "Rise over run.",
                      $"{m}."
                  ],
                  $"{m} catches a game.", "a number",
                  "Slope is a rate of change: how much y moves for every 1 that x moves.",
                  ["Slope is the total of the y values divided by the x values.",
                   "Slope is how far the line is from the origin."])
            ],
            Recap = $"m = ({y2} − {y1})/({x2} − {x1}) = {y2 - y1}/{run} = {m} per game."
        };
    }

    private static WorkedProblem LineEquation(Random r, Flavor f)
    {
        int m = P(r, 2, 9), b = P(r, 5, 40), x = P(r, 2, 10);

        return new WorkedProblem
        {
            Setup = $"Catering costs ${b} to set up plus ${m} a head.\n\nWrite and use the rule.",
            Steps =
            [
                S("The fixed part never changes; the other part depends on how many.",
                  $"Which number is the y-intercept — the cost before anyone eats?", b.ToString(),
                  [
                      "It is the part that happens even at zero guests.",
                      "The setup fee.",
                      $"{b}."
                  ],
                  "That's b.", "a number",
                  "The y-intercept is the value when x is zero — the part that does not depend on x.",
                  ["The y-intercept is always the largest number in the equation.",
                   "The y-intercept is the number multiplied by x."]),

                S("And the part that grows.",
                  $"What is the slope — the cost per guest?", m.ToString(),
                  [
                      "The bit that depends on how many guests.",
                      "Per head.",
                      $"{m}."
                  ],
                  "That's m."),

                S($"So the rule is y = {m}x + {b}.",
                  $"What does {x} guests cost?", (m * x + b).ToString(),
                  [
                      $"{m} × {x} = {m * x}.",
                      $"Plus the {b} setup.",
                      $"${m * x + b}."
                  ],
                  $"${m * x + b}.")
            ],
            Recap = $"y = {m}x + {b}\nAt x = {x}: y = {m * x} + {b} = {m * x + b}."
        };
    }

    private static WorkedProblem Systems(Random r, Flavor f)
    {
        int x = P(r, 2, 9), y = P(r, 2, 9);
        int a = P(r, 2, 4), c = P(r, 2, 4);
        int s1 = x + y, s2 = a * x + c * y;

        return new WorkedProblem
        {
            Setup = $"{s1} dishes were brought to the reunion.\nThe {a}-serving trays and {c}-serving trays fed {s2}.\n\nx + y = {s1}\n{a}x + {c}y = {s2}",
            Steps =
            [
                S("The first equation is easy to rearrange. Do that, then substitute.",
                  $"From x + y = {s1}, what does y equal? (like {s1} - x)", $"{s1}-x",
                  [
                      "Take x off both sides.",
                      $"y = {s1} − x.",
                      $"{s1} - x."
                  ],
                  "Now put that into the other equation.", $"like {s1} - x",
                  "Substitution replaces a letter with something equal to it, so nothing changes value.",
                  ["Substitution works by guessing values until both equations fit.",
                   "You can only substitute when both equations have the same numbers."],
                  $"{s1} - x", $"{s1}−x"),

                S($"Substituting: {a}x + {c}({s1} − x) = {s2}. Expand and collect.",
                  $"What is {c} × {s1}?", (c * s1).ToString(),
                  [
                      $"{c} × {s1}.",
                      "Multiply out the bracket.",
                      $"{c * s1}."
                  ],
                  $"So {a}x + {c * s1} − {c}x = {s2}."),

                S($"Collect the x terms: {a}x − {c}x.",
                  "What is x?", x.ToString(),
                  [
                      $"{a - c}x = {s2} − {c * s1} = {s2 - c * s1}.",
                      $"Divide by {a - c}.",
                      $"x = {x}."
                  ],
                  $"x = {x}."),

                S("Now back-substitute.",
                  "What is y?", y.ToString(),
                  [
                      $"y = {s1} − x.",
                      $"{s1} − {x}.",
                      $"y = {y}."
                  ],
                  $"({x}, {y}).")
            ],
            Recap = $"x + y = {s1}, {a}x + {c}y = {s2}\n → x = {x}, y = {y}"
        };
    }

    private static WorkedProblem FunctionRule(Random r, Flavor f)
    {
        int m = P(r, 2, 7), b = P(r, 1, 12), t = P(r, 5, 12);

        return new WorkedProblem
        {
            Setup = $"A function machine: put in 1, get {m + b}. Put in 2, get {2 * m + b}. Put in 3, get {3 * m + b}.",
            Steps =
            [
                S("Look at what happens between one output and the next.",
                  $"What does the output go up by each time?", m.ToString(),
                  [
                      $"{2 * m + b} − {m + b}.",
                      "The constant difference.",
                      $"{m}."
                  ],
                  "That's the multiplier.", "a number",
                  "A constant difference between outputs means the rule multiplies the input by that amount.",
                  ["A constant difference means the rule adds that number to itself.",
                   "The difference between outputs is always the starting value."]),

                S($"So the rule multiplies by {m}. Now find what gets added.",
                  $"If input 1 gives {m + b}, and {m} × 1 = {m}, what is added?", b.ToString(),
                  [
                      $"{m + b} − {m}.",
                      "The leftover after the multiplying.",
                      $"{b}."
                  ],
                  $"So the rule is {m}x + {b}."),

                S("Test it on a number you have not been given.",
                  $"What comes out if you put in {t}?", (m * t + b).ToString(),
                  [
                      $"{m} × {t} = {m * t}.",
                      $"Plus {b}.",
                      $"{m * t + b}."
                  ],
                  "Rule found and tested.")
            ],
            Recap = $"Rule: {m}x + {b}\nInput {t} → {m * t + b}."
        };
    }

    // ============================================================= unit five

    private static WorkedProblem Exponents(Random r, Flavor f)
    {
        int b = P(r, 2, 6), p = P(r, 3, 6), q = P(r, 2, 3);

        return new WorkedProblem
        {
            Setup = $"Simplify:\n\n({b}^{p}) ÷ ({b}^{q})",
            Steps =
            [
                S($"Write it out: {p} of them on top, {q} on the bottom. They cancel in pairs.",
                  $"How many {b}s are left after cancelling?", (p - q).ToString(),
                  [
                      $"{p} on top, {q} on the bottom.",
                      $"{p} − {q}.",
                      $"{p - q} left."
                  ],
                  "So dividing SUBTRACTS the powers.", "a number",
                  "Each factor on the bottom cancels one on top, so you subtract the exponents.",
                  ["Dividing powers means dividing the exponents.",
                   "Dividing powers means you subtract the bases."]),

                S("Write the single power.",
                  $"What is ({b}^{p}) ÷ ({b}^{q})? (like 2^3)", $"{b}^{p - q}",
                  [
                      "Same base, subtract the powers.",
                      $"{p} − {q} = {p - q}.",
                      $"{b}^{p - q}."
                  ],
                  "Subtracted.", "like 2^3"),

                S("And the number, to prove it is not magic.",
                  $"What is {b}^{p - q} as an ordinary number?", ((int)Math.Pow(b, p - q)).ToString(),
                  [
                      $"{b} multiplied {p - q} times.",
                      $"{b}^{p - q}.",
                      $"{(int)Math.Pow(b, p - q)}."
                  ],
                  "Same answer either way.")
            ],
            Recap = $"{b}^{p} ÷ {b}^{q} = {b}^{p - q} = {(int)Math.Pow(b, p - q)}\n\nMultiply → add powers. Divide → subtract."
        };
    }

    private static WorkedProblem Scientific(Random r, Flavor f)
    {
        int lead = P(r, 1, 9), dec = P(r, 1, 99), power = P(r, 4, 7);
        var mant = lead + dec / 100.0;
        var value = mant * Math.Pow(10, power);

        return new WorkedProblem
        {
            Setup = $"The stadium deal is worth {value:N0} dollars.",
            Steps =
            [
                S("Scientific notation: exactly one non-zero digit before the point.",
                  "What is the number part? (like 4.25)", mant.ToString("0.##"),
                  [
                      "Move the point until one digit is in front of it.",
                      $"From {value:N0}.",
                      $"{mant:0.##}."
                  ],
                  "One digit, then the rest.", "like 4.25"),

                S("Count the moves.",
                  "How many places did the point move?", power.ToString(),
                  [
                      "Count the jumps left.",
                      $"From {value:N0} to {mant:0.##}.",
                      $"{power}."
                  ],
                  $"{power} places.", "a number",
                  "Each place the point moves left is one factor of ten taken out of the number.",
                  ["The exponent counts the digits in the original number.",
                   "The exponent is always one less than the number of zeros."]),

                S("Assemble it.",
                  "Write it in scientific notation. (like 4.25 x 10^6)", $"{mant:0.##}x10^{power}",
                  [
                      $"Number part {mant:0.##}.",
                      $"Power {power}.",
                      $"{mant:0.##} × 10^{power}."
                  ],
                  "Standard form.", "like 4.25 x 10^6", "", null,
                  $"{mant:0.##} x 10^{power}", $"{mant:0.##}*10^{power}", $"{mant:0.##} × 10^{power}")
            ],
            Recap = $"{value:N0} = {mant:0.##} × 10^{power}"
        };
    }

    private static WorkedProblem Roots(Random r, Flavor f)
    {
        int n = P(r, 4, 15);
        var sq = n * n;
        int k = P(r, 2, 5);
        var inside = sq * k * k;

        return new WorkedProblem
        {
            Setup = $"Work out √{sq}, then √{inside}.",
            Steps =
            [
                S("A square root asks: what times itself makes this?",
                  $"What is √{sq}?", n.ToString(),
                  [
                      $"What number squared is {sq}?",
                      $"Try {n}.",
                      $"√{sq} = {n}."
                  ],
                  $"{n}.", "a number",
                  "A square root undoes squaring, so it asks which number multiplied by itself gives this.",
                  ["A square root is the number divided by two.",
                   "A square root is the number of digits in the value."]),

                S($"Now a bigger one. {inside} = {sq} × {k * k}, and roots split across multiplication.",
                  $"What is √{k * k}?", k.ToString(),
                  [
                      $"What squared is {k * k}?",
                      $"{k} × {k} = {k * k}.",
                      $"{k}."
                  ],
                  "Now put the pieces together."),

                S("Multiply the two roots.",
                  $"What is √{inside}?", (n * k).ToString(),
                  [
                      $"√{sq} × √{k * k}.",
                      $"{n} × {k}.",
                      $"{n * k}."
                  ],
                  "Roots split across multiplying — that's the useful bit.")
            ],
            Recap = $"√{sq} = {n}\n√{inside} = √{sq} × √{k * k} = {n} × {k} = {n * k}"
        };
    }

    private static WorkedProblem Pythagoras(Random r, Flavor f)
    {
        (int a, int b, int c)[] t = [(3, 4, 5), (6, 8, 10), (5, 12, 13), (9, 12, 15), (8, 15, 17), (20, 21, 29)];
        var tr = t[r.Next(t.Length)];

        return new WorkedProblem
        {
            Setup = $"A receiver runs {tr.a} yards downfield and {tr.b} yards across.\n\nHow far is the throw?",
            Steps =
            [
                S("Right angle, so a² + b² = c². Square both short sides.",
                  $"What is {tr.a}² + {tr.b}²?", (tr.c * tr.c).ToString(),
                  [
                      $"{tr.a}² = {tr.a * tr.a} and {tr.b}² = {tr.b * tr.b}.",
                      $"{tr.a * tr.a} + {tr.b * tr.b}.",
                      $"{tr.c * tr.c}."
                  ],
                  "That is c², not c.", "a number",
                  "The theorem relates the squares of the sides, so it gives you c squared, not c.",
                  ["The theorem gives the longest side directly.",
                   "You add the sides and then square the total."]),

                S("Now undo the square.",
                  $"What is √{tr.c * tr.c}?", tr.c.ToString(),
                  [
                      $"What squared is {tr.c * tr.c}?",
                      $"Try {tr.c}.",
                      $"{tr.c} yards."
                  ],
                  $"{tr.c} yards."),

                S("The classic trap.",
                  $"Is the throw {tr.a + tr.b} yards — the two runs added? (yes or no)", "no",
                  [
                      "The throw is a straight line, not the path he ran.",
                      $"{tr.c} is less than {tr.a + tr.b}.",
                      "No — the hypotenuse is always shorter than the two legs added."
                  ],
                  "Straight line beats running two sides. Always.", "yes or no")
            ],
            Recap = $"{tr.a}² + {tr.b}² = {tr.c * tr.c}, so c = {tr.c} yards.\nShorter than running {tr.a + tr.b}."
        };
    }

    // ============================================================== unit six

    private static WorkedProblem Expand(Random r, Flavor f)
    {
        int a = P(r, 1, 9), b = P(r, 1, 9);

        return new WorkedProblem
        {
            Setup = $"Expand:\n\n(x + {a})(x + {b})",
            Steps =
            [
                S("Every term in the first bracket meets every term in the second. Four products.",
                  $"The two middle terms are {b}x and {a}x. What do they add to? (just the number)",
                  (a + b).ToString(),
                  [
                      $"{b} + {a}.",
                      "Like terms combine.",
                      $"{a + b}x."
                  ],
                  $"{a + b}x.", "a number",
                  "Both brackets contribute one x term, and like terms add.",
                  ["The middle term is the product of the two numbers.",
                   "The middle term comes from squaring x."]),

                S("And the constant.",
                  $"What is {a} × {b}?", (a * b).ToString(),
                  [
                      $"{a} × {b}.",
                      "Last times last.",
                      $"{a * b}."
                  ],
                  "Good."),

                S("Write it out.",
                  "What does it expand to? (like x^2 + 5x + 6)", $"x^2+{a + b}x+{a * b}",
                  [
                      $"x², then {a + b}x, then {a * b}.",
                      "Highest power first.",
                      $"x² + {a + b}x + {a * b}."
                  ],
                  "Expanded.", "like x^2 + 5x + 6", "", null,
                  $"x^2 + {a + b}x + {a * b}", $"x² + {a + b}x + {a * b}")
            ],
            Recap = $"(x + {a})(x + {b}) = x² + {a + b}x + {a * b}\nMiddle is the SUM, end is the PRODUCT."
        };
    }

    private static WorkedProblem SolveQuadratic(Random r, Flavor f)
    {
        int a = P(r, 1, 8), b = P(r, 1, 8);

        return new WorkedProblem
        {
            Setup = $"Solve:\n\nx² + {a + b}x + {a * b} = 0",
            Steps =
            [
                S("Factorise. Two numbers multiplying to the constant and adding to the middle.",
                  $"Which two numbers multiply to {a * b} and add to {a + b}? Give the smaller.",
                  Math.Min(a, b).ToString(),
                  [
                      $"Factor pairs of {a * b}.",
                      $"{Math.Min(a, b)} and {Math.Max(a, b)}.",
                      $"{Math.Min(a, b)}."
                  ],
                  $"So (x + {Math.Min(a, b)})(x + {Math.Max(a, b)}) = 0."),

                S("Two things multiplying to zero means one of them IS zero.",
                  $"From (x + {Math.Min(a, b)}) = 0, what is x?", (-Math.Min(a, b)).ToString(),
                  [
                      $"x = −{Math.Min(a, b)}.",
                      "Take it off both sides.",
                      $"{-Math.Min(a, b)}."
                  ],
                  "One root.", "a number",
                  "If a product is zero then at least one factor must be zero.",
                  ["A quadratic always has exactly one solution.",
                   "You can divide both sides by x to solve it."]),

                S("And the other.",
                  $"From (x + {Math.Max(a, b)}) = 0, what is x?", (-Math.Max(a, b)).ToString(),
                  [
                      $"x = −{Math.Max(a, b)}.",
                      "Same move.",
                      $"{-Math.Max(a, b)}."
                  ],
                  $"Two roots: {-Math.Min(a, b)} and {-Math.Max(a, b)}.")
            ],
            Recap = $"x² + {a + b}x + {a * b} = (x + {Math.Min(a, b)})(x + {Math.Max(a, b)}) = 0\nx = {-Math.Min(a, b)} or {-Math.Max(a, b)}"
        };
    }

    private static WorkedProblem Sequence(Random r, Flavor f)
    {
        int first = P(r, 2, 15), d = P(r, 2, 9), n = P(r, 8, 20);
        var nth = first + d * (n - 1);

        return new WorkedProblem
        {
            Setup = $"Week 1 he ran {first} miles, then {first + d}, then {first + 2 * d}, then {first + 3 * d}.",
            Steps =
            [
                S("Find the step between terms first. It has to be the same every time.",
                  "What is the common difference?", d.ToString(),
                  [
                      $"{first + d} − {first}.",
                      "Check it holds for the next pair too.",
                      $"{d}."
                  ],
                  "Constant step — so it's arithmetic.", "a number",
                  "A constant difference means each term adds the same amount to the one before.",
                  ["A constant difference means each term is multiplied by the same amount.",
                   "The common difference is the first term."]),

                S($"The rule is first + d × (n − 1). The −1 matters: week 1 has had NO steps yet.",
                  $"How many steps have happened by week {n}?", (n - 1).ToString(),
                  [
                      "Week 1 is the start, before any step.",
                      $"{n} − 1.",
                      $"{n - 1}."
                  ],
                  "That −1 is where most people go wrong."),

                S("Now finish it.",
                  $"How many miles in week {n}?", nth.ToString(),
                  [
                      $"{first} + {d} × {n - 1}.",
                      $"{d} × {n - 1} = {d * (n - 1)}.",
                      $"{first} + {d * (n - 1)} = {nth}."
                  ],
                  $"{nth} miles.")
            ],
            Recap = $"aₙ = {first} + {d}(n − 1)\nWeek {n}: {first} + {d * (n - 1)} = {nth} miles."
        };
    }

    private static WorkedProblem Averages(Random r, Flavor f)
    {
        var vals = new int[5];
        for (var i = 0; i < 5; i++) vals[i] = P(r, 4, 40);
        Array.Sort(vals);
        var sum = vals.Sum();
        var mean = sum / 5.0;

        return new WorkedProblem
        {
            Setup = $"Five games, yards gained: {string.Join(", ", vals)}.",
            Steps =
            [
                S("Mean is the total shared out equally.",
                  "What do the five add to?", sum.ToString(),
                  [
                      string.Join(" + ", vals) + ".",
                      "Add them in pairs if it helps.",
                      $"{sum}."
                  ],
                  "Total first."),

                S("Now share it.",
                  $"What is the mean, {sum} ÷ 5?", mean.ToString("0.##"),
                  [
                      $"{sum} ÷ 5.",
                      "Divide by how many there are.",
                      $"{mean:0.##}."
                  ],
                  $"Mean {mean:0.##}.", "a number"),

                S("The median is the middle one once they are in order. They already are.",
                  "What is the median?", vals[2].ToString(),
                  [
                      "Line them up and take the middle.",
                      $"{string.Join(", ", vals)}.",
                      $"The middle is {vals[2]}."
                  ],
                  "Middle value.", "a number",
                  "The median is positional, so one very large or small value cannot drag it.",
                  ["The median is the average of the largest and smallest.",
                   "The median and mean are always the same number."])
            ],
            Recap = $"Total {sum}, mean {mean:0.##}, median {vals[2]}.\nOne freak game moves the mean but not the median."
        };
    }
}
