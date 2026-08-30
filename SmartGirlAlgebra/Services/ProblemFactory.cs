using SmartGirlAlgebra.Models;

namespace SmartGirlAlgebra.Services;

/// <summary>
/// Builds a problem from scratch every time it is asked.
///
/// The old lessons were a fixed list of questions with four choices each. Two
/// things follow from that and both were happening: a child can guess one in
/// four, and a child who sees the same twelve questions repeatedly learns the
/// ANSWERS rather than the method.
///
/// So nothing here is stored. Every problem is generated with new numbers, and
/// every step has to be typed. There is nothing to memorise and nothing to
/// guess at.
/// </summary>
public static class ProblemFactory
{
    public static WorkedProblem Make(int skillId, Random r, Flavor f)
    {
        // Each course has its own file; identical rules, different maths.
        if (skillId >= 2000) return AccelFactory.Make(skillId, r, f);
        if (skillId >= 1000) return AlgebraFactory.Make(skillId, r, f);

        return Build(skillId, r, f);
    }

    private static WorkedProblem Build(int skillId, Random r, Flavor f) => skillId switch
    {
        101 => PlaceValue(r, f),
        102 => BigAddition(r, f),
        103 => BigSubtraction(r, f),
        104 => TimesOneDigit(r, f),

        201 => Factors(r, f),
        202 => Multiples(r, f),
        203 => TwoByTwo(r, f),
        204 => LongDivision(r, f),

        301 => EqualFractions(r, f),
        302 => Simplify(r, f),
        303 => CompareFractions(r, f),
        304 => SameBottom(r, f),

        401 => UnlikeBottoms(r, f),
        402 => MultiplyFractions(r, f),
        403 => DecimalPlace(r, f),
        404 => AddDecimals(r, f),

        501 => MultiplyDecimals(r, f),
        502 => DivideDecimal(r, f),
        503 => OrderOfOperations(r, f),
        504 => PowersOfTen(r, f),

        601 => EvaluateExpression(r, f),
        602 => WordsIntoMaths(r, f),
        603 => OneStep(r, f),
        604 => TwoStep(r, f),

        _ => PlaceValue(r, f)
    };

    // ================================================================ helpers

    private static int Pick(Random r, int lo, int hi) => r.Next(lo, hi + 1);

    private static int Gcd(int a, int b) { while (b != 0) (a, b) = (b, a % b); return Math.Abs(a); }

    private static int Lcm(int a, int b) => a / Gcd(a, b) * b;

    private static string N(int n) => n.ToString("N0");

    private static TypedStep Step(string say, string ask, string answer, string[] hints,
                                  string yes, string placeholder = "a number", params string[] accept)
        => new()
        {
            Say = say, Ask = ask, Answer = answer, Hints = hints,
            Yes = yes, Placeholder = placeholder, Accept = accept
        };

    // ============================================================== unit one

    private static WorkedProblem PlaceValue(Random r, Flavor f)
    {
        var digits = new int[5];
        for (var i = 0; i < 5; i++) digits[i] = Pick(r, 1, 9);
        var number = int.Parse(string.Concat(digits));

        // Which digit are we asking about? Not the first, so it's a real question.
        var slot = Pick(r, 1, 4);
        var digit = digits[slot];
        var placeValue = digit * (int)Math.Pow(10, 4 - slot);
        var placeName = (4 - slot) switch
        {
            0 => "ones", 1 => "tens", 2 => "hundreds", 3 => "thousands", _ => "ten thousands"
        };

        return new WorkedProblem
        {
            Setup = $"{N(number)} people came to {f.Event} this year.\n\nLook at the digit {digit}.",
            Steps =
            [
                Step($"Every digit sits in a place, and the place decides what it is worth.",
                     $"Which place is the {digit} in? (ones, tens, hundreds, thousands, ten thousands)",
                     placeName,
                     [
                         "Count the places from the RIGHT, starting at ones.",
                         "ones, tens, hundreds, thousands, ten thousands — going left.",
                         $"The {digit} is in the {placeName} place."
                     ],
                     $"Yes — the {placeName} place.", "a place name"),

                Step("Now what is it actually worth?",
                     $"What is the VALUE of that {digit}?",
                     placeValue.ToString(),
                     [
                         $"The digit is {digit}, and it is in the {placeName} place.",
                         $"So it means {digit} lots of {(int)Math.Pow(10, 4 - slot)}.",
                         $"{digit} × {(int)Math.Pow(10, 4 - slot)} = {placeValue}."
                     ],
                     $"Right — it is worth {N(placeValue)}, not {digit}.")
            ],
            Recap = $"In {N(number)}, the {digit} sits in the {placeName} place, so it is worth {N(placeValue)}."
        };
    }

    private static WorkedProblem BigAddition(Random r, Flavor f)
    {
        // Force a carry in the ones so the method actually gets used.
        int a1 = Pick(r, 5, 9), b1 = Pick(r, 10 - a1, 9);
        int a2 = Pick(r, 1, 9), b2 = Pick(r, 1, 9);
        int a3 = Pick(r, 1, 8), b3 = Pick(r, 1, 8);

        var a = a3 * 100 + a2 * 10 + a1;
        var b = b3 * 100 + b2 * 10 + b1;
        var ones = a1 + b1;
        var carried = ones / 10;
        var tens = a2 + b2 + carried;

        return new WorkedProblem
        {
            Setup = $"{f.Place} sold {N(a)} {f.Things} in the morning\nand {N(b)} more in the afternoon.",
            Steps =
            [
                Step("Start on the right, with the ones. Never the left.",
                     $"What is {a1} + {b1}?",
                     ones.ToString(),
                     [
                         "Just the last digit of each number.",
                         $"{a1} + {b1}.",
                         $"{a1} + {b1} = {ones}."
                     ],
                     $"Yes. That is more than 9, so a ten gets carried."),

                Step($"{ones} does not fit in one column, so the {carried} ten goes over to the tens.",
                     $"Now add the tens including the carry: {a2} + {b2} + {carried}?",
                     tens.ToString(),
                     [
                         "Add the two tens digits, then add the carried one.",
                         $"{a2} + {b2} = {a2 + b2}, and one more carried.",
                         $"{a2} + {b2} + {carried} = {tens}."
                     ],
                     "Good — carried and counted."),

                Step("Last column. Add the hundreds, and the carry if there was one.",
                     $"What is the whole answer, {N(a)} + {N(b)}?",
                     (a + b).ToString(),
                     [
                         $"You have the ones and the tens already.",
                         $"Hundreds: {a3} + {b3}{(tens >= 10 ? " + 1 carried" : "")}.",
                         $"{N(a)} + {N(b)} = {N(a + b)}."
                     ],
                     "That is the whole thing.", "a number", N(a + b))
            ],
            Recap = $"{N(a)} + {N(b)} = {N(a + b)}.\nRight column first, carry when it will not fit."
        };
    }

