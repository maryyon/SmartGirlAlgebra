using SmartGirlAlgebra.Models;

namespace SmartGirlAlgebra.Services;

/// <summary>
/// The methods course: tricks, shortcuts, and how to learn.
///
/// This one is not really about the answers. It teaches the MOVES — why nine
/// times anything has digits that add to nine, how to know something divides by
/// three without dividing, how to check your own work and catch your own
/// mistake before anyone else does.
///
/// It starts at fifth grade for a seventh grader, on purpose: a trick learned on
/// easy numbers is a trick you own. Language stays plain and short. It is meant
/// to be funny, because he will do more of it if it is.
/// </summary>
public static class MethodsFactory
{
    public static WorkedProblem Make(int skillId, Random r, Flavor f) => skillId switch
    {
        3101 => MagicNines(r, f),
        3102 => ElevenTrick(r, f),
        3103 => FiveTrick(r, f),
        3104 => DoubleHalve(r, f),

        3201 => DivisibleBy2510(r, f),
        3202 => DigitSumRule(r, f),
        3203 => DivisibleBy4And6(r, f),
        3204 => QuickFactor(r, f),

        3301 => MakeATen(r, f),
        3302 => Compensate(r, f),
        3303 => BreakApart(r, f),
        3304 => EstimateFirst(r, f),

        3401 => Benchmarks(r, f),
        3402 => SameThreeWays(r, f),
        3403 => TenPercentTrick(r, f),
        3404 => TipMath(r, f),

        3501 => Negatives(r, f),
        3502 => Expressions(r, f),
        3503 => OneStep(r, f),
        3504 => TwoStep(r, f),

        3601 => CheckByEstimate(r, f),
        3602 => CheckBackwards(r, f),
        3603 => FindTheMistake(r, f),
        3604 => ExplainIt(r, f),

        _ => MagicNines(r, f)
    };

    private static int P(Random r, int lo, int hi) => r.Next(lo, hi + 1);
    private static int DigitSum(int n) { var s = 0; n = Math.Abs(n); while (n > 0) { s += n % 10; n /= 10; } return s; }

    private static TypedStep S(string say, string ask, string answer, string[] hints, string yes,
                               string placeholder = "a number", string why = "",
                               string[]? whyWrong = null, params string[] accept)
        => new()
        {
            Say = say, Ask = ask, Answer = answer, Hints = hints, Yes = yes,
            Placeholder = placeholder, Why = why, WhyWrong = whyWrong ?? [], Accept = accept
        };

    // ====================================================== unit 1 — tricks

    private static WorkedProblem MagicNines(Random r, Flavor f)
    {
        var n = P(r, 3, 9);
        var prod = 9 * n;

        return new WorkedProblem
        {
            Setup = $"The nine times table has a secret.\nNobody tells you. I'm telling you.\n\nLook at 9 × {n}.",
            Steps =
            [
                S("First, do it the boring way so we can prove the trick works.",
                  $"What is 9 × {n}?", prod.ToString(),
                  [
                      $"Nine {n}s.",
                      $"Or: {n} tens take away {n}.",
                      $"{prod}."
                  ],
                  "Right. Now watch this."),

                S($"Add up the digits of your answer. {prod} has digits {prod / 10} and {prod % 10}.",
                  $"What is {prod / 10} + {prod % 10}?", "9",
                  [
                      $"{prod / 10} + {prod % 10}.",
                      "Just add the two digits.",
                      "It's 9. It is ALWAYS 9."
                  ],
                  "Nine. Every single time. Try to break it — you can't.", "a number",
                  "Nine is one less than ten, so every jump of nine adds one ten and takes away one unit.",
                  ["Nine is a lucky number so its digits behave.",
                   "It only works for answers below 100."]),

                S("Here's the actually useful part. The tens digit is always ONE LESS than what you multiplied by.",
                  $"So for 9 × 8, what is the tens digit?", "7",
                  [
                      "One less than 8.",
                      "8 − 1.",
                      "7 — and then the ones digit makes it up to 9, so 72."
                  ],
                  "9 × 8 = 72. You now know the nines forever.")
            ],
            Recap = $"9 × {n} = {prod}, and {prod / 10} + {prod % 10} = 9.\n\nTens digit = one less than the number.\nThe two digits always add to 9."
        };
    }

    private static WorkedProblem ElevenTrick(Random r, Flavor f)
    {
        int a = P(r, 1, 4), b = P(r, 0, 4);
        var n = a * 10 + b;
        var prod = n * 11;

        return new WorkedProblem
        {
            Setup = $"Eleven times a two-digit number, in your head, faster than anyone can type it.\n\n{n} × 11",
            Steps =
            [
                S($"Split the digits apart: {a} and {b}. Now add them together.",
                  $"What is {a} + {b}?", (a + b).ToString(),
                  [
                      $"{a} + {b}.",
                      "Just the two digits.",
                      $"{a + b}."
                  ],
                  "That goes in the MIDDLE."),

                S($"Now drop it between the two original digits: {a}, then the {a + b}, then {b}.",
                  $"What is {n} × 11?", prod.ToString(),
                  [
                      $"{a} _ {b}, with {a + b} in the gap.",
                      $"{a}{a + b}{b}.",
                      $"{prod}."
                  ],
                  "That's it. That's the whole trick.", "a number",
                  "Times eleven is times ten plus one more, so each digit lands in two places and overlaps in the middle.",
                  ["Eleven is prime, so its table has no pattern.",
                   "The middle digit is always the bigger of the two."]),

                S("Careful though — the trick has a catch when the middle adds to more than 9.",
                  $"For 78 × 11, the middle would be 7 + 8 = 15. Does 7-15-8 work as a number? (yes or no)",
                  "no",
                  [
                      "You can't put 15 in one digit slot.",
                      "The 1 has to carry into the 7.",
                      "No — it becomes 858."
                  ],
                  "Knowing when a trick breaks is worth more than the trick.", "yes or no")
            ],
            Recap = $"{n} × 11 = {prod}\n\nSplit, add the digits, drop the total in the middle.\nIf the middle goes over 9, carry it."
        };
    }

