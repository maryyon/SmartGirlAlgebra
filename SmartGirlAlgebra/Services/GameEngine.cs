namespace SmartGirlAlgebra.Services;

public class GameEngine
{
    private Random random = new Random();
    private CheerleadingScenarioGenerator scenarioGenerator = new CheerleadingScenarioGenerator();

    public List<string> GetWittyCorrectResponse(int streak, bool usedHints)
    {
        var responses = new List<string>();

        if (streak >= 10)
        {
            responses.Add("🔥 OMG BESTIE! 10+ in a row! You're literally UNSTOPPABLE! Are you secretly a math wizard?! 🧙‍♀️✨");
        }
        else if (streak >= 5)
        {
            responses.Add($"💫 YASSS! {streak} STREAK! You're absolutely CRUSHING it like a championship squad! 👑");
        }
        else if (!usedHints)
        {
            responses.Add("🎯 PERFECT! No hints needed - you're a NATURAL! Coach would be SO proud! 💪✨");
        }

        var wittyResponses = new List<string>
        {
            "✨ YASSS QUEEN! That's how it's DONE! The squad is CHEERING for you!",
            "🎉 BOOM! Nailed it like a PERFECT landing! Coach is IMPRESSED!",
            "⭐ OMG! You just made that look SO easy! Are you secretly a genius?!",
            "💪 FLEX! You just CRUSHED that like a championship routine!",
            "🏆 LEGENDARY! The whole squad is watching and they're AMAZED!",
            "🎊 SPECTACULAR! You're basically a MATH CHEERLEADER now! 📣",
            "🌟 BRILLIANT! Your brain is literally GLOWING! Sparkle power activated! ✨",
            "🚀 TO THE TOP! That answer was FLAWLESS! You're flying high!",
            "💎 FLAWLESS! Like a perfectly executed stunt! The crowd goes WILD!",
            "🎭 DRAMATIC PAUSE... CORRECT! *entire stadium erupts in cheers*",
            "🎪 AMAZING! You just pulled off the IMPOSSIBLE! That's ELITE level!",
            "🌈 MAGICAL! Did you just sprinkle algebra fairy dust on that?! ✨",
            "⚡ LIGHTNING FAST! And totally PERFECT! You're on FIRE!",
            "🎯 BULLSEYE! Right on target! That's championship material!",
            "🎨 MASTERPIECE! That solution was BEAUTIFUL! Pure artistry!"
        };

        responses.Add(wittyResponses[random.Next(wittyResponses.Count)]);
        return responses;
    }

    public string GetWittyIncorrectResponse(int mistakeCount)
    {
        if (mistakeCount >= 3)
        {
            var encouragingResponses = new List<string>
            {
                "💪 Hey bestie, even CHAMPIONS stumble before they stick the landing! Let's break this down together! ✨",
                "🌟 No worries! Every mistake is just PRACTICE in disguise! You're learning! Keep going! 🎀",
                "🎯 Close! You're thinking in the RIGHT direction - let's refine it and nail this! 💫",
                "💡 Ooh, interesting approach! Let me show you a cheerleader trick to make this EASIER! 📣"
            };
            return encouragingResponses[random.Next(encouragingResponses.Count)];
        }

        var responses = new List<string>
        {
            "🤔 Hmm, not quite bestie! But I LIKE where your head's at! Try again! 💭",
            "💭 Close call! You're on the right track! Keep pushing! You've got this! ✨",
            "🎪 Oops! That's not it, but NICE try! Every cheerleader needs practice! 🎀",
            "🌊 Almost! You're SO close! One more adjustment and you'll nail it! 💪",
            "🎯 So close I can FEEL it! Try again! You're almost there! 🌟",
            "🎨 Creative answer! But let's aim for the bullseye! You can do this! 💫",
            "🎭 Plot twist! That's not it, but don't give up! Champions never quit! 🏆",
            "🎪 Whoopsie! Let's give it another shot! You're learning! That's what matters! ✨",
            "🌟 Not quite, but your EFFORT is shining through! Keep going! 💎",
            "💫 Nope! But hey, mistakes are how we LEARN and GROW! Try again, bestie! 🎀"
        };

        return responses[random.Next(responses.Count)];
    }

