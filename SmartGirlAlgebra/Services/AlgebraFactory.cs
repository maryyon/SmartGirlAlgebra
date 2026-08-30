using SmartGirlAlgebra.Models;

namespace SmartGirlAlgebra.Services;

/// <summary>
/// Algebra 1, generated fresh every time.
///
/// Same rule as everywhere else in this app: no choices, every step typed, new
/// numbers each problem. At ninth grade the numbers are bigger and negatives
/// turn up, but the reason is identical — an answer that can be remembered is
/// not an answer that was worked out.
/// </summary>
public static class AlgebraFactory
{
    public static WorkedProblem Make(int skillId, Random r, Flavor f) => skillId switch
    {
        1101 => Integers(r, f),
        1102 => OrderWithPowers(r, f),
        1103 => Evaluate(r, f),
        1104 => LikeTerms(r, f),

        1201 => TwoStep(r, f),
        1202 => BothSides(r, f),
        1203 => Distribute(r, f),
        1204 => Rearrange(r, f),

        1301 => Inequality(r, f),
        1302 => Proportion(r, f),
        1303 => Percent(r, f),
        1304 => UnitRate(r, f),

        1401 => SlopeFromPoints(r, f),
        1402 => SlopeIntercept(r, f),
        1403 => FromPointAndSlope(r, f),
        1404 => Intercepts(r, f),

        1501 => Substitution(r, f),
        1502 => Elimination(r, f),
        1503 => ExponentRules(r, f),
        1504 => Scientific(r, f),

        1601 => Foil(r, f),
        1602 => FactorTrinomial(r, f),
        1603 => SolveByFactoring(r, f),
        1604 => Pythagoras(r, f),

        _ => Integers(r, f)
    };

    private static int P(Random r, int lo, int hi) => r.Next(lo, hi + 1);

    private static int NonZero(Random r, int lo, int hi)
    {
        var v = P(r, lo, hi);
        return v == 0 ? (hi > 0 ? hi : lo) : v;
    }

    private static int Gcd(int a, int b) { while (b != 0) (a, b) = (b, a % b); return Math.Abs(a); }

    private static string Signed(int n) => n < 0 ? $"({n})" : n.ToString();

    private static TypedStep S(string say, string ask, string answer, string[] hints,
                               string yes, string placeholder = "a number", params string[] accept)
        => new() { Say = say, Ask = ask, Answer = answer, Hints = hints, Yes = yes,
                   Placeholder = placeholder, Accept = accept };

    /// <summary>Reduced fraction as text, or a whole number when it divides.</summary>
    private static string Frac(int top, int bottom)
    {
        if (bottom < 0) { top = -top; bottom = -bottom; }
        var g = Gcd(Math.Abs(top), Math.Abs(bottom));
        if (g != 0) { top /= g; bottom /= g; }
        return bottom == 1 ? top.ToString() : $"{top}/{bottom}";
    }

    // ============================================================== unit one

    private static WorkedProblem Integers(Random r, Flavor f)
    {
        int a = P(r, -18, -3), b = P(r, 4, 20), c = P(r, -14, -2);
        var step1 = a + b;
        var total = step1 - c;

        return new WorkedProblem
        {
            Setup = $"A team's point difference changed by {a}, then by +{b}, then by {c}.\n\nWork out {a} + {b} − {Signed(c)}",
            Steps =
            [
                S("Take it left to right. Adding a positive moves you up the number line.",
                  $"What is {a} + {b}?", step1.ToString(),
                  [
                      $"Start at {a} and move {b} to the right.",
                      $"{b} is bigger than {Math.Abs(a)}, so you pass zero." ,
                      $"{a} + {b} = {step1}."
                  ],
                  "Good — through zero and out the other side."),

                S($"Now subtracting a negative. Taking away a debt makes you better off, so it turns into ADDING.",
                  $"Rewrite − {Signed(c)} as an addition. What is being added?", Math.Abs(c).ToString(),
                  [
                      "Two minus signs together make a plus.",
                      $"− ({c}) becomes + {Math.Abs(c)}.",
                      $"So you add {Math.Abs(c)}."
                  ],
                  "Yes — minus a minus is a plus."),

                S("So finish it off.",
                  $"What is {step1} + {Math.Abs(c)}?", total.ToString(),
                  [
                      $"{step1} + {Math.Abs(c)}.",
                      $"Move {Math.Abs(c)} to the right from {step1}.",
                      $"It is {total}."
                  ],
                  $"{total}. Signs handled.")
            ],
            Recap = $"{a} + {b} − ({c})\n = {step1} + {Math.Abs(c)}\n = {total}\n\nSubtracting a negative is adding."
        };
    }

    private static WorkedProblem OrderWithPowers(Random r, Flavor f)
    {
        int b = P(r, 2, 5), a = P(r, 2, 9), c = P(r, 2, 9), d = P(r, 2, 20);
        var power = b * b;
        var prod = a * power;
        var answer = prod - d + c;

        return new WorkedProblem
        {
            Setup = $"Work this out:\n\n{a} × {b}² − {d} + {c}",
            Steps =
            [
                S("Powers come before everything except brackets.",
                  $"What is {b}²?", power.ToString(),
                  [
                      $"{b}² means {b} × {b}.",
                      $"Not {b} × 2.",
                      $"{b} × {b} = {power}."
                  ],
                  "Powers first — good."),

                S("Now multiplication, before any adding or taking away.",
                  $"What is {a} × {power}?", prod.ToString(),
                  [
                      $"{a} × {power}.",
                      "Multiplying comes before + and −.",
                      $"{a} × {power} = {prod}."
                  ],
                  "Good."),

                S($"Now it reads {prod} − {d} + {c}. Left to right from here.",
                  $"What is the answer?", answer.ToString(),
                  [
                      $"{prod} − {d} = {prod - d}.",
                      $"Then add {c}.",
                      $"{prod - d} + {c} = {answer}."
                  ],
                  $"{answer}. Order kept.")
            ],
            Recap = $"{a} × {b}² − {d} + {c}\n = {a} × {power} − {d} + {c}\n = {prod} − {d} + {c}\n = {answer}\n\nPowers, then × and ÷, then + and −."
        };
    }