    private static WorkedProblem FiveTrick(Random r, Flavor f)
    {
        var n = P(r, 6, 24) * 2;

        return new WorkedProblem
        {
            Setup = $"Times five is just halve-and-times-ten.\nBecause 5 is half of 10. That's it. That's the reason.\n\n{n} × 5",
            Steps =
            [
                S("Halve it first. Halving is easier than multiplying, always.",
                  $"What is half of {n}?", (n / 2).ToString(),
                  [
                      $"{n} ÷ 2.",
                      "Split it in two.",
                      $"{n / 2}."
                  ],
                  "Now the easy bit."),

                S("Now times ten. Which means stick a zero on.",
                  $"What is {n / 2} × 10?", (n / 2 * 10).ToString(),
                  [
                      "Add a zero to the end.",
                      $"{n / 2} becomes {n / 2}0.",
                      $"{n / 2 * 10}."
                  ],
                  $"{n} × 5 = {n * 5}. No times table needed.", "a number",
                  "Five is half of ten, so multiplying by five is halving and then multiplying by ten.",
                  ["Five is half of ten, so you double and then divide by ten.",
                   "It works because five is an odd number."]),

                S("Prove it to yourself, because a trick you don't trust is useless.",
                  $"Does {n} × 5 really equal {n * 5}? (yes or no)", "yes",
                  [
                      "Check it the long way if you like.",
                      $"Half of {n} is {n / 2}, times ten is {n * 5}.",
                      "Yes."
                  ],
                  "Trust it now. Use it forever.", "yes or no")
            ],
            Recap = $"{n} × 5 = ({n} ÷ 2) × 10 = {n / 2} × 10 = {n * 5}\n\nHalve, then add a zero."
        };
    }

    private static WorkedProblem DoubleHalve(Random r, Flavor f)
    {
        int a = new[] { 14, 16, 18, 22, 24, 26 }[r.Next(6)];
        int b = new[] { 5, 15, 25, 35, 50 }[r.Next(5)];
        var prod = a * b;

        return new WorkedProblem
        {
            Setup = $"A hard multiplication can be swapped for an easy one.\nHalve one side, double the other. The answer does not change.\n\n{a} × {b}",
            Steps =
            [
                S($"Halve the {a}.",
                  $"What is half of {a}?", (a / 2).ToString(),
                  [
                      $"{a} ÷ 2.",
                      "Just split it.",
                      $"{a / 2}."
                  ],
                  "Now double the other one."),

                S($"Double the {b} to keep it fair.",
                  $"What is {b} × 2?", (b * 2).ToString(),
                  [
                      $"{b} + {b}.",
                      "Double it.",
                      $"{b * 2}."
                  ],
                  $"So now it's {a / 2} × {b * 2}, which is much nicer.", "a number",
                  "Halving one factor and doubling the other leaves the product unchanged.",
                  ["Halving one side and doubling the other halves the answer.",
                   "It only works when both numbers are even."]),

                S("Now do the easy version.",
                  $"What is {a / 2} × {b * 2}?", prod.ToString(),
                  [
                      $"{a / 2} × {b * 2}.",
                      "Much friendlier numbers.",
                      $"{prod}."
                  ],
                  $"Same answer as {a} × {b}. Less work.")
            ],
            Recap = $"{a} × {b} = {a / 2} × {b * 2} = {prod}\n\nHalve one, double the other. Free swap."
        };
    }

    // ================================================ unit 2 — the rules

    private static WorkedProblem DivisibleBy2510(Random r, Flavor f)
    {
        var n = P(r, 100, 999);
        var last = n % 10;

        return new WorkedProblem
        {
            Setup = $"You can tell if a number divides by 2, 5 or 10 without doing ANY dividing.\nYou only look at the last digit.\n\nTake {n}.",
            Steps =
            [
                S("Last digit only. Ignore everything else.",
                  $"What is the last digit of {n}?", last.ToString(),
                  [
                      "The one on the far right.",
                      $"{n}.",
                      $"{last}."
                  ],
                  "That digit decides all three."),

                S("Even last digit means the whole number is even.",
                  $"Does {n} divide by 2? (yes or no)", last % 2 == 0 ? "yes" : "no",
                  [
                      $"Is {last} even?",
                      last % 2 == 0 ? "It is, so the whole number is." : "It isn't, so the whole number isn't.",
                      last % 2 == 0 ? "Yes." : "No."
                  ],
                  "One glance, no division.", "yes or no",
                  "Every ten is even, so only the leftover last digit decides whether the whole number is.",
                  ["A number is even if all its digits are even.",
                   "You have to add the digits to know if it is even."]),

                S("Now 5 and 10, same glance.",
                  $"Does {n} divide by 5? (yes or no)", last is 0 or 5 ? "yes" : "no",
                  [
                      "Numbers dividing by 5 end in 0 or 5.",
                      $"This one ends in {last}.",
                      last is 0 or 5 ? "Yes." : "No."
                  ],
                  "Three rules, one digit.", "yes or no")
            ],
            Recap = $"{n} ends in {last}.\n  ÷2? {(last % 2 == 0 ? "yes" : "no")}   ÷5? {(last is 0 or 5 ? "yes" : "no")}   ÷10? {(last == 0 ? "yes" : "no")}\n\nLast digit tells you all three."
        };
    }