    public string GetAchievementMessage(string achievement)
    {
        var messages = new Dictionary<string, string>
        {
            { "first_correct", "🎊 ACHIEVEMENT UNLOCKED: First Victory! You're officially in the game!" },
            { "streak_5", "🔥 ACHIEVEMENT UNLOCKED: Hot Streak! 5 in a row!" },
            { "streak_10", "⚡ ACHIEVEMENT UNLOCKED: Unstoppable! 10 streak - you're a legend!" },
            { "no_hints_master", "🎯 ACHIEVEMENT UNLOCKED: Pure Genius! Solved without hints!" },
            { "topic_master", "👑 ACHIEVEMENT UNLOCKED: Topic Master! You've conquered this topic!" },
            { "speed_demon", "🚀 ACHIEVEMENT UNLOCKED: Speed Demon! Lightning-fast solving!" },
            { "comeback_kid", "💪 ACHIEVEMENT UNLOCKED: Comeback Kid! Turned it around!" },
            { "perfectionist", "💎 ACHIEVEMENT UNLOCKED: Perfectionist! 100% accuracy!" },
            { "level_5", "⭐ ACHIEVEMENT UNLOCKED: Rising Star! Reached Level 5!" },
            { "level_10", "🏆 ACHIEVEMENT UNLOCKED: Elite Status! Level 10 achieved!" }
        };

        return messages.ContainsKey(achievement) ? messages[achievement] : "";
    }

    // NEW: Generate cheerleading-themed challenges
    public List<CheerChallenge> GenerateCheerChallenges(Player player, int count)
    {
        var challenges = new List<CheerChallenge>();

        // Find weakest topics
        var weakTopics = player.TopicMastery
            .Where(t => t.Value.TotalAttempts > 0)
            .OrderBy(t => t.Value.Accuracy)
            .Take(3)
            .Select(t => t.Key)
            .ToList();

        // If no weak topics yet, use variety for beginners
        if (!weakTopics.Any())
        {
            weakTopics = new List<AlgebraTopic>
            {
                AlgebraTopic.LinearEquations,
                AlgebraTopic.MultiStepEquations,
                AlgebraTopic.Inequalities
            };
        }

        for (int i = 0; i < count; i++)
        {
            var topic = weakTopics[i % weakTopics.Count];
            var difficulty = Math.Min(3, player.Level / 3 + 1);
            challenges.Add(scenarioGenerator.GenerateChallenge(topic, difficulty, player.SquadRank));
        }

        return challenges;
    }

    public List<AlgebraQuestion> GenerateDiagnosticQuiz()
    {
        var questions = new List<AlgebraQuestion>();

        // One question from each major topic to assess quickly
        questions.Add(CreateMultiStepEquation(1));
        questions.Add(CreateInequality(1));
        questions.Add(CreateExponentProblem(1));
        questions.Add(CreatePolynomialProblem(1));
        questions.Add(CreateSystemOfEquations(1));

        return questions;
    }

    public List<AlgebraQuestion> GenerateAdaptiveQuestions(Player player, int count)
    {
        var questions = new List<AlgebraQuestion>();

        // Find weakest topics
        var weakTopics = player.TopicMastery
            .Where(t => t.Value.TotalAttempts > 0)
            .OrderBy(t => t.Value.Accuracy)
            .Take(3)
            .Select(t => t.Key)
            .ToList();

        // If no weak topics yet, use variety
        if (!weakTopics.Any())
        {
            weakTopics = new List<AlgebraTopic>
            {
                AlgebraTopic.MultiStepEquations,
                AlgebraTopic.Inequalities,
                AlgebraTopic.Exponents
            };
        }

        for (int i = 0; i < count; i++)
        {
            var topic = weakTopics[i % weakTopics.Count];
            var difficulty = Math.Min(3, player.Level / 2 + 1);
            questions.Add(GenerateQuestionForTopic(topic, difficulty));
        }

        return questions;
    }