    private static WorkedProblem Evaluate(Random r, Flavor f)
    {
        int a = P(r, 2, 6), x = P(r, -8, -2), b = P(r, 3, 12);
        var sq = x * x;
        var first = a * sq;
        var answer = first + b * x;

        return new WorkedProblem
        {
            Setup = $"Work out {a}x² + {b}x when x = {x}.",
            Steps =
            [
                S($"Put {x} in wherever there is an x — in brackets, so the sign is not lost.",
                  $"What is ({x})²?", sq.ToString(),
                  [
                      $"({x}) × ({x}).",
                      "A negative times a negative is positive.",
                      $"({x})² = {sq}."
                  ],
                  "Positive — negatives square away."),

                S("Now the first term.",
                  $"What is {a} × {sq}?", first.ToString(),
                  [
                      $"{a} × {sq}.",
                      "That is the whole first term.",
                      $"{a} × {sq} = {first}."
                  ],
                  "Good."),

                S("Now the second term, and remember x is still negative.",
                  $"What is {b} × ({x})?", (b * x).ToString(),
                  [
                      $"{b} × {x}.",
                      "A positive times a negative is negative.",
                      $"{b} × ({x}) = {b * x}."
                  ],
                  "Sign kept."),

                S("Put the two terms together.",
                  $"What is {a}x² + {b}x when x = {x}?", answer.ToString(),
                  [
                      $"{first} + ({b * x}).",
                      "Adding a negative is taking away.",
                      $"{first} − {Math.Abs(b * x)} = {answer}."
                  ],
                  $"{answer}.")
            ],
            Recap = $"{a}x² + {b}x at x = {x}\n = {a}({sq}) + {b}({x})\n = {first} + ({b * x})\n = {answer}"
        };
    }

    private static WorkedProblem LikeTerms(Random r, Flavor f)
    {
        int a = P(r, 3, 9), b = P(r, 2, 8), c = P(r, 2, 9), d = P(r, 2, 9);
        var xs = a - b;
        var nums = d - c;

        return new WorkedProblem
        {
            Setup = $"Tidy this up:\n\n{a}x + {c} − {b}x − {(nums >= 0 ? c : c)} + {d}",
            Steps =
            [
                S("Only terms of the same kind can be joined. x terms with x terms, plain numbers with plain numbers.",
                  $"Collect the x terms: what is {a}x − {b}x? (just the number in front)", xs.ToString(),
                  [
                      $"{a} − {b}.",
                      "The x comes along for the ride.",
                      $"{a}x − {b}x = {xs}x."
                  ],
                  $"{xs}x."),

                S("Now the plain numbers.",
                  $"What is {c} − {c} + {d}?", d.ToString(),
                  [
                      $"{c} − {c} cancels out.",
                      $"That leaves just {d}.",
                      $"It is {d}."
                  ],
                  "Good."),

                S("Write the tidied expression, x term first.",
                  "What does it simplify to? (like 4x + 7)", $"{xs}x+{d}",
                  [
                      $"You have {xs}x and {d}.",
                      "Put the x term first, then the number.",
                      $"{xs}x + {d}."
                  ],
                  "Tidy.", "like 4x + 7",
                  $"{xs}x + {d}", $"{d}+{xs}x", $"{d} + {xs}x")
            ],
            Recap = $"{a}x + {c} − {b}x − {c} + {d} = {xs}x + {d}\n\nJoin like with like; nothing else can be joined."
        };
    }

    // ============================================================== unit two

    private static WorkedProblem TwoStep(Random r, Flavor f)
    {
        int m = P(r, 2, 9), x = P(r, 2, 14), b = P(r, 3, 25);
        var total = m * x + b;

        return new WorkedProblem
        {
            Setup = $"{f.Place}: each of x {f.Boxes} scored {m} {f.Things}, plus {b} from free throws. {total} in all.\n\n{m}x + {b} = {total}",
            Steps =
            [
                S($"Undo in the opposite order to how it was built. The + {b} goes first.",
                  $"What is {total} − {b}?", (m * x).ToString(),
                  [
                      $"Take {b} off both sides.",
                      $"{total} − {b}.",
                      $"That leaves {m}x = {m * x}."
                  ],
                  $"Now it reads {m}x = {m * x}."),

                S($"One thing left: x is multiplied by {m}.",
                  $"Divide both sides by {m}. What is x?", x.ToString(),
                  [
                      $"{m * x} ÷ {m}.",
                      $"{m} × ? = {m * x}.",
                      $"x = {x}."
                  ],
                  $"x = {x}."),

                S("Always check by putting it back.",
                  $"What is {m} × {x} + {b}?", total.ToString(),
                  [
                      $"{m} × {x} = {m * x}.",
                      $"Then add {b}.",
                      $"You get {total}, which is what you started with."
                  ],
                  "Checked — that is how you know.")
            ],
            Recap = $"{m}x + {b} = {total}\n  −{b}:  {m}x = {m * x}\n  ÷{m}:  x = {x}\n  check: {m}({x}) + {b} = {total} ✓"
        };
    }