    private static WorkedProblem BigSubtraction(Random r, Flavor f)
    {
        // Force a borrow in the ones.
        int a1 = Pick(r, 0, 4), b1 = Pick(r, a1 + 1, 9);
        int a2 = Pick(r, 2, 9), b2 = Pick(r, 1, a2 - 1);
        int a3 = Pick(r, 3, 9), b3 = Pick(r, 1, a3 - 1);

        var a = a3 * 100 + a2 * 10 + a1;
        var b = b3 * 100 + b2 * 10 + b1;
        var ones = a1 + 10 - b1;
        var tensTop = a2 - 1;

        return new WorkedProblem
        {
            Setup = $"There were {N(a)} {f.Things} ready for {f.Event}.\n{N(b)} of them were used.",
            Steps =
            [
                Step($"Ones first. You cannot take {b1} from {a1}, so borrow a ten.",
                     $"After borrowing, what is {a1} + 10 − {b1}?",
                     ones.ToString(),
                     [
                         $"{a1} is too small, so borrow: it becomes {a1 + 10}.",
                         $"{a1 + 10} − {b1}.",
                         $"{a1 + 10} − {b1} = {ones}."
                     ],
                     "Yes. Borrowing is just moving a ten across."),

                Step($"The tens gave one away, so the {a2} is now {tensTop}.",
                     $"What is {tensTop} − {b2}?",
                     (tensTop - b2).ToString(),
                     [
                         "The tens digit dropped by one when you borrowed.",
                         $"{tensTop} − {b2}.",
                         $"{tensTop} − {b2} = {tensTop - b2}."
                     ],
                     "Good — you remembered the borrow."),

                Step("Now the hundreds, and then read off the whole answer.",
                     $"What is {N(a)} − {N(b)}?",
                     (a - b).ToString(),
                     [
                         $"Hundreds: {a3} − {b3} = {a3 - b3}.",
                         $"Put the three columns together.",
                         $"{N(a)} − {N(b)} = {N(a - b)}."
                     ],
                     "That is it.", "a number", N(a - b))
            ],
            Recap = $"{N(a)} − {N(b)} = {N(a - b)}.\nWhen the top digit is too small, borrow a ten from next door."
        };
    }

    private static WorkedProblem TimesOneDigit(Random r, Flavor f)
    {
        var tens = Pick(r, 2, 9);
        var ones = Pick(r, 2, 9);
        var by = Pick(r, 3, 9);
        var big = tens * 10 + ones;

        return new WorkedProblem
        {
            Setup = $"Each {f.Box} at {f.Place} holds {big} {f.Things}.\nThere are {by} {f.Boxes}.",
            Steps =
            [
                Step($"Break {big} into {tens * 10} and {ones}. Easier in two pieces than one.",
                     $"What is {tens * 10} × {by}?",
                     (tens * 10 * by).ToString(),
                     [
                         $"{tens} × {by} first, then put the zero back.",
                         $"{tens} × {by} = {tens * by}.",
                         $"So {tens * 10} × {by} = {tens * 10 * by}."
                     ],
                     "That is the big piece."),

                Step("Now the small piece.",
                     $"What is {ones} × {by}?",
                     (ones * by).ToString(),
                     [
                         "Just the ones digit times the number.",
                         $"{ones} × {by}.",
                         $"{ones} × {by} = {ones * by}."
                     ],
                     "Good."),

                Step("Two pieces make the whole.",
                     $"Add them: {tens * 10 * by} + {ones * by}?",
                     (big * by).ToString(),
                     [
                         "Add the two answers you just worked out.",
                         $"{tens * 10 * by} + {ones * by}.",
                         $"{big} × {by} = {big * by}."
                     ],
                     $"So {big} × {by} = {big * by}.", "a number", N(big * by))
            ],
            // Written as a proper chain. "a = b plus c = d" puts an equals sign
            // between things that are not equal, which is exactly the habit she
            // is about to need for equations.
            Recap = $"{big} × {by}\n = {tens * 10} × {by} + {ones} × {by}\n = {tens * 10 * by} + {ones * by}\n = {big * by}\n\nSplitting the number makes it easier, and it is allowed."
        };
    }

    // ============================================================== unit two

    private static WorkedProblem Factors(Random r, Flavor f)
    {
        int[] pool = [12, 18, 20, 24, 28, 30, 36, 40, 42, 45, 48, 50, 54, 56, 60];
        var n = pool[r.Next(pool.Length)];
        var factors = Enumerable.Range(1, n).Where(i => n % i == 0).ToArray();
        var pair = Pick(r, 2, Math.Max(2, factors.Length - 2));
        var some = factors[pair];

        return new WorkedProblem
        {
            Setup = $"{n} {f.Things} have to go into equal {f.Boxes},\nwith none left over.",
            Steps =
            [
                Step("A factor is a number that divides in with nothing left over.",
                     $"Does {some} divide into {n} with nothing left over? (yes or no)",
                     "yes",
                     [
                         $"Try it: {n} ÷ {some}.",
                         $"If it comes out whole, it is a factor.",
                         $"{n} ÷ {some} = {n / some}, so yes."
                     ],
                     "Yes — so it is a factor.", "yes or no"),

                Step("Every factor has a partner it multiplies with.",
                     $"What do you multiply {some} by to get {n}?",
                     (n / some).ToString(),
                     [
                         $"{some} × what = {n}?",
                         $"That is the same as {n} ÷ {some}.",
                         $"{some} × {n / some} = {n}."
                     ],
                     $"Right — {some} and {n / some} are a pair."),

                Step("Now the whole list, smallest first, separated by commas.",
                     $"What are ALL the factors of {n}?",
                     string.Join(", ", factors),
                     [
                         "Start at 1 and work up, testing each one.",
                         $"1 and {n} are always factors of {n}.",
                         $"They are: {string.Join(", ", factors)}."
                     ],
                     "That is the full set.", "numbers with commas",
                     string.Join(",", factors), string.Join(" ", factors))
            ],
            Recap = $"The factors of {n} are {string.Join(", ", factors)}.\nThey come in pairs that multiply to {n}."
        };
    }