    private static WorkedProblem DigitSumRule(Random r, Flavor f)
    {
        var n = P(r, 3, 111) * 3;
        var ds = DigitSum(n);

        return new WorkedProblem
        {
            Setup = $"Here's a properly good one.\nTo test if a number divides by 3, ADD ITS DIGITS.\n\nTake {n}.",
            Steps =
            [
                S("Add every digit together.",
                  $"What do the digits of {n} add up to?", ds.ToString(),
                  [
                      $"Add them one at a time.",
                      $"{string.Join(" + ", n.ToString().Select(c => c.ToString()))}.",
                      $"{ds}."
                  ],
                  "Now test THAT little number instead."),

                S($"If {ds} divides by 3, then {n} does too.",
                  $"Does {ds} divide by 3? (yes or no)", ds % 3 == 0 ? "yes" : "no",
                  [
                      $"{ds} ÷ 3.",
                      ds % 3 == 0 ? "It goes exactly." : "It doesn't go exactly.",
                      ds % 3 == 0 ? "Yes." : "No."
                  ],
                  $"So {n} {(ds % 3 == 0 ? "does" : "does not")} divide by 3.", "yes or no",
                  "Every power of ten is one more than a multiple of three, so only the digit total matters.",
                  ["Adding digits works for every number you can think of.",
                   "It works because three is an odd number."]),

                S("Same trick, bigger version: if the digits add to a multiple of NINE, it divides by 9.",
                  $"The digits of {n} add to {ds}. Does {n} divide by 9? (yes or no)",
                  ds % 9 == 0 ? "yes" : "no",
                  [
                      $"Is {ds} a multiple of 9?",
                      ds % 9 == 0 ? $"{ds} ÷ 9 goes exactly." : $"{ds} ÷ 9 does not go exactly.",
                      ds % 9 == 0 ? "Yes." : "No."
                  ],
                  "One digit-add, two rules tested.", "yes or no")
            ],
            Recap = $"{n} → digits add to {ds}\n  ÷3? {(ds % 3 == 0 ? "yes" : "no")}    ÷9? {(ds % 9 == 0 ? "yes" : "no")}\n\nAdd the digits. Test the little number instead."
        };
    }

    private static WorkedProblem DivisibleBy4And6(Random r, Flavor f)
    {
        var n = P(r, 25, 249) * 4;
        var lastTwo = n % 100;
        var ds = DigitSum(n);

        return new WorkedProblem
        {
            Setup = $"Four and six have their own rules, and they're both quick.\n\nTake {n}.",
            Steps =
            [
                S("For 4, you only need the LAST TWO digits. Everything before them is hundreds, and hundreds always divide by 4.",
                  $"What are the last two digits of {n}?", lastTwo.ToString("00"),
                  [
                      "The final pair.",
                      $"{n}.",
                      $"{lastTwo:00}."
                  ],
                  "That's all you have to test.", "a number",
                  "One hundred divides by four, so only the leftover last two digits can decide it.",
                  ["Four is even so you only check the last digit.",
                   "You add the digits, the same as for three."],
                  lastTwo.ToString()),

                S($"Now test just that.",
                  $"Does {lastTwo:00} divide by 4? (yes or no)", lastTwo % 4 == 0 ? "yes" : "no",
                  [
                      $"{lastTwo} ÷ 4.",
                      lastTwo % 4 == 0 ? "It goes exactly." : "It doesn't.",
                      lastTwo % 4 == 0 ? "Yes." : "No."
                  ],
                  $"So {n} {(lastTwo % 4 == 0 ? "does" : "does not")}.", "yes or no"),

                S("Six is sneaky: it's just 2 AND 3 together. It has to pass both.",
                  $"{n} is even, and its digits add to {ds}. Does {n} divide by 6? (yes or no)",
                  ds % 3 == 0 ? "yes" : "no",
                  [
                      "It passes the 2 test already.",
                      $"Does {ds} divide by 3?",
                      ds % 3 == 0 ? "Both pass, so yes." : "The 3 test fails, so no."
                  ],
                  "Six = two tests, both must pass.", "yes or no")
            ],
            Recap = $"{n}: last two are {lastTwo:00} → ÷4 {(lastTwo % 4 == 0 ? "yes" : "no")}\ndigits add to {ds} → ÷6 {(ds % 3 == 0 ? "yes" : "no")}\n\n4 = last two digits. 6 = passes 2 and 3."
        };
    }

    private static WorkedProblem QuickFactor(Random r, Flavor f)
    {
        int a = P(r, 4, 12), b = P(r, 4, 12);
        var n = a * b;

        return new WorkedProblem
        {
            Setup = $"Now put the rules together to break a number apart fast.\n\nTake {n}.",
            Steps =
            [
                S("Run the quick tests first. Even?",
                  $"Does {n} divide by 2? (yes or no)", n % 2 == 0 ? "yes" : "no",
                  [
                      "Look at the last digit.",
                      n % 2 == 0 ? "It's even." : "It's odd.",
                      n % 2 == 0 ? "Yes." : "No."
                  ],
                  "One test down."),

                S("Digits.",
                  $"Do the digits of {n} add to a multiple of 3? (yes or no)",
                  DigitSum(n) % 3 == 0 ? "yes" : "no",
                  [
                      $"They add to {DigitSum(n)}.",
                      DigitSum(n) % 3 == 0 ? "Which is a multiple of 3." : "Which is not.",
                      DigitSum(n) % 3 == 0 ? "Yes." : "No."
                  ],
                  "Two tests down.", "yes or no",
                  "The quick rules narrow the search before you try dividing anything.",
                  ["The rules give you every factor immediately.",
                   "You have to test all numbers up to n to find a factor."]),

                S("Now find an actual pair.",
                  $"Give a number, other than 1 and {n}, that divides {n} exactly.",
                  Math.Min(a, b).ToString(),
                  [
                      "Use whichever test passed.",
                      $"Try {Math.Min(a, b)}.",
                      $"{Math.Min(a, b)} × {Math.Max(a, b)} = {n}."
                  ],
                  $"{Math.Min(a, b)} × {Math.Max(a, b)} = {n}. Found without guessing.",
                  "a number", "", null,
                  Math.Max(a, b).ToString())
            ],
            Recap = $"{n} = {Math.Min(a, b)} × {Math.Max(a, b)}\n\nRun the quick tests, then look. Don't guess."
        };
    }