    private static WorkedProblem BothSides(Random r, Flavor f)
    {
        int a = P(r, 4, 9), c = P(r, 2, 3), x = P(r, 2, 12), b = P(r, 2, 20);
        var d = (a - c) * x + b;

        return new WorkedProblem
        {
            Setup = $"Solve:\n\n{a}x + {b} = {c}x + {d}",
            Steps =
            [
                S("Get all the x terms onto one side. Take the smaller one off both sides.",
                  $"Take {c}x from both sides. What is {a}x − {c}x? (just the number in front)",
                  (a - c).ToString(),
                  [
                      $"{a} − {c}.",
                      "Same thing done to both sides keeps it balanced.",
                      $"{a}x − {c}x = {a - c}x."
                  ],
                  $"Left side is now {a - c}x + {b}."),

                S($"Now the numbers. The equation reads {a - c}x + {b} = {d}.",
                  $"Take {b} off both sides. What is {d} − {b}?", ((a - c) * x).ToString(),
                  [
                      $"{d} − {b}.",
                      "That clears the number from the left.",
                      $"{a - c}x = {(a - c) * x}."
                  ],
                  "Good."),

                S($"One step left.",
                  $"Divide both sides by {a - c}. What is x?", x.ToString(),
                  [
                      $"{(a - c) * x} ÷ {a - c}.",
                      $"{a - c} × ? = {(a - c) * x}.",
                      $"x = {x}."
                  ],
                  $"x = {x}. Check: {a}({x}) + {b} = {a * x + b}, and {c}({x}) + {d} = {c * x + d}.")
            ],
            Recap = $"{a}x + {b} = {c}x + {d}\n  −{c}x:  {a - c}x + {b} = {d}\n  −{b}:   {a - c}x = {(a - c) * x}\n  ÷{a - c}:   x = {x}"
        };
    }

    private static WorkedProblem Distribute(Random r, Flavor f)
    {
        int a = P(r, 2, 7), b = P(r, 2, 9), x = P(r, 2, 12);
        var total = a * (x + b);

        return new WorkedProblem
        {
            Setup = $"Solve:\n\n{a}(x + {b}) = {total}",
            Steps =
            [
                S($"The {a} outside multiplies EVERYTHING inside, not just the first bit.",
                  $"Multiply out: {a} × {b} = ?", (a * b).ToString(),
                  [
                      $"{a} times the {b} inside.",
                      "Both terms get multiplied.",
                      $"{a} × {b} = {a * b}."
                  ],
                  $"So it becomes {a}x + {a * b} = {total}."),

                S($"Now it is an ordinary two-step. Undo the + {a * b}.",
                  $"What is {total} − {a * b}?", (a * x).ToString(),
                  [
                      $"{total} − {a * b}.",
                      $"That leaves {a}x on the left.",
                      $"{a}x = {a * x}."
                  ],
                  "Good."),

                S($"Last step.",
                  $"Divide by {a}. What is x?", x.ToString(),
                  [
                      $"{a * x} ÷ {a}.",
                      $"{a} × ? = {a * x}.",
                      $"x = {x}."
                  ],
                  $"x = {x}. Check: {a}({x} + {b}) = {a}({x + b}) = {total}.")
            ],
            Recap = $"{a}(x + {b}) = {total}\n  expand:  {a}x + {a * b} = {total}\n  −{a * b}:  {a}x = {a * x}\n  ÷{a}:  x = {x}"
        };
    }

    private static WorkedProblem Rearrange(Random r, Flavor f)
    {
        int m = P(r, 2, 9), b = P(r, 2, 15);

        return new WorkedProblem
        {
            Setup = $"Rearrange to get x on its own:\n\ny = {m}x + {b}",
            Steps =
            [
                S("Same moves as solving, except the other side is letters instead of a number.",
                  $"Take {b} off both sides. What is the left side now? (like y - 4)",
                  $"y-{b}",
                  [
                      $"You are subtracting {b} from y.",
                      "It cannot be simplified further — y is not a number.",
                      $"It is y − {b}."
                  ],
                  $"So y − {b} = {m}x.", "like y - 4",
                  $"y - {b}", $"y−{b}"),

                S($"Now x is multiplied by {m}.",
                  $"What do you do to both sides? (write it like /3)", $"/{m}",
                  [
                      $"The opposite of times {m}.",
                      $"Divide by {m}.",
                      $"/{m}."
                  ],
                  "Yes.", "like /3",
                  $"÷{m}", $"divide by {m}", m.ToString()),

                S("Write the finished rearrangement.",
                  "What does x equal? (like (y - 4)/3)", $"(y-{b})/{m}",
                  [
                      $"The whole of y − {b} gets divided by {m}.",
                      "Brackets matter — the whole top is divided.",
                      $"x = (y − {b})/{m}."
                  ],
                  "Whole numerator, brackets kept.", "like (y - 4)/3",
                  $"(y - {b})/{m}", $"(y−{b})/{m}")
            ],
            Recap = $"y = {m}x + {b}\n  −{b}:  y − {b} = {m}x\n  ÷{m}:  x = (y − {b})/{m}"
        };
    }

    // ============================================================ unit three