    private static WorkedProblem Multiples(Random r, Flavor f)
    {
        var a = Pick(r, 3, 8);
        var b = Pick(r, 3, 9);
        for (var guard = 0; b == a && guard < 40; guard++) b = Pick(r, 3, 9);
        if (b == a) b = a == 3 ? 4 : 3;
        var lcm = Lcm(a, b);
        var fifth = a * 5;

        return new WorkedProblem
        {
            Setup = $"One group of {f.People} arrives every {a} minutes.\nAnother arrives every {b} minutes.\nThey both just arrived together.",
            Steps =
            [
                Step($"A multiple is what you land on counting up in {a}s.",
                     $"Counting {a}, {a * 2}, {a * 3}... what is the FIFTH multiple of {a}?",
                     fifth.ToString(),
                     [
                         $"Count up in {a}s and stop at the fifth one.",
                         $"{a}, {a * 2}, {a * 3}, {a * 4}, ...",
                         $"{a} × 5 = {fifth}."
                     ],
                     "Yes — the fifth jump."),

                Step($"Now list the multiples of {b} until you find one that is also a multiple of {a}.",
                     $"What is the SMALLEST number both {a} and {b} go into?",
                     lcm.ToString(),
                     [
                         $"Write the {a}s and the {b}s and look for the first match.",
                         $"{a}: {string.Join(", ", Enumerable.Range(1, 6).Select(i => a * i))}",
                         $"The first number in both lists is {lcm}."
                     ],
                     $"That is the one — {lcm}."),

                Step("So back to the question.",
                     $"In how many minutes will both groups arrive together again?",
                     lcm.ToString(),
                     [
                         "It is the number you just found.",
                         "They meet on a number that is in both lists.",
                         $"{lcm} minutes."
                     ],
                     "Exactly.")
            ],
            Recap = $"Both groups meet again after {lcm} minutes.\nThat is the smallest number {a} and {b} both divide into."
        };
    }

    private static WorkedProblem TwoByTwo(Random r, Flavor f)
    {
        int a2 = Pick(r, 2, 9), a1 = Pick(r, 2, 9);
        int b2 = Pick(r, 2, 9), b1 = Pick(r, 2, 9);
        var a = a2 * 10 + a1;
        var b = b2 * 10 + b1;

        int p1 = a2 * 10 * (b2 * 10), p2 = a2 * 10 * b1, p3 = a1 * (b2 * 10), p4 = a1 * b1;

        return new WorkedProblem
        {
            Setup = $"{f.Place} has {a} {f.Boxes}.\nEach one holds {b} {f.Things}.",
            Steps =
            [
                Step($"Split both numbers: {a} is {a2 * 10} and {a1}. {b} is {b2 * 10} and {b1}.\nThat makes four small multiplications.",
                     $"First piece: what is {a2 * 10} × {b2 * 10}?",
                     p1.ToString(),
                     [
                         $"{a2} × {b2} first, then add the two zeros back.",
                         $"{a2} × {b2} = {a2 * b2}.",
                         $"So {a2 * 10} × {b2 * 10} = {p1}."
                     ],
                     "The biggest piece, done."),

                Step("Second piece.",
                     $"What is {a2 * 10} × {b1}?",
                     p2.ToString(),
                     [
                         $"{a2} × {b1}, then one zero back.",
                         $"{a2} × {b1} = {a2 * b1}.",
                         $"So {a2 * 10} × {b1} = {p2}."
                     ],
                     "Good."),

                Step("Third piece.",
                     $"What is {a1} × {b2 * 10}?",
                     p3.ToString(),
                     [
                         $"{a1} × {b2}, then one zero back.",
                         $"{a1} × {b2} = {a1 * b2}.",
                         $"So {a1} × {b2 * 10} = {p3}."
                     ],
                     "Nearly there."),

                Step($"Last piece: {a1} × {b1} = {p4}. Now put all four together.",
                     $"What is {a} × {b}?",
                     (a * b).ToString(),
                     [
                         $"Add {p1} + {p2} + {p3} + {p4}.",
                         $"{p1} + {p2} = {p1 + p2}, and {p3} + {p4} = {p3 + p4}.",
                         $"{a} × {b} = {a * b}."
                     ],
                     $"{a} × {b} = {a * b}. Four pieces, one answer.", "a number", N(a * b))
            ],
            Recap = $"{a} × {b} = {p1} + {p2} + {p3} + {p4} = {a * b}.\nSplit both numbers, multiply the pieces, add them up."
        };
    }

    private static WorkedProblem LongDivision(Random r, Flavor f)
    {
        var by = Pick(r, 3, 9);
        var quotient = Pick(r, 21, 98);
        var rem = Pick(r, 1, by - 1);
        var total = quotient * by + rem;

        var h = total / 100;
        var firstTwo = total / 10;
        var q1 = firstTwo / by;
        var carry = firstTwo % by;
        var nextNum = carry * 10 + total % 10;

        return new WorkedProblem
        {
            Setup = $"{total} {f.Things} are shared between {by} {f.Boxes}.\nEach {f.Box} gets the same number.",
            Steps =
            [
                Step($"Work left to right. Take the first two digits of {total}, which are {firstTwo}.",
                     $"How many whole {by}s fit into {firstTwo}?",
                     q1.ToString(),
                     [
                         $"How many times does {by} go into {firstTwo} without going over?",
                         $"{by} × {q1} = {by * q1}, and {by} × {q1 + 1} = {by * (q1 + 1)} is too big.",
                         $"It goes in {q1} times."
                     ],
                     "That is the first digit of the answer."),

                Step($"{by} × {q1} = {by * q1}. Take that off {firstTwo}, then bring down the last digit.",
                     $"What number are you dividing into now?",
                     nextNum.ToString(),
                     [
                         $"{firstTwo} − {by * q1} = {carry}.",
                         $"Bring down the {total % 10} next to it.",
                         $"That makes {nextNum}."
                     ],
                     "Good — bring it down and carry on."),

                Step($"Now divide {nextNum} by {by}.",
                     $"How many whole {by}s fit into {nextNum}?",
                     (nextNum / by).ToString(),
                     [
                         $"{by} × ? gets closest to {nextNum} without passing it.",
                         $"{by} × {nextNum / by} = {by * (nextNum / by)}.",
                         $"It goes in {nextNum / by} times."
                     ],
                     "That is the next digit."),

                Step("Whatever is left over is the remainder.",
                     $"What is left over?",
                     rem.ToString(),
                     [
                         $"{nextNum} − {by * (nextNum / by)}.",
                         "It is always smaller than what you are dividing by.",
                         $"The remainder is {rem}."
                     ],
                     $"So {total} ÷ {by} = {quotient} remainder {rem}.")
            ],
            Recap = $"{total} ÷ {by} = {quotient} remainder {rem}.\nOne digit at a time, left to right, bringing the next one down."
        };
    }

    // ============================================================ unit three