    public AlgebraQuestion GenerateQuestionForTopic(AlgebraTopic topic, int difficulty)
    {
        return topic switch
        {
            AlgebraTopic.LinearEquations => CreateLinearEquation(difficulty),
            AlgebraTopic.MultiStepEquations => CreateMultiStepEquation(difficulty),
            AlgebraTopic.Inequalities => CreateInequality(difficulty),
            AlgebraTopic.Exponents => CreateExponentProblem(difficulty),
            AlgebraTopic.Polynomials => CreatePolynomialProblem(difficulty),
            AlgebraTopic.SystemsOfEquations => CreateSystemOfEquations(difficulty),
            AlgebraTopic.Factoring => CreateFactoringProblem(difficulty),
            AlgebraTopic.QuadraticEquations => CreateQuadraticEquation(difficulty),
            _ => CreateLinearEquation(difficulty)
        };
    }

    private AlgebraQuestion CreateLinearEquation(int difficulty)
    {
        // TEAM SHOPPING TRIP - Real-world story problem
        int pricePerItem = random.Next(3, 12);
        int totalBudget = random.Next(25, 60);
        int maxItems = totalBudget / pricePerItem;
        int moneySpent = maxItems * pricePerItem;
        int changeLeft = totalBudget - moneySpent;

        string[] items = { "hair bows", "pompoms", "team socks", "water bottles", "spirit ribbons" };
        string item = items[random.Next(items.Length)];

        var question = new AlgebraQuestion(
            $"💰 TEAM SHOPPING TRIP!\n\n" +
            $"OMG bestie! Coach gave you ${totalBudget} to buy {item} for the squad! 🛍️\n\n" +
            $"Each one costs ${pricePerItem} at the cheer store.\n\n" +
            $"How many can you buy?\n\n" +
            $"(Just enter the number you can buy)",
            maxItems.ToString(),
            $"The algebra: {pricePerItem}x ≤ {totalBudget}, so x = {maxItems} with ${changeLeft} change!\n\n" +
            $"You can buy {maxItems} {item}!",
            AlgebraTopic.LinearEquations,
            difficulty
        );

        question.AddHint($"You have ${totalBudget} to spend");
        question.AddHint($"Each {item} costs ${pricePerItem}");
        question.AddHint($"Divide: ${totalBudget} ÷ ${pricePerItem} = ?");
        question.AddAcceptableAnswer($"{maxItems}");

        return question;
    }

    private AlgebraQuestion CreateMultiStepEquation(int difficulty)
    {
        // PRACTICE PERFORMANCE - Real-world story problem
        int pointsFirstHalf = random.Next(8, 15);
        int totalPoints = random.Next(20, 35);
        int pointsSecondHalf = totalPoints - pointsFirstHalf;

        string[] activities = { "tumbling passes", "dance moves", "jumps", "stunts", "cheers" };
        string activity = activities[random.Next(activities.Length)];

        var question = new AlgebraQuestion(
            $"📊 PRACTICE PERFORMANCE!\n\n" +
            $"You're tracking your progress at cheer practice today! 📋\n\n" +
            $"In the FIRST HALF of practice, you nailed {pointsFirstHalf} perfect {activity}! 💪\n\n" +
            $"By the END of practice, you had {totalPoints} total perfect moves!\n\n" +
            $"How many perfect {activity} did you do in the SECOND HALF?",
            pointsSecondHalf.ToString(),
            $"The algebra: {pointsFirstHalf} + x = {totalPoints}, so x = {pointsSecondHalf}!\n\n" +
            $"You did {pointsSecondHalf} {activity} in the second half!",
            AlgebraTopic.MultiStepEquations,
            difficulty
        );

        question.AddHint($"You did {pointsFirstHalf} in the first half");
        question.AddHint($"Your total was {totalPoints} by the end");
        question.AddHint($"Subtract: {totalPoints} - {pointsFirstHalf} = ?");

        return question;
    }