    private static WorkedProblem Inequality(Random r, Flavor f)
    {
        int m = P(r, 2, 6), x = P(r, 2, 9), b = P(r, 2, 14);
        var total = -m * x + b;

        return new WorkedProblem
        {
            Setup = $"Solve, and mind the sign:\n\n−{m}x + {b} > {total - m}",
            Steps =
            [
                S($"Start the same way as an equation. Take the {b} off both sides.",
                  $"What is {total - m} − {b}?", (total - m - b).ToString(),
                  [
                      $"{total - m} − {b}.",
                      "Nothing special happens here yet.",
                      $"It is {total - m - b}."
                  ],
                  $"So −{m}x > {total - m - b}."),

                S($"Now divide by −{m}. This is the one rule people forget.",
                  "When you divide an inequality by a NEGATIVE, what happens to the > sign?",
                  "it flips",
                  [
                      "Try it with numbers: 2 < 4, now divide both by −1.",
                      "−2 is GREATER than −4, so the sign turned round.",
                      "It flips — > becomes <."
                  ],
                  "It flips. That is the whole trick.", "flips or stays",
                  "flips", "it flips over", "flip", "reverses", "it reverses"),

                S($"So divide by −{m} and turn the sign round.",
                  "What is x? (like x < 5)", $"x<{(total - m - b) / -m}",
                  [
                      $"{total - m - b} ÷ −{m} = {(total - m - b) / -m}.",
                      "And the > becomes <.",
                      $"x < {(total - m - b) / -m}."
                  ],
                  "Divided and flipped.", "like x < 5",
                  $"x < {(total - m - b) / -m}")
            ],
            Recap = $"−{m}x + {b} > {total - m}\n  −{b}:  −{m}x > {total - m - b}\n  ÷−{m}:  x < {(total - m - b) / -m}   (sign flips)"
        };
    }

    private static WorkedProblem Proportion(Random r, Flavor f)
    {
        int a = P(r, 2, 9), b = P(r, 3, 12), k = P(r, 2, 8);
        int c = a * k, d = b * k;

        return new WorkedProblem
        {
            Setup = $"{a} {f.People} need {b} {f.Things}.\nHow many for {c} {f.People}?\n\n{a}/{b} = {c}/x",
            Steps =
            [
                S("Cross-multiply: top-left times bottom-right, top-right times bottom-left.",
                  $"What is {b} × {c}?", (b * c).ToString(),
                  [
                      $"{b} × {c}.",
                      "That is one of the two cross products.",
                      $"{b} × {c} = {b * c}."
                  ],
                  $"So {a}x = {b * c}."),

                S($"Now solve {a}x = {b * c}.",
                  $"What is x?", d.ToString(),
                  [
                      $"{b * c} ÷ {a}.",
                      $"{a} × ? = {b * c}.",
                      $"x = {d}."
                  ],
                  $"x = {d}."),

                S("Sense-check it.",
                  $"{c} is {k} times {a}. So {d} should be {k} times what?", b.ToString(),
                  [
                      "The two sides scale together.",
                      $"{d} ÷ {k}.",
                      $"It is {b} — which is what you started with."
                  ],
                  "Scales properly.")
            ],
            Recap = $"{a}/{b} = {c}/x  →  {a}x = {b * c}  →  x = {d}\n\nBoth sides scaled by {k}."
        };
    }

    private static WorkedProblem Percent(Random r, Flavor f)
    {
        int pct = new[] { 10, 15, 20, 25, 30, 40, 50, 60, 75 }[r.Next(9)];
        int whole = P(r, 4, 40) * 5;
        var part = whole * pct / 100;

        return new WorkedProblem
        {
            Setup = $"{whole} {f.Things} were scored this season.\n{pct}% of them came in the last {f.Box}.",
            Steps =
            [
                S("Per cent means out of a hundred. Write it as a fraction first.",
                  $"Write {pct}% as a decimal. (like 0.25)", (pct / 100.0).ToString("0.##"),
                  [
                      "Divide by 100.",
                      $"{pct} ÷ 100.",
                      $"{pct}% = {pct / 100.0:0.##}."
                  ],
                  "Good.", "like 0.25",
                  $"{pct}/100"),

                S("\"Of\" means multiply.",
                  $"What is {pct / 100.0:0.##} × {whole}?", part.ToString(),
                  [
                      $"{pct / 100.0:0.##} × {whole}.",
                      $"Or {whole} ÷ 100 × {pct}.",
                      $"It is {part}."
                  ],
                  $"{part} {f.Things}."),

                S("And the rest?",
                  $"How many were scored BEFORE the last {f.Box}?", (whole - part).ToString(),
                  [
                      $"{whole} − {part}.",
                      "The whole minus the part.",
                      $"{whole} − {part} = {whole - part}."
                  ],
                  "Both parts add back to the whole.")
            ],
            Recap = $"{pct}% of {whole} = {pct / 100.0:0.##} × {whole} = {part}\nThe other {whole - part} came earlier."
        };
    }

    private static WorkedProblem UnitRate(Random r, Flavor f)
    {
        int per = P(r, 3, 25), units = P(r, 3, 12);
        var total = per * units;
        var many = P(r, 2, 9) + units;

        return new WorkedProblem
        {
            Setup = $"{units} {f.People} scored {total} {f.Things} between them, all equal.",
            Steps =
            [
                S("A unit rate is how much for ONE. Divide the total by how many.",
                  $"What is {total} ÷ {units}?", per.ToString(),
                  [
                      $"{total} shared between {units}.",
                      $"{units} × ? = {total}.",
                      $"{total} ÷ {units} = {per}."
                  ],
                  $"{per} each — that is the rate."),

                S("A rate is useful because you can scale it.",
                  $"At that rate, how many would {many} {f.People} score?", (per * many).ToString(),
                  [
                      $"{per} each, {many} of them.",
                      $"{per} × {many}.",
                      $"It is {per * many}."
                  ],
                  "Rate times number."),

                S("Backwards now, which is the harder direction.",
                  $"How many {f.People} would it take to score {per * 20} {f.Things}?", "20",
                  [
                      $"Divide by the rate: {per * 20} ÷ {per}.",
                      "Total divided by rate gives how many.",
                      "It is 20."
                  ],
                  "Both directions.")
            ],
            Recap = $"{total} ÷ {units} = {per} each.\nRate × number = total, and total ÷ rate = number."
        };
    }