    private static WorkedProblem EqualFractions(Random r, Flavor f)
    {
        var den = Pick(r, 3, 9);
        var num = Pick(r, 1, den - 1);
        var by = Pick(r, 2, 5);

        return new WorkedProblem
        {
            Setup = $"{num}/{den} of the {f.Things} at {f.Place} are ready.\n\nYou need to write that with a bottom number of {den * by}.",
            Steps =
            [
                Step("Whatever you do to the bottom, you must do to the top. That is the whole rule.",
                     $"What do you multiply {den} by to get {den * by}?",
                     by.ToString(),
                     [
                         $"{den} × what = {den * by}?",
                         $"That is {den * by} ÷ {den}.",
                         $"{den} × {by} = {den * by}."
                     ],
                     $"Yes — times {by}."),

                Step($"So the top gets multiplied by {by} as well.",
                     $"What is {num} × {by}?",
                     (num * by).ToString(),
                     [
                         "Same thing you did to the bottom.",
                         $"{num} × {by}.",
                         $"{num} × {by} = {num * by}."
                     ],
                     "Good."),

                Step("Now write the whole fraction.",
                     $"Write {num}/{den} with a bottom of {den * by}. (like 3/4)",
                     $"{num * by}/{den * by}",
                     [
                         "Top over bottom, using the two numbers you worked out.",
                         $"Top {num * by}, bottom {den * by}.",
                         $"{num}/{den} = {num * by}/{den * by}."
                     ],
                     "Same amount, different numbers.", "a fraction like 3/4")
            ],
            Recap = $"{num}/{den} = {num * by}/{den * by}.\nMultiply top and bottom by the same thing and the amount does not change."
        };
    }

    private static WorkedProblem Simplify(Random r, Flavor f)
    {
        // Already-in-lowest-terms pairs, so there is always something to simplify
        // TO. Rolling until coprime can spin; a list cannot.
        (int n, int d)[] lowest =
        [
            (1, 2), (1, 3), (2, 3), (1, 4), (3, 4), (1, 5), (2, 5), (3, 5), (4, 5),
            (1, 6), (5, 6), (1, 7), (2, 7), (3, 7), (4, 7), (5, 7), (6, 7),
            (1, 8), (3, 8), (5, 8), (7, 8), (1, 9), (2, 9), (4, 9), (5, 9), (7, 9), (8, 9)
        ];
        var chosen = lowest[r.Next(lowest.Length)];
        var baseNum = chosen.n;
        var baseDen = chosen.d;

        var by = Pick(r, 2, 6);
        int num = baseNum * by, den = baseDen * by;

        return new WorkedProblem
        {
            Setup = $"{num} of the {den} {f.Things} at {f.Place} are painted.\n\nWrite that fraction as simply as it will go.",
            Steps =
            [
                Step("Simplifying means finding the biggest number that divides into BOTH.",
                     $"What is the biggest number that divides into both {num} and {den}?",
                     by.ToString(),
                     [
                         $"Try the factors of {num} and see which also divide {den}.",
                         $"{num} ÷ {by} and {den} ÷ {by} both come out whole.",
                         $"The biggest one is {by}."
                     ],
                     $"Yes — {by} divides into both."),

                Step($"Now divide the top by {by}.",
                     $"What is {num} ÷ {by}?",
                     baseNum.ToString(),
                     [
                         "Just the top number.",
                         $"{num} ÷ {by}.",
                         $"{num} ÷ {by} = {baseNum}."
                     ],
                     "Good."),

                Step($"And the bottom by the same {by}.",
                     $"Write {num}/{den} in its simplest form. (like 3/4)",
                     $"{baseNum}/{baseDen}",
                     [
                         $"{den} ÷ {by} for the bottom.",
                         $"Top {baseNum}, bottom {baseDen}.",
                         $"{num}/{den} = {baseNum}/{baseDen}."
                     ],
                     "That will not go any further.", "a fraction like 3/4")
            ],
            Recap = $"{num}/{den} = {baseNum}/{baseDen}.\nDivide top and bottom by the biggest number that goes into both."
        };
    }

    private static WorkedProblem CompareFractions(Random r, Flavor f)
    {
        // Pick a pair that is genuinely comparable. Spinning on a re-roll can
        // never terminate: with a bottom of 2 there is only ONE possible top, so
        // if that one ties there is nothing else to pick.
        int d1 = 2, d2 = 3, n1 = 1, n2 = 1, common = 6, c1 = 3, c2 = 2;

        for (var attempt = 0; attempt < 40; attempt++)
        {
            d1 = Pick(r, 2, 6);
            d2 = Pick(r, 3, 8);
            if (d2 == d1) continue;

            n1 = Pick(r, 1, d1 - 1);
            n2 = Pick(r, 1, d2 - 1);

            common = Lcm(d1, d2);
            c1 = n1 * (common / d1);
            c2 = n2 * (common / d2);

            if (c1 != c2) break;
        }

        var bigger = c1 > c2 ? $"{n1}/{d1}" : $"{n2}/{d2}";

        return new WorkedProblem
        {
            Setup = $"One {f.Person} finished {n1}/{d1} of the work.\nAnother finished {n2}/{d2}.\n\nWho did more?",
            Steps =
            [
                Step("You cannot compare fractions until the bottoms match.",
                     $"What is the smallest number both {d1} and {d2} divide into?",
                     common.ToString(),
                     [
                         $"Count up in {d1}s and in {d2}s and find the first match.",
                         $"{d1}: {string.Join(", ", Enumerable.Range(1, 5).Select(i => d1 * i))}",
                         $"They meet at {common}."
                     ],
                     $"{common} — that is the common bottom."),

                Step($"Change {n1}/{d1} so its bottom is {common}.",
                     $"What is the new top number for {n1}/{d1}?",
                     c1.ToString(),
                     [
                         $"{d1} × {common / d1} = {common}, so the top gets × {common / d1} too.",
                         $"{n1} × {common / d1}.",
                         $"{n1} × {common / d1} = {c1}."
                     ],
                     $"So {n1}/{d1} = {c1}/{common}."),

                Step($"Now the other one: {n2}/{d2} with a bottom of {common}.",
                     $"What is the new top number for {n2}/{d2}?",
                     c2.ToString(),
                     [
                         $"{d2} × {common / d2} = {common}, so the top gets × {common / d2}.",
                         $"{n2} × {common / d2}.",
                         $"{n2} × {common / d2} = {c2}."
                     ],
                     $"So {n2}/{d2} = {c2}/{common}."),

                Step($"Same bottoms now, so the bigger top wins: {c1}/{common} and {c2}/{common}.",
                     "Which original fraction is bigger? (write it like 3/4)",
                     bigger,
                     [
                         $"Compare {c1} and {c2}.",
                         $"{Math.Max(c1, c2)} is bigger, so that one is more.",
                         $"{bigger} is the bigger fraction."
                     ],
                     "That is the bigger one.", "a fraction like 3/4")
            ],
            Recap = $"{n1}/{d1} = {c1}/{common} and {n2}/{d2} = {c2}/{common}, so {bigger} is bigger.\nMake the bottoms match, then compare the tops."
        };
    }