    // ============================================== unit 3 — mental methods

    private static WorkedProblem MakeATen(Random r, Flavor f)
    {
        int a = P(r, 6, 9), b = P(r, 5, 9);
        var need = 10 - a;

        return new WorkedProblem
        {
            Setup = $"Your brain likes tens. Give it one.\n\n{a} + {b}",
            Steps =
            [
                S($"How far is {a} from 10? That's how much you're going to borrow.",
                  $"What is 10 − {a}?", need.ToString(),
                  [
                      $"10 − {a}.",
                      $"{a} needs a bit more to reach ten.",
                      $"{need}."
                  ],
                  $"Take {need} off the other number."),

                S($"Split the {b} into {need} and the rest.",
                  $"What is {b} − {need}?", (b - need).ToString(),
                  [
                      $"{b} − {need}.",
                      "That's the leftover after topping up to ten.",
                      $"{b - need}."
                  ],
                  $"So it's 10 + {b - need}.", "a number",
                  "Moving an amount from one number to the other keeps the total the same, and ten is easy to add to.",
                  ["Making a ten always makes the answer bigger.",
                   "You can only make a ten with even numbers."]),

                S("Now the easy sum.",
                  $"What is 10 + {b - need}?", (a + b).ToString(),
                  [
                      $"10 + {b - need}.",
                      "Adding to ten is instant.",
                      $"{a + b}."
                  ],
                  $"{a} + {b} = {a + b}, via a ten.")
            ],
            Recap = $"{a} + {b} = ({a} + {need}) + {b - need} = 10 + {b - need} = {a + b}\n\nTop one up to ten, take it off the other."
        };
    }

    private static WorkedProblem Compensate(Random r, Flavor f)
    {
        int a = P(r, 30, 90);
        int near = new[] { 19, 29, 39, 49, 98, 99 }[r.Next(6)];
        var round = near + 1;

        return new WorkedProblem
        {
            Setup = $"Adding an awkward number? Add a nice one instead, then fix it.\n\n{a} + {near}",
            Steps =
            [
                S($"{near} is nearly {round}. Add {round} instead — much easier.",
                  $"What is {a} + {round}?", (a + round).ToString(),
                  [
                      $"{a} + {round}.",
                      "Round numbers are quick.",
                      $"{a + round}."
                  ],
                  "But you added one too many."),

                S("So give it back.",
                  $"What is {a + round} − 1?", (a + near).ToString(),
                  [
                      $"{a + round} − 1.",
                      "You overshot by exactly one.",
                      $"{a + near}."
                  ],
                  $"{a} + {near} = {a + near}.", "a number",
                  "Adding too much and then taking the extra back leaves the total correct.",
                  ["Rounding up makes the answer approximately right.",
                   "You should round and then round again at the end."]),

                S("The bit that catches people: if you SUBTRACT an awkward number, the fix goes the other way.",
                  $"For {a} − {near}, you'd do {a} − {round} and then... add 1 or subtract 1?", "add 1",
                  [
                      "You took away one too many.",
                      "So you owe one back.",
                      "Add 1."
                  ],
                  "Adding: give it back. Subtracting: take it back.", "add 1 or subtract 1",
                  "", null, "add", "add one", "+1")
            ],
            Recap = $"{a} + {near} = {a} + {round} − 1 = {a + round} − 1 = {a + near}\n\nRound it, then fix it."
        };
    }

    private static WorkedProblem BreakApart(Random r, Flavor f)
    {
        int t = P(r, 2, 6), o = P(r, 2, 9), by = P(r, 3, 9);
        var n = t * 10 + o;

        return new WorkedProblem
        {
            Setup = $"Big multiplication? Break it into two small ones.\n\n{n} × {by}",
            Steps =
            [
                S($"Split {n} into {t * 10} and {o}.",
                  $"What is {t * 10} × {by}?", (t * 10 * by).ToString(),
                  [
                      $"{t} × {by} = {t * by}, then add the zero.",
                      "Tens are easy.",
                      $"{t * 10 * by}."
                  ],
                  "Big chunk done."),

                S("Now the small chunk.",
                  $"What is {o} × {by}?", (o * by).ToString(),
                  [
                      $"{o} × {by}.",
                      "Straight from the times table.",
                      $"{o * by}."
                  ],
                  "Both pieces ready.", "a number",
                  "Multiplication distributes over addition, so splitting a number and multiplying each part gives the same total.",
                  ["Splitting a number splits the answer in half too.",
                   "You can only split numbers that end in zero."]),

                S("Add the two pieces.",
                  $"What is {n} × {by}?", (n * by).ToString(),
                  [
                      $"{t * 10 * by} + {o * by}.",
                      "Add your two chunks.",
                      $"{n * by}."
                  ],
                  "That's the whole method, and it works on any size.")
            ],
            Recap = $"{n} × {by} = {t * 10} × {by} + {o} × {by} = {t * 10 * by} + {o * by} = {n * by}"
        };
    }