    // ============================================================= unit four

    private static WorkedProblem SlopeFromPoints(Random r, Flavor f)
    {
        int x1 = P(r, -6, 4), y1 = P(r, -6, 8);
        int run = P(r, 1, 5), m = NonZero(r, -4, 5);
        int x2 = x1 + run, y2 = y1 + m * run;

        return new WorkedProblem
        {
            Setup = $"A line passes through ({x1}, {y1}) and ({x2}, {y2}).",
            Steps =
            [
                S("Slope is rise over run: how far up, divided by how far across.",
                  $"What is the rise, {y2} − {Signed(y1)}?", (y2 - y1).ToString(),
                  [
                      $"{y2} − {Signed(y1)}.",
                      "Careful with the signs.",
                      $"The rise is {y2 - y1}."
                  ],
                  $"Rise {y2 - y1}."),

                S("Now the run, in the SAME order.",
                  $"What is {x2} − {Signed(x1)}?", run.ToString(),
                  [
                      $"{x2} − {Signed(x1)}.",
                      "Same order as the rise, or the sign comes out wrong.",
                      $"The run is {run}."
                  ],
                  $"Run {run}."),

                S("Slope is one over the other.",
                  "What is the slope?", m.ToString(),
                  [
                      $"{y2 - y1} ÷ {run}.",
                      "Rise over run.",
                      $"The slope is {m}."
                  ],
                  $"m = {m}.")
            ],
            Recap = $"m = (y₂ − y₁)/(x₂ − x₁) = ({y2} − {Signed(y1)})/({x2} − {Signed(x1)}) = {y2 - y1}/{run} = {m}"
        };
    }

    private static WorkedProblem SlopeIntercept(Random r, Flavor f)
    {
        int m = NonZero(r, -5, 6), b = P(r, -9, 12), x = P(r, 1, 6);
        var y = m * x + b;

        return new WorkedProblem
        {
            Setup = $"A line is y = {m}x + {Signed(b)}",
            Steps =
            [
                S("In y = mx + b, the m and the b each mean something specific.",
                  "What is the slope?", m.ToString(),
                  [
                      "It is the number attached to the x.",
                      "The other number is not the slope.",
                      $"The slope is {m}."
                  ],
                  $"Slope {m}."),

                S("And the other one.",
                  "What is the y-intercept — where it crosses the y-axis?", b.ToString(),
                  [
                      "It is the number on its own.",
                      "It is the y value when x = 0.",
                      $"The y-intercept is {b}."
                  ],
                  $"Crosses at {b}."),

                S("Now use the line.",
                  $"What is y when x = {x}?", y.ToString(),
                  [
                      $"{m} × {x} + {Signed(b)}.",
                      $"{m} × {x} = {m * x}.",
                      $"{m * x} + {Signed(b)} = {y}."
                  ],
                  $"({x}, {y}) is on the line.")
            ],
            Recap = $"y = {m}x + {Signed(b)}: slope {m}, crosses the y-axis at {b}.\nAt x = {x}, y = {y}."
        };
    }

    private static WorkedProblem FromPointAndSlope(Random r, Flavor f)
    {
        int m = NonZero(r, -4, 5), x1 = P(r, -5, 6), y1 = P(r, -6, 9);
        var b = y1 - m * x1;

        return new WorkedProblem
        {
            Setup = $"A line has slope {m} and passes through ({x1}, {y1}).\n\nFind its equation.",
            Steps =
            [
                S("Start from y = mx + b. You know m, and you know one point that must fit.",
                  $"Put the point in: what is {m} × {Signed(x1)}?", (m * x1).ToString(),
                  [
                      $"{m} × {Signed(x1)}.",
                      "Watch the signs.",
                      $"{m} × {Signed(x1)} = {m * x1}."
                  ],
                  $"So {y1} = {m * x1} + b."),

                S("Now solve for b.",
                  $"What is {y1} − {Signed(m * x1)}?", b.ToString(),
                  [
                      $"{y1} − {Signed(m * x1)}.",
                      "Subtracting a negative adds.",
                      $"b = {b}."
                  ],
                  $"b = {b}."),

                S("Write the whole equation.",
                  "What is the equation? (like y = 2x + 3)", $"y={m}x+{b}",
                  [
                      $"You have m = {m} and b = {b}.",
                      "Put them into y = mx + b.",
                      $"y = {m}x + {Signed(b)}."
                  ],
                  "That is the line.", "like y = 2x + 3",
                  $"y = {m}x + {b}", $"y={m}x{(b < 0 ? b.ToString() : "+" + b)}",
                  $"y = {m}x {(b < 0 ? "- " + Math.Abs(b) : "+ " + b)}")
            ],
            Recap = $"y = mx + b with m = {m} and ({x1}, {y1}):\n  {y1} = {m}({x1}) + b\n  b = {b}\n  y = {m}x + {Signed(b)}"
        };
    }