    private AlgebraQuestion CreateInequality(int difficulty)
    {
        // FUNDRAISER GOAL - Real-world story problem
        int pricePerItem = random.Next(4, 10);
        int goalAmount = random.Next(50, 120);
        int minItemsNeeded = (goalAmount + pricePerItem - 1) / pricePerItem; // Ceiling division

        string[] items = { "candy bars", "raffle tickets", "spirit shirts", "car washes", "bake sale treats" };
        string item = items[random.Next(items.Length)];

        var question = new AlgebraQuestion(
            $"🎯 FUNDRAISER GOAL!\n\n" +
            $"The squad needs to raise AT LEAST ${goalAmount} for new uniforms! 💰\n\n" +
            $"You're selling {item} for ${pricePerItem} each.\n\n" +
            $"How many do you need to sell to reach your goal?\n\n" +
            $"Remember: you need ${goalAmount} or MORE! Every sale counts! 📣",
            minItemsNeeded.ToString(),
            $"The algebra: {pricePerItem}x ≥ {goalAmount}, so x ≥ {minItemsNeeded}!\n\n" +
            $"You need to sell AT LEAST {minItemsNeeded} {item}!",
            AlgebraTopic.Inequalities,
            difficulty
        );

        question.AddHint($"You need AT LEAST ${goalAmount}");
        question.AddHint($"Each {item} sells for ${pricePerItem}");
        question.AddHint($"Divide: ${goalAmount} ÷ ${pricePerItem} and round UP");
        question.AddAcceptableAnswer($"{minItemsNeeded}");
        question.AddAcceptableAnswer($"at least {minItemsNeeded}");

        return question;
    }

    private AlgebraQuestion CreateExponentProblem(int difficulty)
    {
        // PYRAMID POWER - Real-world story problem
        int baseHeight = random.Next(2, 5);
        int levels = random.Next(2, 4);
        int totalHeight = baseHeight;
        for (int i = 1; i < levels; i++)
        {
            totalHeight *= 2; // Each level doubles
        }

        var question = new AlgebraQuestion(
            $"🏔️ PYRAMID POWER!\n\n" +
            $"You're building a {levels}-level pyramid! 🏔️\n\n" +
            $"The BASE level has {baseHeight} cheerleaders.\n\n" +
            $"Each level UP doubles the difficulty points! 💪\n\n" +
            $"Level 1: {baseHeight} points\n" +
            $"Level 2: {baseHeight} × 2 points\n" +
            $"Level 3: {baseHeight} × 2 × 2 points\n\n" +
            $"What's the TOTAL difficulty at the top level?",
            totalHeight.ToString(),
            $"The algebra: {baseHeight} × 2^{levels - 1} = {totalHeight}!\n\n" +
            $"The top level has {totalHeight} difficulty points!",
            AlgebraTopic.Exponents,
            difficulty
        );

        question.AddHint($"Start with {baseHeight} at the base");
        question.AddHint($"Double it {levels - 1} times");
        question.AddHint($"Each level multiplies by 2");
        question.AddAcceptableAnswer($"{totalHeight}");

        return question;
    }

    private AlgebraQuestion CreatePolynomialProblem(int difficulty)
    {
        // ROUTINE MASHUP - Real-world story problem
        int routine1Moves = random.Next(4, 8);
        int routine1Beats = random.Next(8, 16);
        int routine2Moves = random.Next(3, 6);
        int routine2Beats = random.Next(6, 12);
        int totalMoves = routine1Moves + routine2Moves;
        int totalBeats = routine1Beats + routine2Beats;

        var question = new AlgebraQuestion(
            $"🎵 ROUTINE MASHUP!\n\n" +
            $"Coach wants to combine TWO routines into ONE mega-routine! 🎵\n\n" +
            $"Routine A has:\n" +
            $"• {routine1Moves} dance moves 💃\n" +
            $"• {routine1Beats} music beats 🎶\n\n" +
            $"Routine B has:\n" +
            $"• {routine2Moves} dance moves 💃\n" +
            $"• {routine2Beats} music beats 🎶\n\n" +
            $"When you combine them, how many TOTAL moves are there?\n\n" +
            $"(Just enter the number of moves)",
            totalMoves.ToString(),
            $"The algebra: ({routine1Moves}m + {routine1Beats}b) + ({routine2Moves}m + {routine2Beats}b) = {totalMoves}m + {totalBeats}b!\n\n" +
            $"Total moves: {totalMoves}, Total beats: {totalBeats}",
            AlgebraTopic.Polynomials,
            difficulty
        );

        question.AddHint($"Routine A has {routine1Moves} moves");
        question.AddHint($"Routine B has {routine2Moves} moves");
        question.AddHint($"Add the moves together: {routine1Moves} + {routine2Moves} = ?");
        question.AddAcceptableAnswer($"{totalMoves}");

        return question;
    }