    private static WorkedProblem EstimateFirst(Random r, Flavor f)
    {
        int a = P(r, 21, 89), b = P(r, 21, 89);
        var exact = a * b;
        var ra = (int)Math.Round(a / 10.0) * 10;
        var rb = (int)Math.Round(b / 10.0) * 10;
        var est = ra * rb;

        return new WorkedProblem
        {
            Setup = $"Estimate BEFORE you calculate. Then you'll know if your answer is nonsense.\n\n{a} × {b}",
            Steps =
            [
                S("Round both to the nearest ten.",
                  $"What does {a} round to?", ra.ToString(),
                  [
                      "Nearest ten.",
                      $"{a} is closest to {ra}.",
                      $"{ra}."
                  ],
                  "One rounded."),

                S("And the other.",
                  $"What does {b} round to?", rb.ToString(),
                  [
                      "Nearest ten.",
                      $"{b} is closest to {rb}.",
                      $"{rb}."
                  ],
                  "Now multiply the easy ones."),

                S("This is your rough answer. It should be near the real one.",
                  $"What is {ra} × {rb}?", est.ToString(),
                  [
                      $"{ra / 10} × {rb / 10}, then two zeros.",
                      $"{ra / 10 * (rb / 10)} then 00.",
                      $"{est}."
                  ],
                  $"So the real answer is around {est}.", "a number",
                  "An estimate gives you a range, so an answer far outside it must be wrong.",
                  ["An estimate gives the exact answer faster.",
                   "Estimating replaces the need to calculate."]),

                S("Now the real one, and see how close you were.",
                  $"What is {a} × {b}?", exact.ToString(),
                  [
                      "Break it apart if you like.",
                      $"Around {est}, remember.",
                      $"{exact}."
                  ],
                  $"Estimate {est}, real {exact}. If you'd got {exact * 10}, you'd now KNOW it was wrong.")
            ],
            Recap = $"Estimate: {ra} × {rb} = {est}\nActual: {a} × {b} = {exact}\n\nEstimate first. It's your alarm bell."
        };
    }

    // ========================================= unit 4 — fractions & percent

    private static WorkedProblem Benchmarks(Random r, Flavor f)
    {
        (int n, int d)[] fr = [(3, 8), (5, 8), (2, 5), (3, 5), (5, 12), (7, 12), (4, 9), (5, 9)];
        var (n, dd) = fr[r.Next(fr.Length)];
        var half = dd / 2.0;
        var bigger = n > half;

        return new WorkedProblem
        {
            Setup = $"You don't need a calculator to compare fractions.\nYou need ONE landmark: a half.\n\nTake {n}/{dd}.",
            Steps =
            [
                S($"Half of the bottom number is your landmark.",
                  $"What is half of {dd}?", half.ToString("0.##"),
                  [
                      $"{dd} ÷ 2.",
                      "That's what the top would be for exactly a half.",
                      $"{half:0.##}."
                  ],
                  "That's your marker."),

                S($"Now compare the actual top, {n}, to it.",
                  $"Is {n}/{dd} bigger than a half? (yes or no)", bigger ? "yes" : "no",
                  [
                      $"Is {n} more than {half:0.##}?",
                      bigger ? "It is." : "It isn't.",
                      bigger ? "Yes." : "No."
                  ],
                  "No calculator. No common denominators.", "yes or no",
                  "Comparing the top to half the bottom tells you which side of a half the fraction sits.",
                  ["A fraction is over a half whenever the top is bigger than 1.",
                   "You must always find a common denominator to compare."]),

                S("Now use it for something real.",
                  $"Is {n}/{dd} closer to 0, to a half, or to 1?",
                  Math.Abs(n / (double)dd - 0.5) < 0.2 ? "a half" : (n / (double)dd > 0.7 ? "1" : (n / (double)dd < 0.3 ? "0" : "a half")),
                  [
                      $"{n}/{dd} is about {n / (double)dd:0.##}.",
                      "Which landmark is it nearest?",
                      $"About {n / (double)dd:0.##}, so nearest " +
                      (Math.Abs(n / (double)dd - 0.5) < 0.2 ? "a half" : (n / (double)dd > 0.7 ? "1" : "0")) + "."
                  ],
                  "Landmarks beat calculators for speed.", "0, a half, or 1",
                  "", null, "half", "0.5", "1/2")
            ],
            Recap = $"Half of {dd} is {half:0.##}. The top is {n}.\nSo {n}/{dd} is {(bigger ? "more" : "less")} than a half."
        };
    }

    private static WorkedProblem SameThreeWays(Random r, Flavor f)
    {
        (int n, int d, string dec, int pct)[] set =
        [
            (1, 2, "0.5", 50), (1, 4, "0.25", 25), (3, 4, "0.75", 75), (1, 5, "0.2", 20),
            (2, 5, "0.4", 40), (3, 5, "0.6", 60), (4, 5, "0.8", 80), (1, 10, "0.1", 10),
            (3, 10, "0.3", 30), (7, 10, "0.7", 70)
        ];
        var s = set[r.Next(set.Length)];

        return new WorkedProblem
        {
            Setup = $"A fraction, a decimal and a percent can be the SAME number wearing different clothes.\n\nTake {s.n}/{s.d}.",
            Steps =
            [
                S("A fraction is a division sign in disguise. Top divided by bottom.",
                  $"What is {s.n} ÷ {s.d}, as a decimal?", s.dec,
                  [
                      $"{s.n} ÷ {s.d}.",
                      "The line means divide.",
                      $"{s.dec}."
                  ],
                  "Same number, decimal clothes.", "a decimal",
                  "The fraction line means divide, so every fraction is a division waiting to happen.",
                  ["A fraction becomes a decimal by putting a point in front.",
                   "You divide the bottom by the top."]),

                S("Percent means out of a hundred, so multiply the decimal by 100.",
                  $"What is {s.dec} as a percent? (just the number)", s.pct.ToString(),
                  [
                      $"{s.dec} × 100.",
                      "Move the point two places right.",
                      $"{s.pct}%."
                  ],
                  "Three outfits, one number."),

                S("Now the useful direction.",
                  $"What is {s.pct}% of 200?", (200 * s.pct / 100).ToString(),
                  [
                      $"{s.n}/{s.d} of 200.",
                      $"Or {s.dec} × 200.",
                      $"{200 * s.pct / 100}."
                  ],
                  "Pick whichever outfit makes the sum easiest. That's the skill.")
            ],
            Recap = $"{s.n}/{s.d} = {s.dec} = {s.pct}%\n\nSame number. Use whichever form is easiest."
        };
    }