    private static WorkedProblem SameBottom(Random r, Flavor f)
    {
        var den = Pick(r, 5, 12);
        var n1 = Pick(r, 1, den - 2);
        var n2 = Pick(r, 1, den - n1 - 1);
        var sum = n1 + n2;
        var g = Gcd(sum, den);

        return new WorkedProblem
        {
            Setup = $"One {f.Person} painted {n1}/{den} of the wall.\nAnother painted {n2}/{den}.",
            Steps =
            [
                Step("The bottoms already match, so only the tops get added. The bottom stays put.",
                     $"What is {n1} + {n2}?",
                     sum.ToString(),
                     [
                         "Just the two top numbers.",
                         $"{n1} + {n2}.",
                         $"{n1} + {n2} = {sum}."
                     ],
                     "Yes — tops only."),

                Step($"The bottom does NOT change. It stays {den}.",
                     $"Write the answer as a fraction. (like 3/4)",
                     $"{sum}/{den}",
                     [
                         $"Top {sum}, bottom {den}.",
                         "Adding fractions never adds the bottoms.",
                         $"{n1}/{den} + {n2}/{den} = {sum}/{den}."
                     ],
                     "Right.", "a fraction like 3/4"),

                Step(g > 1
                        ? $"That one will simplify — {g} divides into both."
                        : "Check whether it simplifies. This one does not.",
                     "Write it in simplest form. (like 3/4)",
                     $"{sum / g}/{den / g}",
                     [
                         g > 1 ? $"Divide top and bottom by {g}." : "Nothing divides into both, so it stays as it is.",
                         g > 1 ? $"{sum} ÷ {g} = {sum / g}, {den} ÷ {g} = {den / g}." : $"{sum}/{den} is already simplest.",
                         $"The answer is {sum / g}/{den / g}."
                     ],
                     "Done properly.", "a fraction like 3/4")
            ],
            Recap = $"{n1}/{den} + {n2}/{den} = {sum}/{den} = {sum / g}/{den / g}.\nSame bottoms: add the tops, keep the bottom, then simplify."
        };
    }

    // ============================================================= unit four

    private static WorkedProblem UnlikeBottoms(Random r, Flavor f)
    {
        int d1 = Pick(r, 2, 6), d2 = Pick(r, 3, 8);
        for (var guard = 0; d2 == d1 && guard < 40; guard++) d2 = Pick(r, 3, 8);
        if (d2 == d1) d2 = d1 == 3 ? 4 : 3;
        int n1 = Pick(r, 1, d1 - 1), n2 = Pick(r, 1, d2 - 1);

        var common = Lcm(d1, d2);
        int c1 = n1 * (common / d1), c2 = n2 * (common / d2);
        var sum = c1 + c2;
        var g = Gcd(sum, common);

        return new WorkedProblem
        {
            Setup = $"{n1}/{d1} of the {f.Things} are packed.\nThen another {n2}/{d2} get packed.",
            Steps =
            [
                Step("Different bottoms cannot be added. Make them match first.",
                     $"What is the smallest number both {d1} and {d2} go into?",
                     common.ToString(),
                     [
                         $"List the {d1}s and the {d2}s and find the first match.",
                         $"{d1}: {string.Join(", ", Enumerable.Range(1, 5).Select(i => d1 * i))}",
                         $"They meet at {common}."
                     ],
                     $"{common} is the common bottom."),

                Step($"Rewrite {n1}/{d1} with a bottom of {common}.",
                     "What is its new top?",
                     c1.ToString(),
                     [
                         $"The bottom was multiplied by {common / d1}, so the top is too.",
                         $"{n1} × {common / d1}.",
                         $"{n1} × {common / d1} = {c1}."
                     ],
                     $"{n1}/{d1} = {c1}/{common}."),

                Step($"And {n2}/{d2} with a bottom of {common}.",
                     "What is its new top?",
                     c2.ToString(),
                     [
                         $"The bottom was multiplied by {common / d2}.",
                         $"{n2} × {common / d2}.",
                         $"{n2} × {common / d2} = {c2}."
                     ],
                     $"{n2}/{d2} = {c2}/{common}."),

                Step($"Now they match: {c1}/{common} + {c2}/{common}.",
                     "Add them and give the answer in simplest form. (like 3/4)",
                     $"{sum / g}/{common / g}",
                     [
                         $"Add the tops: {c1} + {c2} = {sum}. Bottom stays {common}.",
                         g > 1 ? $"Then divide top and bottom by {g}." : "Check if it simplifies — this one does not.",
                         $"The answer is {sum / g}/{common / g}."
                     ],
                     "That is it.", "a fraction like 3/4", $"{sum}/{common}")
            ],
            Recap = $"{n1}/{d1} + {n2}/{d2} = {c1}/{common} + {c2}/{common} = {sum / g}/{common / g}.\nMatch the bottoms, add the tops, simplify."
        };
    }

    private static WorkedProblem MultiplyFractions(Random r, Flavor f)
    {
        int d1 = Pick(r, 2, 8), d2 = Pick(r, 2, 8);
        int n1 = Pick(r, 1, d1 - 1), n2 = Pick(r, 1, d2 - 1);
        int num = n1 * n2, den = d1 * d2;
        var g = Gcd(num, den);

        return new WorkedProblem
        {
            Setup = $"{n1}/{d1} of {f.Place} is the workshop.\n{n2}/{d2} of THAT space is used for storage.",
            Steps =
            [
                Step("Multiplying fractions is the easy one: straight across. No matching bottoms.",
                     $"Multiply the tops: {n1} × {n2}?",
                     num.ToString(),
                     [
                         "Just the two top numbers.",
                         $"{n1} × {n2}.",
                         $"{n1} × {n2} = {num}."
                     ],
                     "Top done."),

                Step("Now straight across the bottom.",
                     $"Multiply the bottoms: {d1} × {d2}?",
                     den.ToString(),
                     [
                         "Just the two bottom numbers.",
                         $"{d1} × {d2}.",
                         $"{d1} × {d2} = {den}."
                     ],
                     "Bottom done."),

                Step(g > 1 ? $"That will simplify — {g} goes into both." : "Now check whether it simplifies.",
                     "Give the answer in simplest form. (like 3/4)",
                     $"{num / g}/{den / g}",
                     [
                         $"You have {num}/{den}.",
                         g > 1 ? $"Divide top and bottom by {g}." : "Nothing divides both, so it stays.",
                         $"The answer is {num / g}/{den / g}."
                     ],
                     "Straight across, then tidy up.", "a fraction like 3/4", $"{num}/{den}")
            ],
            Recap = $"{n1}/{d1} × {n2}/{d2} = {num}/{den} = {num / g}/{den / g}.\nTops across, bottoms across, then simplify."
        };
    }