    private static WorkedProblem Intercepts(Random r, Flavor f)
    {
        int a = P(r, 2, 6), b = P(r, 2, 6), k = a * b * P(r, 1, 3);

        return new WorkedProblem
        {
            Setup = $"A line is {a}x + {b}y = {k}",
            Steps =
            [
                S("The x-intercept is where the line crosses the x-axis — and there, y is 0.",
                  $"Put y = 0. What is x?", (k / a).ToString(),
                  [
                      $"{a}x + {b}(0) = {k}, so {a}x = {k}.",
                      $"{k} ÷ {a}.",
                      $"x = {k / a}."
                  ],
                  $"It crosses at ({k / a}, 0)."),

                S("The y-intercept is the other way round — there, x is 0.",
                  $"Put x = 0. What is y?", (k / b).ToString(),
                  [
                      $"{a}(0) + {b}y = {k}, so {b}y = {k}.",
                      $"{k} ÷ {b}.",
                      $"y = {k / b}."
                  ],
                  $"It crosses at (0, {k / b})."),

                S("Two points is enough to draw any straight line.",
                  $"How many points do you need to draw a straight line?", "2",
                  [
                      "You have just found two of them.",
                      "A straight line is fixed by any two points on it.",
                      "Two."
                  ],
                  "Two points and a ruler.")
            ],
            Recap = $"{a}x + {b}y = {k}\n  y = 0 → x = {k / a}\n  x = 0 → y = {k / b}\nPlot ({k / a}, 0) and (0, {k / b}) and join them."
        };
    }

    // ============================================================= unit five

    private static WorkedProblem Substitution(Random r, Flavor f)
    {
        int x = P(r, 1, 8), y = P(r, 1, 8), m = P(r, 2, 5), b = P(r, 1, 9);
        var yExpr = m * x + b;
        int c = P(r, 2, 4);
        var rhs = c * x + (m * x + b);

        return new WorkedProblem
        {
            Setup = $"Solve the pair:\n\ny = {m}x + {b}\n{c}x + y = {rhs}",
            Steps =
            [
                S("The first equation already tells you what y is. Put that into the second.",
                  $"Replacing y, the second becomes {c}x + {m}x + {b} = {rhs}. What is {c}x + {m}x? (just the number in front)",
                  (c + m).ToString(),
                  [
                      $"{c} + {m}.",
                      "They are like terms, so they join.",
                      $"{c}x + {m}x = {c + m}x."
                  ],
                  $"So {c + m}x + {b} = {rhs}."),

                S("Now it is a two-step equation.",
                  $"Solve it. What is x?", x.ToString(),
                  [
                      $"Take {b} off: {c + m}x = {rhs - b}.",
                      $"Divide by {c + m}.",
                      $"x = {x}."
                  ],
                  $"x = {x}."),

                S("Now put x back into the first equation to get y.",
                  $"What is y?", yExpr.ToString(),
                  [
                      $"y = {m}({x}) + {b}.",
                      $"{m} × {x} = {m * x}.",
                      $"y = {yExpr}."
                  ],
                  $"So ({x}, {yExpr}).")
            ],
            Recap = $"Substituting: {c + m}x + {b} = {rhs} → x = {x}, then y = {m}({x}) + {b} = {yExpr}.\nSolution ({x}, {yExpr})."
        };
    }

    private static WorkedProblem Elimination(Random r, Flavor f)
    {
        int x = P(r, 1, 8), y = P(r, 1, 8);
        int a = P(r, 2, 5), b = P(r, 2, 5), c = P(r, 2, 5);
        int s1 = a * x + b * y, s2 = c * x - b * y;

        return new WorkedProblem
        {
            Setup = $"Solve the pair:\n\n{a}x + {b}y = {s1}\n{c}x − {b}y = {s2}",
            Steps =
            [
                S($"Look at the y terms: +{b}y and −{b}y. Add the two equations and they cancel.",
                  $"Adding the left sides: what is {a}x + {c}x? (just the number in front)",
                  (a + c).ToString(),
                  [
                      $"{a} + {c}.",
                      "The y terms cancel to nothing.",
                      $"{a}x + {c}x = {a + c}x."
                  ],
                  "The y is gone — that is the point."),

                S("Add the right sides too.",
                  $"What is {s1} + {s2}?", (s1 + s2).ToString(),
                  [
                      $"{s1} + {s2}.",
                      "Both sides get added.",
                      $"It is {s1 + s2}."
                  ],
                  $"So {a + c}x = {s1 + s2}."),

                S("Now solve for x.",
                  "What is x?", x.ToString(),
                  [
                      $"{s1 + s2} ÷ {a + c}.",
                      $"{a + c} × ? = {s1 + s2}.",
                      $"x = {x}."
                  ],
                  $"x = {x}."),

                S("Put x back into the first equation to find y.",
                  "What is y?", y.ToString(),
                  [
                      $"{a}({x}) + {b}y = {s1}.",
                      $"{b}y = {s1 - a * x}.",
                      $"y = {y}."
                  ],
                  $"({x}, {y}).")
            ],
            Recap = $"Adding cancels y: {a + c}x = {s1 + s2} → x = {x}, then y = {y}.\nSolution ({x}, {y})."
        };
    }