    private AlgebraQuestion CreateSystemOfEquations(int difficulty)
    {
        // COMPETITION SCORING - Real-world story problem
        int stuntPoints = random.Next(5, 10);
        int dancePoints = random.Next(3, 8);
        int totalScore = random.Next(40, 80);

        int stuntScore = stuntPoints * random.Next(3, 6);
        int danceScore = totalScore - stuntScore;
        int danceCount = danceScore / dancePoints;

        var question = new AlgebraQuestion(
            $"🏆 COMPETITION SCORING!\n\n" +
            $"You just competed and the scores are in! 📊\n\n" +
            $"Your team scored {stuntScore} points from stunts! 💪\n\n" +
            $"Your TOTAL score was {totalScore} points!\n\n" +
            $"Each dance section is worth {dancePoints} points.\n\n" +
            $"How many dance sections did you complete?",
            danceCount.ToString(),
            $"The algebra: {stuntScore} + {dancePoints}x = {totalScore}, so x = {danceCount}!\n\n" +
            $"You completed {danceCount} dance sections!",
            AlgebraTopic.SystemsOfEquations,
            difficulty
        );

        question.AddHint($"Stunts gave you {stuntScore} points");
        question.AddHint($"Total was {totalScore} points");
        question.AddHint($"Subtract: {totalScore} - {stuntScore} = {danceScore} dance points");
        question.AddHint($"Divide: {danceScore} ÷ {dancePoints} = ?");

        return question;
    }

    private AlgebraQuestion CreateFactoringProblem(int difficulty)
    {
        int a = random.Next(2, 6);
        int b = random.Next(2, 6);
        int product = a * b;
        int sum = a + b;

        var question = new AlgebraQuestion(
            $"Factor: x² + {sum}x + {product}",
            $"(x + {a})(x + {b})",
            $"Find two numbers that multiply to {product} and add to {sum}: {a} and {b}",
            AlgebraTopic.Factoring,
            difficulty
        );

        question.AddHint($"Find two numbers that multiply to {product} and add to {sum}");
        question.AddHint($"Think about the factors of {product}");
        question.AddAcceptableAnswer($"(x+{a})(x+{b})");
        question.AddAcceptableAnswer($"(x + {a})(x + {b})");
        question.AddAcceptableAnswer($"(x+{b})(x+{a})");
        question.AddAcceptableAnswer($"(x + {b})(x + {a})");

        return question;
    }

    private AlgebraQuestion CreateQuadraticEquation(int difficulty)
    {
        int a = random.Next(1, 4);
        int b = random.Next(-5, 6);
        int c = random.Next(-5, 6);

        // Simple quadratic that factors nicely
        int x1 = random.Next(1, 5);
        int x2 = random.Next(1, 5);
        int bCoeff = -(x1 + x2);
        int cCoeff = x1 * x2;

        string[] formations = {
            "pyramid formation",
            "stunt tower",
            "dance line arrangement",
            "competition floor pattern",
            "basket toss formation"
        };
        string formation = formations[random.Next(formations.Length)];

        var question = new AlgebraQuestion(
            $"🎪 FORMATION PLANNING!\n\n" +
            $"Your squad is designing a {formation} for the big competition! 🏆\n\n" +
            $"The spacing formula is: x² + ({bCoeff})x + {cCoeff} = 0\n\n" +
            $"Where x represents the number of cheerleaders in each section.\n\n" +
            $"Solve for x to find the perfect formation sizes!\n\n" +
            $"(The formation must balance perfectly with no gaps!) ✨",
            $"x = {x1} or x = {x2}",
            $"Factor to (x - {x1})(x - {x2}) = 0, so x = {x1} or x = {x2}\n\n" +
            $"Your {formation} can have {x1} or {x2} cheerleaders per section!",
            AlgebraTopic.QuadraticEquations,
            difficulty
        );

        question.AddHint("Try to factor this quadratic equation");
        question.AddHint($"Look for two numbers that multiply to {cCoeff} and add to {bCoeff}");
        question.AddHint($"The formation works with {x1} or {x2} cheerleaders!");
        question.AddAcceptableAnswer($"{x1}, {x2}");
        question.AddAcceptableAnswer($"{x2}, {x1}");
        question.AddAcceptableAnswer($"x={x1} or x={x2}");
        question.AddAcceptableAnswer($"x={x2} or x={x1}");

        return question;
    }
}