    private static WorkedProblem DecimalPlace(Random r, Flavor f)
    {
        int whole = Pick(r, 2, 48), t = Pick(r, 1, 9), h = Pick(r, 1, 9), th = Pick(r, 1, 9);
        var value = $"{whole}.{t}{h}{th}";
        var slot = Pick(r, 1, 3);
        var (digit, place, worth) = slot switch
        {
            1 => (t, "tenths", $"{t}/10"),
            2 => (h, "hundredths", $"{h}/100"),
            _ => (th, "thousandths", $"{th}/1000")
        };

        return new WorkedProblem
        {
            Setup = $"{f.Money} costs ${value} at {f.Event}.\n\nLook at the digit {digit}, after the point.",
            Steps =
            [
                Step("After the point the places are tenths, then hundredths, then thousandths.",
                     $"Which place is the {digit} in?",
                     place,
                     [
                         "Count places to the RIGHT of the point.",
                         "First is tenths, second hundredths, third thousandths.",
                         $"The {digit} is in the {place} place."
                     ],
                     $"Yes — {place}.", "a place name"),

                Step("Every decimal place is really a fraction.",
                     $"Write that digit as a fraction. (like 3/10)",
                     worth,
                     [
                         $"It is {digit} out of the place it sits in.",
                         $"{place} means out of {(slot == 1 ? 10 : slot == 2 ? 100 : 1000)}.",
                         $"It is {worth}."
                     ],
                     "That is what the place means.", "a fraction"),

                Step("One more, to be sure you can read the whole thing.",
                     $"How many whole dollars is ${value}? (the part before the point)",
                     whole.ToString(),
                     [
                         "Everything to the LEFT of the point.",
                         "The point separates whole from part.",
                         $"{whole} whole dollars."
                     ],
                     "Good.")
            ],
            Recap = $"In {value}, the {digit} is in the {place} place, which means {worth}."
        };
    }

    private static WorkedProblem AddDecimals(Random r, Flavor f)
    {
        int aw = Pick(r, 2, 40), at = Pick(r, 1, 9), ah = Pick(r, 1, 9);
        int bw = Pick(r, 2, 40), bt = Pick(r, 1, 9), bh = Pick(r, 1, 9);

        var a = aw + at / 10.0 + ah / 100.0;
        var b = bw + bt / 10.0 + bh / 100.0;
        var sum = Math.Round(a + b, 2);
        var hund = ah + bh;
        var carry = hund / 10;

        return new WorkedProblem
        {
            Setup = $"One {f.Money} costs ${a:0.00}.\nAnother costs ${b:0.00}.",
            Steps =
            [
                Step("Line the decimal points up under each other. That is the whole trick.",
                     $"Add the hundredths: {ah} + {bh}?",
                     hund.ToString(),
                     [
                         "The last digit of each price.",
                         $"{ah} + {bh}.",
                         $"{ah} + {bh} = {hund}."
                     ],
                     hund >= 10 ? "More than 9, so one carries." : "Good."),

                Step(carry > 0 ? $"Carry the {carry} into the tenths." : "Now the tenths.",
                     $"Add the tenths: {at} + {bt}{(carry > 0 ? $" + {carry}" : "")}?",
                     (at + bt + carry).ToString(),
                     [
                         "The first digit after each point.",
                         $"{at} + {bt}{(carry > 0 ? $", then add the {carry} carried" : "")}.",
                         $"That is {at + bt + carry}."
                     ],
                     "Good."),

                Step("Now the whole dollars, and write the full amount with the point in it.",
                     $"What is ${a:0.00} + ${b:0.00}? (like 12.34)",
                     sum.ToString("0.00"),
                     [
                         $"Whole dollars: {aw} + {bw} = {aw + bw}, plus any carry.",
                         "Keep the point lined up in the answer.",
                         $"${a:0.00} + ${b:0.00} = ${sum:0.00}."
                     ],
                     "Point lined up, answer right.", "like 12.34",
                     $"${sum:0.00}", sum.ToString())
            ],
            Recap = $"${a:0.00} + ${b:0.00} = ${sum:0.00}.\nLine up the points and add each column like normal."
        };
    }

    // ============================================================= unit five

    private static WorkedProblem MultiplyDecimals(Random r, Flavor f)
    {
        int a = Pick(r, 12, 89), b = Pick(r, 2, 9);
        var dec = a / 10.0;
        var product = Math.Round(dec * b, 1);

        return new WorkedProblem
        {
            Setup = $"Each {f.Thing} costs ${dec:0.0}.\nA group buys {b} of them.",
            Steps =
            [
                Step("Ignore the point for a moment and multiply as whole numbers.",
                     $"What is {a} × {b}?",
                     (a * b).ToString(),
                     [
                         "Pretend the point is not there.",
                         $"{a} × {b}.",
                         $"{a} × {b} = {a * b}."
                     ],
                     "Now the point goes back."),

                Step($"Count the digits after the point in the question. {dec:0.0} has one.",
                     "How many digits should be after the point in the answer?",
                     "1",
                     [
                         "Count them in the numbers you multiplied.",
                         $"{dec:0.0} has one digit after the point; {b} has none.",
                         "So the answer has 1."
                     ],
                     "Right — one."),

                Step("Put the point that many places from the right.",
                     $"What is ${dec:0.0} × {b}? (like 12.3)",
                     product.ToString("0.0"),
                     [
                         $"You have {a * b}, and the point goes one place from the right.",
                         $"{a * b} becomes {product:0.0}.",
                         $"${dec:0.0} × {b} = ${product:0.0}."
                     ],
                     "Multiply, then count the places.", "like 12.3",
                     $"${product:0.0}", product.ToString())
            ],
            Recap = $"${dec:0.0} × {b} = ${product:0.0}.\nMultiply as whole numbers, then put back as many decimal places as you started with."
        };
    }