    private static WorkedProblem TenPercentTrick(Random r, Flavor f)
    {
        var amount = P(r, 4, 40) * 10;

        return new WorkedProblem
        {
            Setup = $"Ten per cent is the easiest number in maths.\nMove the point one place left. Done.\n\nTake {amount}.",
            Steps =
            [
                S("Move the decimal point one to the left.",
                  $"What is 10% of {amount}?", (amount / 10).ToString(),
                  [
                      $"{amount} ÷ 10.",
                      "Point moves one left.",
                      $"{amount / 10}."
                  ],
                  "Now build everything else from it.", "a number",
                  "Ten per cent is one tenth, and dividing by ten moves every digit one place down.",
                  ["Ten per cent means taking ten off the number.",
                   "You move the point one place to the right."]),

                S("Twenty per cent is just double it.",
                  $"What is 20% of {amount}?", (amount / 5).ToString(),
                  [
                      $"{amount / 10} × 2.",
                      "Two lots of ten per cent.",
                      $"{amount / 5}."
                  ],
                  "Half of ten per cent gives you 5% too."),

                S("Now a proper one, built from pieces.",
                  $"What is 35% of {amount}? (30% plus 5%)", (amount * 35 / 100).ToString(),
                  [
                      $"10% is {amount / 10}, so 30% is {amount * 3 / 10}.",
                      $"5% is half of 10%, so {amount / 20}.",
                      $"{amount * 3 / 10} + {amount / 20} = {amount * 35 / 100}."
                  ],
                  "Any percent, built out of tens and halves. No calculator.")
            ],
            Recap = $"10% of {amount} = {amount / 10}\n20% = {amount / 5}   5% = {amount / 20}   35% = {amount * 35 / 100}\n\nBuild every percent out of 10% and 5%."
        };
    }

    private static WorkedProblem TipMath(Random r, Flavor f)
    {
        var bill = P(r, 12, 60) * 2;

        return new WorkedProblem
        {
            Setup = $"Real one. The ramen bill is ${bill} and you want to leave 20%.\nNo phone. Do it in your head.",
            Steps =
            [
                S("Ten per cent first. Always ten per cent first.",
                  $"What is 10% of {bill}?", (bill / 10.0).ToString("0.##"),
                  [
                      $"{bill} ÷ 10.",
                      "Point one to the left.",
                      $"{bill / 10.0:0.##}."
                  ],
                  "Halfway there already.", "a number"),

                S("Twenty per cent is two of those.",
                  $"What is the tip?", (bill / 5.0).ToString("0.##"),
                  [
                      $"{bill / 10.0:0.##} × 2.",
                      "Double the ten per cent.",
                      $"{bill / 5.0:0.##}."
                  ],
                  $"${bill / 5.0:0.##} tip.", "a number",
                  "Twenty per cent is two lots of ten per cent, and ten per cent is a point move.",
                  ["Twenty per cent is found by dividing by twenty.",
                   "You must multiply the bill by 0.2 on a calculator to be accurate."]),

                S("And the total.",
                  $"What do you pay altogether?", (bill + bill / 5.0).ToString("0.##"),
                  [
                      $"{bill} + {bill / 5.0:0.##}.",
                      "Bill plus tip.",
                      $"{bill + bill / 5.0:0.##}."
                  ],
                  "You just did that faster than anyone could unlock a phone.")
            ],
            Recap = $"10% = {bill / 10.0:0.##}, tip = {bill / 5.0:0.##}, total = ${bill + bill / 5.0:0.##}"
        };
    }

    // ============================================ unit 5 — seventh grade

    private static WorkedProblem Negatives(Random r, Flavor f)
    {
        int a = P(r, -12, -2), b = P(r, 3, 15);

        return new WorkedProblem
        {
            Setup = $"Negatives are just a number line with a left-hand side.\nThat's genuinely all they are.\n\n{a} + {b}",
            Steps =
            [
                S($"Start at {a}. That's to the LEFT of zero.",
                  $"Adding {b} moves you which way — left or right?", "right",
                  [
                      "Adding always moves right.",
                      "Taking away moves left.",
                      "Right."
                  ],
                  "Adding goes right. Always.", "left or right",
                  "Adding moves in the positive direction regardless of where you start.",
                  ["Adding to a negative moves further left.",
                   "The direction depends on which number is bigger."]),

                S($"So move {b} to the right from {a}.",
                  $"Where do you land?", (a + b).ToString(),
                  [
                      $"From {a}, count {b} to the right.",
                      $"You pass zero after {Math.Abs(a)} steps.",
                      $"{a + b}."
                  ],
                  $"{a + b}."),

                S("Now the one that trips everyone.",
                  $"What is {a} − ({b})?", (a - b).ToString(),
                  [
                      "Taking away moves LEFT.",
                      $"From {a}, go {b} further left.",
                      $"{a - b}."
                  ],
                  "Left for minus, right for plus. That's the whole thing.")
            ],
            Recap = $"{a} + {b} = {a + b}   (right)\n{a} − {b} = {a - b}   (left)\n\nIt's a number line. Walk it."
        };
    }