    private static WorkedProblem ExponentRules(Random r, Flavor f)
    {
        int b = P(r, 2, 5), p = P(r, 2, 5), q = P(r, 2, 4);

        return new WorkedProblem
        {
            Setup = $"Simplify, leaving it as a power:\n\n({b}^{p}) × ({b}^{q})",
            Steps =
            [
                S("Same base, multiplied. Write out what it means and count.",
                  $"{b}^{p} is {b} multiplied {p} times, and {b}^{q} is {b} multiplied {q} times. How many {b}s altogether?",
                  (p + q).ToString(),
                  [
                      $"{p} of them and {q} more.",
                      $"{p} + {q}.",
                      $"{p + q} of them."
                  ],
                  "So the powers ADD."),

                S("Write the simplified power.",
                  $"What is ({b}^{p}) × ({b}^{q}) as a single power? (like 2^7)", $"{b}^{p + q}",
                  [
                      "Same base, add the exponents.",
                      $"{p} + {q} = {p + q}.",
                      $"{b}^{p + q}."
                  ],
                  "Added, not multiplied.", "like 2^7",
                  $"{b}^({p + q})"),

                S("And the value, so you can see it is not a trick.",
                  $"What is {b}^{p + q} as an ordinary number?", ((int)Math.Pow(b, p + q)).ToString(),
                  [
                      $"{b} multiplied by itself {p + q} times.",
                      $"{b}^{p + q}.",
                      $"It is {(int)Math.Pow(b, p + q)}."
                  ],
                  "Same thing, written out.")
            ],
            Recap = $"{b}^{p} × {b}^{q} = {b}^{p + q} = {(int)Math.Pow(b, p + q)}\n\nSame base multiplied → add the powers."
        };
    }

    private static WorkedProblem Scientific(Random r, Flavor f)
    {
        int lead = P(r, 1, 9), dec = P(r, 0, 99), power = P(r, 3, 6);
        var mantissa = lead + dec / 100.0;
        var value = mantissa * Math.Pow(10, power);

        return new WorkedProblem
        {
            Setup = $"A stadium deal is worth {value:N0} dollars.\n\nWrite it in scientific notation.",
            Steps =
            [
                S("Scientific notation is one digit, then a point, then the rest — times a power of ten.",
                  $"Where does the point go? Give the number part. (like 4.25)", mantissa.ToString("0.##"),
                  [
                      "Exactly one non-zero digit before the point.",
                      $"Start at {value:N0} and move the point left.",
                      $"The number part is {mantissa:0.##}."
                  ],
                  "One digit, then the rest.", "like 4.25"),

                S("Now count how far the point moved.",
                  "How many places did the point move left?", power.ToString(),
                  [
                      "Count the jumps from where it was to where it is now.",
                      $"From {value:N0} to {mantissa:0.##}.",
                      $"{power} places."
                  ],
                  $"{power} places."),

                S("Put it together.",
                  $"Write it in scientific notation. (like 4.25 x 10^6)",
                  $"{mantissa:0.##}x10^{power}",
                  [
                      $"The number part is {mantissa:0.##}.",
                      $"The power is {power}.",
                      $"{mantissa:0.##} × 10^{power}."
                  ],
                  "That is the standard form.", "like 4.25 x 10^6",
                  $"{mantissa:0.##} x 10^{power}", $"{mantissa:0.##}*10^{power}",
                  $"{mantissa:0.##} × 10^{power}")
            ],
            Recap = $"{value:N0} = {mantissa:0.##} × 10^{power}\n\nOne digit before the point; the power counts the places moved."
        };
    }

    // ============================================================== unit six

    private static WorkedProblem Foil(Random r, Flavor f)
    {
        int a = P(r, 1, 9), b = P(r, 1, 9);
        var mid = a + b;
        var last = a * b;

        return new WorkedProblem
        {
            Setup = $"Multiply out:\n\n(x + {a})(x + {b})",
            Steps =
            [
                S("Every term in the first bracket multiplies every term in the second. Four products.",
                  "First times first: what is x × x? (like x^2)", "x^2",
                  [
                      "x multiplied by itself.",
                      "It is not 2x.",
                      "x²."
                  ],
                  "x² — a squared term.", "like x^2",
                  "x²", "x*x", "xx"),

                S($"Now the two middle products: x × {b} and {a} × x.",
                  $"Added together, what is {b}x + {a}x? (just the number in front)", mid.ToString(),
                  [
                      $"{b} + {a}.",
                      "They are like terms, so they join.",
                      $"{mid}x."
                  ],
                  $"{mid}x in the middle."),

                S($"And last times last: {a} × {b}.",
                  $"What is {a} × {b}?", last.ToString(),
                  [
                      $"{a} × {b}.",
                      "The two numbers on their own.",
                      $"{last}."
                  ],
                  "Good."),

                S("Put all three parts together.",
                  "What does it expand to? (like x^2 + 5x + 6)",
                  $"x^2+{mid}x+{last}",
                  [
                      $"x², then {mid}x, then {last}.",
                      "Highest power first.",
                      $"x² + {mid}x + {last}."
                  ],
                  "Expanded.", "like x^2 + 5x + 6",
                  $"x^2 + {mid}x + {last}", $"x² + {mid}x + {last}", $"x²+{mid}x+{last}")
            ],
            Recap = $"(x + {a})(x + {b}) = x² + {mid}x + {last}\n\nThe middle is {a} + {b}; the end is {a} × {b}."
        };
    }