    private static WorkedProblem DivideDecimal(Random r, Flavor f)
    {
        int by = Pick(r, 2, 8);
        int q = Pick(r, 11, 89);
        var total = Math.Round(q * by / 10.0, 1);
        var each = q / 10.0;

        return new WorkedProblem
        {
            Setup = $"${total:0.0} is shared equally between {by} {f.People}.",
            Steps =
            [
                Step("Dividing a decimal is normal division. The point stays where it is, straight up into the answer.",
                     $"Ignore the point: what is {q * by} ÷ {by}?",
                     q.ToString(),
                     [
                         $"How many {by}s go into {q * by}?",
                         $"{by} × {q} = {q * by}.",
                         $"So {q * by} ÷ {by} = {q}."
                     ],
                     "Good."),

                Step($"Now put the point back. {total:0.0} has one digit after the point.",
                     $"How many digits go after the point in the answer?",
                     "1",
                     [
                         "The same as the number you were dividing.",
                         "Dividing by a whole number does not move the point.",
                         "One."
                     ],
                     "Right."),

                Step("So write the amount each person gets.",
                     $"How much does each get? (like 3.5)",
                     each.ToString("0.0"),
                     [
                         $"You had {q}, and the point goes one from the right.",
                         $"{q} becomes {each:0.0}.",
                         $"${total:0.0} ÷ {by} = ${each:0.0}."
                     ],
                     "Point straight up.", "like 3.5",
                     $"${each:0.0}", each.ToString())
            ],
            Recap = $"${total:0.0} ÷ {by} = ${each:0.0} each.\nDivide as usual; the point comes straight up into the answer."
        };
    }

    private static WorkedProblem OrderOfOperations(Random r, Flavor f)
    {
        int a = Pick(r, 2, 9), b = Pick(r, 2, 9), c = Pick(r, 2, 9), d = Pick(r, 2, 12);
        var product = b * c;
        var answer = a + product - d;
        if (answer < 0) { d = Pick(r, 2, Math.Max(2, a + product)); answer = a + product - d; }

        return new WorkedProblem
        {
            Setup = $"Work this out:\n\n{a} + {b} × {c} − {d}",
            Steps =
            [
                Step("Not left to right. Multiplication and division happen BEFORE adding and taking away.",
                     $"Which part gets done first?",
                     $"{b} × {c}",
                     [
                         "Look for multiplying or dividing.",
                         $"There is one multiplication: {b} × {c}.",
                         $"So {b} × {c} goes first."
                     ],
                     "Yes — multiply first.", $"like {b} x {c}",
                     $"{b}x{c}", $"{b}*{c}"),

                Step("So do that piece.",
                     $"What is {b} × {c}?",
                     product.ToString(),
                     [
                         "Just those two numbers.",
                         $"{b} × {c}.",
                         $"{b} × {c} = {product}."
                     ],
                     "Good."),

                Step($"Now the sum reads {a} + {product} − {d}. NOW you can go left to right.",
                     $"What is {a} + {product} − {d}?",
                     answer.ToString(),
                     [
                         $"{a} + {product} first.",
                         $"{a} + {product} = {a + product}, then take away {d}.",
                         $"{a + product} − {d} = {answer}."
                     ],
                     "That is it.")
            ],
            Recap = $"{a} + {b} × {c} − {d} = {a} + {product} − {d} = {answer}.\nMultiply and divide first, then add and take away, left to right."
        };
    }

    private static WorkedProblem PowersOfTen(Random r, Flavor f)
    {
        int a = Pick(r, 12, 98);
        var power = Pick(r, 2, 4);
        var mult = (int)Math.Pow(10, power);
        var dec = a / 100.0;
        var result = Math.Round(dec * mult, 2);

        return new WorkedProblem
        {
            Setup = $"{f.Place} orders {dec:0.00} boxes-worth of {f.Things}\nfor every one of its {mult} {f.People}.",
            Steps =
            [
                Step($"10 to the power {power} just means {power} tens multiplied together.",
                     $"What is 10 to the power {power} as a normal number?",
                     mult.ToString(),
                     [
                         $"That is {string.Join(" × ", Enumerable.Repeat("10", power))}.",
                         $"It is a 1 followed by {power} zeros.",
                         $"It is {mult}."
                     ],
                     $"Yes — {N(mult)}."),

                Step($"Multiplying by {mult} moves the point {power} places to the RIGHT.",
                     $"How many places does the point move?",
                     power.ToString(),
                     [
                         "One place for each zero.",
                         $"{mult} has {power} zeros.",
                         $"{power} places."
                     ],
                     "Right."),

                Step($"Start at {dec:0.00} and move the point {power} places right.",
                     $"What is {dec:0.00} × {mult}?",
                     result.ToString("0.##"),
                     [
                         $"Move the point in {dec:0.00} one place at a time.",
                         $"After {power} moves you get {result:0.##}.",
                         $"{dec:0.00} × {mult} = {result:0.##}."
                     ],
                     "Bigger number, point moved right.", "a number",
                     result.ToString(), result.ToString("0.00"))
            ],
            Recap = $"{dec:0.00} × 10^{power} = {result:0.##}.\nEach zero moves the point one place to the right."
        };
    }

    // ============================================================== unit six

    private static WorkedProblem EvaluateExpression(Random r, Flavor f)
    {
        int m = Pick(r, 2, 9), b = Pick(r, 1, 20), x = Pick(r, 2, 12);
        var answer = m * x + b;

        return new WorkedProblem
        {
            Setup = $"Each {f.Person} needs {m} {f.Things}, and {b} spare are kept back.\n\nThat is {m}x + {b}, where x is the number of {f.People}.\n\nWork it out when x = {x}.",
            Steps =
            [
                Step("A letter is just a number you have not been told yet. Now you have been told it.",
                     $"{m}x means {m} times x. What is {m} × {x}?",
                     (m * x).ToString(),
                     [
                         $"Put {x} in place of x.",
                         $"{m} × {x}.",
                         $"{m} × {x} = {m * x}."
                     ],
                     "That is the multiplying part."),

                Step($"Now the rest of the expression.",
                     $"What is {m * x} + {b}?",
                     answer.ToString(),
                     [
                         "Add the spare ones on.",
                         $"{m * x} + {b}.",
                         $"{m * x} + {b} = {answer}."
                     ],
                     "Good."),

                Step("So say what the whole thing comes to.",
                     $"What is {m}x + {b} when x = {x}?",
                     answer.ToString(),
                     [
                         "It is the number you just worked out.",
                         $"{m} × {x} + {b}.",
                         $"It is {answer}."
                     ],
                     $"{m}x + {b} = {answer} when x = {x}.")
            ],
            Recap = $"When x = {x}, {m}x + {b} = {m * x} + {b} = {answer}.\nSwap the number in for the letter, then work it out."
        };
    }