    private static WorkedProblem Expressions(Random r, Flavor f)
    {
        int m = P(r, 2, 8), b = P(r, 1, 15), x = P(r, 2, 10);

        return new WorkedProblem
        {
            Setup = $"A letter is just a number nobody's told you yet.\n\nWork out {m}x + {b} when x = {x}.",
            Steps =
            [
                S($"Swap the x for {x}. That's all 'substitute' means.",
                  $"What is {m} × {x}?", (m * x).ToString(),
                  [
                      $"{m} × {x}.",
                      "The letter is just that number now.",
                      $"{m * x}."
                  ],
                  "Multiplying part done."),

                S("Now add the rest.",
                  $"What is {m * x} + {b}?", (m * x + b).ToString(),
                  [
                      $"{m * x} + {b}.",
                      "Nearly there.",
                      $"{m * x + b}."
                  ],
                  $"{m}x + {b} = {m * x + b} when x = {x}.", "a number",
                  "The letter stands for one particular number, so replacing it makes an ordinary sum.",
                  ["The letter means you multiply everything together.",
                   "A letter always stands for the number 1 unless told otherwise."]),

                S("Test that you actually get it.",
                  $"What would it be if x = 0?", b.ToString(),
                  [
                      $"{m} × 0 = 0.",
                      $"So you're left with just the {b}.",
                      $"{b}."
                  ],
                  "Zero kills the x term. Useful to know.")
            ],
            Recap = $"{m}x + {b} at x = {x} → {m * x} + {b} = {m * x + b}\nAt x = 0 → {b}"
        };
    }

    private static WorkedProblem OneStep(Random r, Flavor f)
    {
        int x = P(r, 3, 20), b = P(r, 2, 20);
        var total = x + b;

        return new WorkedProblem
        {
            Setup = $"An equation is a balance. Whatever you do to one side, do to the other.\n\nx + {b} = {total}",
            Steps =
            [
                S($"x has a +{b} stuck to it. Get rid of it by doing the opposite.",
                  $"What is the opposite of adding {b}?", $"subtracting {b}",
                  [
                      "The opposite of adding.",
                      $"Taking {b} away.",
                      $"Subtracting {b}."
                  ],
                  "Opposites undo things.", "in words",
                  "Every operation has an inverse, and applying it to both sides keeps the balance.",
                  ["The opposite of adding is multiplying.",
                   "You can remove a number by moving it across unchanged."],
                  $"-{b}", $"minus {b}", "subtract", "subtracting", "taking away"),

                S($"Do it to BOTH sides.",
                  $"What is {total} − {b}?", x.ToString(),
                  [
                      $"{total} − {b}.",
                      "The left side becomes just x.",
                      $"{x}."
                  ],
                  $"x = {x}."),

                S("Check it. Every time. It takes three seconds and it catches everything.",
                  $"What is {x} + {b}?", total.ToString(),
                  [
                      $"{x} + {b}.",
                      "Should be what you started with.",
                      $"{total}."
                  ],
                  "Checked. Now you KNOW, you don't hope.")
            ],
            Recap = $"x + {b} = {total}  →  x = {x}\nCheck: {x} + {b} = {total} ✓"
        };
    }

    private static WorkedProblem TwoStep(Random r, Flavor f)
    {
        int m = P(r, 2, 8), x = P(r, 2, 12), b = P(r, 2, 20);
        var total = m * x + b;

        return new WorkedProblem
        {
            Setup = $"Two things stuck to the x now. Undo them in the OPPOSITE order to how they went on.\n\n{m}x + {b} = {total}",
            Steps =
            [
                S($"The +{b} went on last, so it comes off first. Like taking your shoes off before your socks.",
                  $"What is {total} − {b}?", (m * x).ToString(),
                  [
                      $"{total} − {b}.",
                      $"Now it reads {m}x = {m * x}.",
                      $"{m * x}."
                  ],
                  "Shoes off.", "a number",
                  "Operations are undone in reverse order, the same way you take off what you put on last.",
                  ["You always undo multiplication first.",
                   "The order of undoing makes no difference to the answer."]),

                S($"Now the ×{m}.",
                  $"What is {m * x} ÷ {m}?", x.ToString(),
                  [
                      $"{m * x} ÷ {m}.",
                      $"{m} × ? = {m * x}.",
                      $"x = {x}."
                  ],
                  $"x = {x}. Socks off."),

                S("Check.",
                  $"What is {m} × {x} + {b}?", total.ToString(),
                  [
                      $"{m} × {x} = {m * x}.",
                      $"Plus {b}.",
                      $"{total}."
                  ],
                  "Back where you started, so it's right.")
            ],
            Recap = $"{m}x + {b} = {total}\n  −{b}:  {m}x = {m * x}\n  ÷{m}:  x = {x}\n  check ✓\n\nUndo backwards. Shoes before socks."
        };
    }

    // ============================================ unit 6 — how to learn

    private static WorkedProblem CheckByEstimate(Random r, Flavor f)
    {
        int a = P(r, 18, 79), b = P(r, 12, 49);
        var right = a * b;
        var wrong = right * 10;

        return new WorkedProblem
        {
            Setup = $"Someone says {a} × {b} = {wrong}.\nYou don't have to redo it to know they're wrong.",
            Steps =
            [
                S("Round both numbers and multiply the easy ones.",
                  $"About what should {a} × {b} be? Round to tens and multiply.",
                  ((int)Math.Round(a / 10.0) * 10 * ((int)Math.Round(b / 10.0) * 10)).ToString(),
                  [
                      $"{a} ≈ {(int)Math.Round(a / 10.0) * 10}, {b} ≈ {(int)Math.Round(b / 10.0) * 10}.",
                      "Multiply those.",
                      $"{(int)Math.Round(a / 10.0) * 10 * ((int)Math.Round(b / 10.0) * 10)}."
                  ],
                  "That's roughly where the answer lives."),

                S($"They claimed {wrong}. Yours is about ten times smaller.",
                  $"Is {wrong} a sensible answer? (yes or no)", "no",
                  [
                      "Compare it to your estimate.",
                      "It's about ten times too big.",
                      "No."
                  ],
                  "You spotted an error without doing the sum.", "yes or no",
                  "An estimate sets the size of the answer, so anything far outside it must be wrong.",
                  ["An estimate proves the exact answer.",
                   "An answer is only wrong if it is odd rather than even."]),

                S("Now find the actual answer.",
                  $"What is {a} × {b}?", right.ToString(),
                  [
                      "Break it apart if that helps.",
                      $"Should be near your estimate.",
                      $"{right}."
                  ],
                  "Estimate, then calculate, then compare. That's the loop.")
            ],
            Recap = $"Estimate first, calculate second, compare third.\n{a} × {b} = {right}, not {wrong}."
        };
    }