    private static WorkedProblem FactorTrinomial(Random r, Flavor f)
    {
        int a = P(r, 1, 9), b = P(r, 1, 9);
        var mid = a + b;
        var last = a * b;

        return new WorkedProblem
        {
            Setup = $"Factorise:\n\nx² + {mid}x + {last}",
            Steps =
            [
                S("Factorising is expanding backwards. You need two numbers that do two jobs at once.",
                  $"You need two numbers that MULTIPLY to {last}. What do they ADD to?", mid.ToString(),
                  [
                      "Look at the number in front of the x.",
                      "That is always the sum of the pair.",
                      $"They add to {mid}."
                  ],
                  $"Multiply to {last}, add to {mid}."),

                S($"Find the pair. List the factor pairs of {last} and check which adds to {mid}.",
                  $"What is the SMALLER of the two numbers?", Math.Min(a, b).ToString(),
                  [
                      $"Which pairs multiply to {last}?",
                      $"{Math.Min(a, b)} × {Math.Max(a, b)} = {last}, and {Math.Min(a, b)} + {Math.Max(a, b)} = {mid}.",
                      $"The smaller one is {Math.Min(a, b)}."
                  ],
                  $"And the other is {Math.Max(a, b)}."),

                S("Write it as two brackets.",
                  "What does it factorise to? (like (x + 2)(x + 3))",
                  $"(x+{Math.Min(a, b)})(x+{Math.Max(a, b)})",
                  [
                      $"The two numbers are {Math.Min(a, b)} and {Math.Max(a, b)}.",
                      "Each goes in its own bracket with an x.",
                      $"(x + {Math.Min(a, b)})(x + {Math.Max(a, b)})."
                  ],
                  "Check it by expanding.", "like (x + 2)(x + 3)",
                  $"(x + {Math.Min(a, b)})(x + {Math.Max(a, b)})",
                  $"(x+{Math.Max(a, b)})(x+{Math.Min(a, b)})",
                  $"(x + {Math.Max(a, b)})(x + {Math.Min(a, b)})")
            ],
            Recap = $"x² + {mid}x + {last} = (x + {Math.Min(a, b)})(x + {Math.Max(a, b)})\n\nTwo numbers that multiply to {last} and add to {mid}."
        };
    }

    private static WorkedProblem SolveByFactoring(Random r, Flavor f)
    {
        int a = P(r, 1, 8), b = P(r, 1, 8);
        var mid = a + b;
        var last = a * b;

        return new WorkedProblem
        {
            Setup = $"Solve:\n\nx² + {mid}x + {last} = 0",
            Steps =
            [
                S("Factorise first. Two numbers that multiply to the end and add to the middle.",
                  $"What two numbers multiply to {last} and add to {mid}? Give the smaller.",
                  Math.Min(a, b).ToString(),
                  [
                      $"Factor pairs of {last}.",
                      $"{Math.Min(a, b)} and {Math.Max(a, b)}.",
                      $"The smaller is {Math.Min(a, b)}."
                  ],
                  $"So (x + {Math.Min(a, b)})(x + {Math.Max(a, b)}) = 0."),

                S("If two things multiply to ZERO, at least one of them must BE zero. That is the whole method.",
                  $"If (x + {Math.Min(a, b)}) = 0, what is x?", (-Math.Min(a, b)).ToString(),
                  [
                      $"x + {Math.Min(a, b)} = 0.",
                      $"Take {Math.Min(a, b)} off both sides.",
                      $"x = {-Math.Min(a, b)}."
                  ],
                  "One solution found."),

                S("And the other bracket.",
                  $"If (x + {Math.Max(a, b)}) = 0, what is x?", (-Math.Max(a, b)).ToString(),
                  [
                      $"x + {Math.Max(a, b)} = 0.",
                      $"Take {Math.Max(a, b)} off both sides.",
                      $"x = {-Math.Max(a, b)}."
                  ],
                  $"Two solutions: {-Math.Min(a, b)} and {-Math.Max(a, b)}.")
            ],
            Recap = $"x² + {mid}x + {last} = 0\n  (x + {Math.Min(a, b)})(x + {Math.Max(a, b)}) = 0\n  x = {-Math.Min(a, b)} or x = {-Math.Max(a, b)}\n\nA quadratic usually has TWO answers."
        };
    }

    private static WorkedProblem Pythagoras(Random r, Flavor f)
    {
        (int a, int b, int c)[] triples = [(3, 4, 5), (6, 8, 10), (5, 12, 13), (9, 12, 15), (8, 15, 17), (7, 24, 25)];
        var t = triples[r.Next(triples.Length)];

        return new WorkedProblem
        {
            Setup = $"A player runs {t.a} metres up the court, then {t.b} metres across.\n\nHow far is she from where she started?",
            Steps =
            [
                S("That is a right-angled triangle, so a² + b² = c². Square the two short sides.",
                  $"What is {t.a}²?", (t.a * t.a).ToString(),
                  [
                      $"{t.a} × {t.a}.",
                      "Squared means times itself.",
                      $"{t.a}² = {t.a * t.a}."
                  ],
                  "Good."),

                S("And the other one.",
                  $"What is {t.b}²?", (t.b * t.b).ToString(),
                  [
                      $"{t.b} × {t.b}.",
                      "Same again.",
                      $"{t.b}² = {t.b * t.b}."
                  ],
                  "Good."),

                S("Add them to get c².",
                  $"What is {t.a * t.a} + {t.b * t.b}?", (t.c * t.c).ToString(),
                  [
                      $"{t.a * t.a} + {t.b * t.b}.",
                      "That gives you c², not c.",
                      $"It is {t.c * t.c}."
                  ],
                  "That is c² — one step left."),

                S("Now undo the square.",
                  $"What is the square root of {t.c * t.c}?", t.c.ToString(),
                  [
                      $"What number times itself makes {t.c * t.c}?",
                      $"Try {t.c}.",
                      $"√{t.c * t.c} = {t.c}."
                  ],
                  $"{t.c} metres.")
            ],
            Recap = $"{t.a}² + {t.b}² = {t.a * t.a} + {t.b * t.b} = {t.c * t.c}\nc = √{t.c * t.c} = {t.c} metres.\n\nDon't stop at c² — square-root it."
        };
    }
}