    private static WorkedProblem WordsIntoMaths(Random r, Flavor f)
    {
        int m = Pick(r, 2, 9), b = Pick(r, 2, 15);

        return new WorkedProblem
        {
            Setup = $"Every {f.Person} at {f.Place} brings {m} {f.Things}.\nThen {b} more are delivered.\n\nWrite that as an expression, using x for the number of {f.People}.",
            Steps =
            [
                Step("Start with the part that depends on how many people there are.",
                     $"How do you write \"{m} {f.Things} for each of x {f.People}\"? (like 3x)",
                     $"{m}x",
                     [
                         "It is a multiplication, but you never write the times sign.",
                         $"{m} times x.",
                         $"You write it {m}x."
                     ],
                     $"Yes — {m}x.", "like 3x",
                     $"{m}*x", $"{m} x", $"x*{m}"),

                Step($"Now the {b} extra ones. Those come no matter how many {f.People} there are.",
                     "Do those get multiplied by x, or just added on? (multiplied or added)",
                     "added",
                     [
                         $"Does the number {b} change when more {f.People} come?",
                         "It does not — it is the same either way.",
                         "So it is just added on."
                     ],
                     "Right — it stands alone.", "multiplied or added"),

                Step("Put the two parts together.",
                     "Write the whole expression. (like 3x + 5)",
                     $"{m}x + {b}",
                     [
                         $"The changing part is {m}x. The fixed part is {b}.",
                         "Join them with a plus.",
                         $"{m}x + {b}."
                     ],
                     "That is the expression.", "like 3x + 5",
                     $"{m}x+{b}", $"{b} + {m}x", $"{b}+{m}x")
            ],
            Recap = $"{m}x + {b}.\nThe part that changes gets the letter; the part that does not is added on."
        };
    }

    private static WorkedProblem OneStep(Random r, Flavor f)
    {
        var x = Pick(r, 3, 20);
        var b = Pick(r, 2, 25);
        var total = x + b;
        var plus = r.Next(2) == 0;

        if (plus)
        {
            return new WorkedProblem
            {
                Setup = $"There are x {f.Things} at {f.Place}.\n{b} more arrive, making {total} in all.\n\nx + {b} = {total}",
                Steps =
                [
                    Step($"Whatever you do to one side you must do to the other. To get x alone, undo the + {b}.",
                         $"What do you do to BOTH sides? (like -5)",
                         $"-{b}",
                         [
                             $"The opposite of adding {b}.",
                             $"You take {b} away.",
                             $"Write it as -{b}."
                         ],
                         $"Yes — subtract {b} from both sides.", "like -5",
                         $"−{b}", $"subtract {b}", $"minus {b}"),

                    Step($"Do it to the right side: {total} − {b}.",
                         $"What is {total} − {b}?",
                         x.ToString(),
                         [
                             $"{total} take away {b}.",
                             $"{total} − {b}.",
                             $"{total} − {b} = {x}."
                         ],
                         "Good."),

                    Step("The left side is now just x, so that is your answer.",
                         "What is x?",
                         x.ToString(),
                         [
                             "It is the number you just worked out.",
                             $"x = {x}.",
                             $"Check it: {x} + {b} = {total}."
                         ],
                         $"x = {x}. Check: {x} + {b} = {total}.")
                ],
                Recap = $"x + {b} = {total}, so x = {x}.\nTake {b} off both sides and x is on its own."
            };
        }

        var m = Pick(r, 2, 9);
        var prod = m * x;

        return new WorkedProblem
        {
            Setup = $"{m} equal {f.Boxes} hold {prod} {f.Things} altogether.\n\n{m}x = {prod}",
            Steps =
            [
                Step($"x is being MULTIPLIED by {m}. The opposite of multiplying is dividing.",
                     $"What do you do to both sides? (like ÷3 — write it as /3)",
                     $"/{m}",
                     [
                         $"The opposite of times {m}.",
                         $"You divide by {m}.",
                         $"Write it as /{m}."
                     ],
                     $"Yes — divide both sides by {m}.", "like /3",
                     $"÷{m}", $"divide by {m}", $"{m}"),

                Step($"Do it to the right: {prod} ÷ {m}.",
                     $"What is {prod} ÷ {m}?",
                     x.ToString(),
                     [
                         $"How many {m}s make {prod}?",
                         $"{prod} ÷ {m}.",
                         $"{prod} ÷ {m} = {x}."
                     ],
                     "Good."),

                Step("Left side is x on its own now.",
                     "What is x?",
                     x.ToString(),
                     [
                         "The number you just found.",
                         $"x = {x}.",
                         $"Check: {m} × {x} = {prod}."
                     ],
                     $"x = {x}. Check: {m} × {x} = {prod}.")
            ],
            Recap = $"{m}x = {prod}, so x = {x}.\nDivide both sides by {m} to get x alone."
        };
    }

    private static WorkedProblem TwoStep(Random r, Flavor f)
    {
        int m = Pick(r, 2, 8), x = Pick(r, 2, 12), b = Pick(r, 2, 20);
        var total = m * x + b;

        return new WorkedProblem
        {
            Setup = $"Each of the x {f.Boxes} holds {m} {f.Things},\nand {b} more sit on the side. There are {total} in all.\n\n{m}x + {b} = {total}",
            Steps =
            [
                Step($"Two things are happening to x: it is times {m}, and {b} is added. Undo the ADDING first — the opposite order to how it was built.",
                     $"What do you do to both sides first? (like -5)",
                     $"-{b}",
                     [
                         $"Deal with the {b} before the {m}.",
                         $"The opposite of adding {b}.",
                         $"Take {b} off both sides: -{b}."
                     ],
                     $"Yes — the {b} goes first.", "like -5",
                     $"−{b}", $"subtract {b}", $"minus {b}"),

                Step($"Right side: {total} − {b}.",
                     $"What does the equation say now? Give just the right side.",
                     (m * x).ToString(),
                     [
                         $"{total} − {b}.",
                         $"{total} − {b} = {m * x}.",
                         $"So now it reads {m}x = {m * x}."
                     ],
                     $"Now it is {m}x = {m * x}."),

                Step($"One thing left: x is times {m}. Undo it.",
                     $"What do you do to both sides now? (write it like /3)",
                     $"/{m}",
                     [
                         $"The opposite of times {m}.",
                         $"Divide by {m}.",
                         $"Write it as /{m}."
                     ],
                     $"Divide both sides by {m}.", "like /3",
                     $"÷{m}", $"divide by {m}", $"{m}"),

                Step($"So work out {m * x} ÷ {m}.",
                     "What is x?",
                     x.ToString(),
                     [
                         $"{m * x} ÷ {m}.",
                         $"{m * x} ÷ {m} = {x}.",
                         $"Check: {m} × {x} + {b} = {total}."
                     ],
                     $"x = {x}. Check: {m} × {x} + {b} = {total}.")
            ],
            Recap = $"{m}x + {b} = {total}\n  −{b}:  {m}x = {m * x}\n  ÷{m}:  x = {x}\nUndo the adding first, the multiplying second."
        };
    }
}