    private static WorkedProblem CheckBackwards(Random r, Flavor f)
    {
        int by = P(r, 3, 9), q = P(r, 12, 60);
        var total = by * q;

        return new WorkedProblem
        {
            Setup = $"The best way to check a division is to multiply it back.\n\n{total} ÷ {by}",
            Steps =
            [
                S("Do the division first.",
                  $"What is {total} ÷ {by}?", q.ToString(),
                  [
                      $"How many {by}s in {total}?",
                      $"{by} × ? = {total}.",
                      $"{q}."
                  ],
                  "Now don't trust it yet."),

                S("Multiply your answer back by what you divided by.",
                  $"What is {q} × {by}?", total.ToString(),
                  [
                      $"{q} × {by}.",
                      "Should land on the original.",
                      $"{total}."
                  ],
                  "Same number you started with. Now you can trust it.", "a number",
                  "Multiplying and dividing undo each other, so going back must return the original number.",
                  ["Checking backwards works only for small numbers.",
                   "You should check by dividing a second time."]),

                S("This works everywhere. Which operation checks a subtraction?",
                  "Adding or dividing?", "adding",
                  [
                      "What undoes taking away?",
                      "Putting it back.",
                      "Adding."
                  ],
                  "Every operation has an undo. Use it as your check.", "adding or dividing",
                  "", null, "add", "addition")
            ],
            Recap = $"{total} ÷ {by} = {q}, and {q} × {by} = {total} ✓\n\nCheck by going backwards. It never lies."
        };
    }

    private static WorkedProblem FindTheMistake(Random r, Flavor f)
    {
        int m = P(r, 2, 8), x = P(r, 3, 12), b = P(r, 3, 18);
        var total = m * x + b;
        var wrongX = total / m;

        return new WorkedProblem
        {
            Setup = $"Someone solved {m}x + {b} = {total} and got x = {wrongX}.\nThat's wrong. Your job is to find WHERE it went wrong.",
            Steps =
            [
                S("First, prove it's wrong by testing their answer.",
                  $"What is {m} × {wrongX} + {b}?", (m * wrongX + b).ToString(),
                  [
                      $"{m} × {wrongX} = {m * wrongX}.",
                      $"Plus {b}.",
                      $"{m * wrongX + b}."
                  ],
                  $"That's not {total}, so they're definitely wrong."),

                S("Now find the actual mistake. They divided first instead of subtracting first.",
                  $"What should they have done to {total} FIRST?", $"subtract {b}",
                  [
                      "Undo the thing that was done last.",
                      $"The +{b} went on last.",
                      $"Subtract {b}."
                  ],
                  "They undid it in the wrong order.", "in words",
                  "Undoing in the wrong order changes the answer, because each operation applies to everything before it.",
                  ["They used the wrong times table.",
                   "The mistake was copying the numbers down wrong."],
                  $"-{b}", $"minus {b}", "subtract", "subtracting", $"take away {b}"),

                S("Now do it properly.",
                  "What is x really?", x.ToString(),
                  [
                      $"{total} − {b} = {m * x}.",
                      $"{m * x} ÷ {m}.",
                      $"x = {x}."
                  ],
                  "Finding someone else's mistake is how you stop making it.")
            ],
            Recap = $"Wrong: divided first, got {wrongX}.\nRight: −{b} then ÷{m}, giving x = {x}.\n\nWrong ORDER, not wrong arithmetic."
        };
    }

    private static WorkedProblem ExplainIt(Random r, Flavor f)
    {
        int a = P(r, 12, 40), b = P(r, 12, 40);

        return new WorkedProblem
        {
            Setup = $"Last one, and it's the most important.\nIf you can't explain it, you don't know it yet.\n\n{a} + {b}",
            Steps =
            [
                S("Do it first.",
                  $"What is {a} + {b}?", (a + b).ToString(),
                  [
                      $"{a} + {b}.",
                      "Break it into tens and ones if you like.",
                      $"{a + b}."
                  ],
                  "Now the harder half."),

                S($"Swap them round: {b} + {a}.",
                  "What do you get?", (a + b).ToString(),
                  [
                      $"{b} + {a}.",
                      "Same two numbers, other order.",
                      $"{a + b}."
                  ],
                  "Same answer. Now WHY?", "a number",
                  "Addition combines the same collection of things, and the order you count them in cannot change how many there are.",
                  ["Addition is reversible because subtraction exists.",
                   "The order works out the same only for two-digit numbers."]),

                S("One more. Does the same swap work for subtraction?",
                  $"Is {a} − {b} the same as {b} − {a}? (yes or no)", "no",
                  [
                      "Try it with 10 and 3.",
                      "10 − 3 = 7, but 3 − 10 = −7.",
                      "No — order matters for subtraction."
                  ],
                  "Knowing where a rule STOPS is knowing the rule.", "yes or no")
            ],
            Recap = $"{a} + {b} = {b} + {a} = {a + b}\nBut {a} − {b} ≠ {b} − {a}\n\nIf you can say why, it's yours."
        };
    }
}
