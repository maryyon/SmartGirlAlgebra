namespace SmartGirlAlgebra.Services;

public class CheerleadingScenarioGenerator
{
    private Random random = new Random();

    public CheerChallenge GenerateChallenge(AlgebraTopic topic, int difficulty, CheerSquadRank rank)
    {
        return topic switch
        {
            AlgebraTopic.LinearEquations => CreateUniformBudgetChallenge(difficulty, rank),
            AlgebraTopic.MultiStepEquations => CreateCampScheduleChallenge(difficulty, rank),
            AlgebraTopic.Inequalities => CreateFundraiserGoalChallenge(difficulty, rank),
            AlgebraTopic.SystemsOfEquations => CreateCompetitionScoringChallenge(difficulty, rank),
            AlgebraTopic.Exponents => CreatePyramidHeightChallenge(difficulty, rank),
            AlgebraTopic.Polynomials => CreateRoutineTimingChallenge(difficulty, rank),
            _ => CreateUniformBudgetChallenge(difficulty, rank)
        };
    }

    // LINEAR EQUATIONS - Like "Shopping & Budgeting"
    private CheerChallenge CreateUniformBudgetChallenge(int difficulty, CheerSquadRank rank)
    {
        int pricePerItem = random.Next(3, 12);
        int totalBudget = random.Next(25, 60);
        int maxItems = totalBudget / pricePerItem;
        int moneySpent = maxItems * pricePerItem;
        int changeLeft = totalBudget - moneySpent;

        string[] items = { "hair bows 🎀", "pompoms 📣", "team socks 🧦", "water bottles 💧", "spirit ribbons 🎗️" };
        string item = items[random.Next(items.Length)];

        var challenge = new CheerChallenge
        {
            Topic = AlgebraTopic.LinearEquations,
            Difficulty = difficulty,
            Title = $"💰 TEAM SHOPPING TRIP!",
            Scenario = $"OMG bestie! Coach gave you ${totalBudget} to buy {item} for the squad! 🛍️\n\n" +
                      $"Each one costs ${pricePerItem} at the cheer store.\n\n" +
                      $"You need to figure out:\n" +
                      $"• How many can you buy?\n" +
                      $"• How much money will you have left?\n\n" +
                      $"The team is counting on you to get as many as possible! Let's do this! ✨",
            InteractionType = ChallengeInteractionType.StepByStepSolver,
            CorrectAnswer = $"{maxItems}",
            Steps = new List<ChallengeStep>
            {
                new ChallengeStep
                {
                    Instruction = "Look at your total budget",
                    Hint = $"You have ${totalBudget} to spend",
                    CheerVoice = $"Okay bestie, we've got ${totalBudget} from Coach! That's our shopping money! 💰"
                },
                new ChallengeStep
                {
                    Instruction = "Check the price per item",
                    Hint = $"Each {item.Replace("🎀", "").Replace("📣", "").Replace("🧦", "").Replace("💧", "").Replace("🎗️", "").Trim()} costs ${pricePerItem}",
                    CheerVoice = $"Each one is ${pricePerItem}! Not bad! ✨"
                },
                new ChallengeStep
                {
                    Instruction = "Calculate how many you can buy",
                    Hint = $"Divide: ${totalBudget} ÷ ${pricePerItem} = ?",
                    CheerVoice = $"So how many times does ${pricePerItem} fit into ${totalBudget}? That's our answer! 🎯"
                },
                new ChallengeStep
                {
                    Instruction = "Figure out the change",
                    Hint = $"Multiply {maxItems} × ${pricePerItem}, then subtract from ${totalBudget}",
                    CheerVoice = $"Now let's see what's left over! Maybe we can get stickers! 💫"
                }
            },
            SuccessMessage = $"YASSS! 🎉 You can buy {maxItems} {item} and have ${changeLeft} left!\n\n" +
                           $"That's the algebra: {pricePerItem}x ≤ {totalBudget}, so x = {maxItems} with ${changeLeft} change!\n\n" +
                           $"The squad is gonna be SO happy! 👑",
            EncouragementMessages = new List<string>
            {
                "Think division! How many times does the price fit? 💭",
                "You're SO close! Don't forget the leftover money! 💪",
                "Almost there! Budget minus spending equals change! ✨"
            }
        };

        return challenge;
    }

    // MULTI-STEP EQUATIONS - Like "Sports & Fitness (Performance Tracking)"
    private CheerChallenge CreateCampScheduleChallenge(int difficulty, CheerSquadRank rank)
    {
        int pointsFirstHalf = random.Next(8, 15);
        int totalPoints = random.Next(20, 35);
        int pointsSecondHalf = totalPoints - pointsFirstHalf;

        string[] activities = { "tumbling passes", "dance moves", "jumps", "stunts", "cheers" };
        string activity = activities[random.Next(activities.Length)];

        var challenge = new CheerChallenge
        {
            Topic = AlgebraTopic.MultiStepEquations,
            Difficulty = difficulty,
            Title = "📊 PRACTICE PERFORMANCE!",
            Scenario = $"You're tracking your progress at cheer practice today! 📋\n\n" +
                      $"In the FIRST HALF of practice, you nailed {pointsFirstHalf} perfect {activity}! 💪\n\n" +
                      $"By the END of practice, you had {totalPoints} total perfect moves!\n\n" +
                      $"How many perfect {activity} did you do in the SECOND HALF?\n\n" +
                      $"Coach is tracking everyone's progress—let's figure this out! ✨",
            InteractionType = ChallengeInteractionType.StepByStepSolver,
            CorrectAnswer = pointsSecondHalf.ToString(),
            Steps = new List<ChallengeStep>
            {
                new ChallengeStep
                {
                    Instruction = "Look at your first half performance",
                    Hint = $"You did {pointsFirstHalf} in the first half",
                    CheerVoice = $"Okay, first half you crushed {pointsFirstHalf} moves! Nice! 💪"
                },
                new ChallengeStep
                {
                    Instruction = "Check your total by the end",
                    Hint = $"Total for the whole practice: {totalPoints}",
                    CheerVoice = $"By the end, you had {totalPoints} total! That's the goal! 🎯"
                },
                new ChallengeStep
                {
                    Instruction = "Figure out the second half",
                    Hint = $"Subtract: {totalPoints} - {pointsFirstHalf} = ?",
                    CheerVoice = "So what's the difference? That's how many you did in the second half! 🤔"
                }
            },
            SuccessMessage = $"PERFECT! 🎉 You did {pointsSecondHalf} {activity} in the second half!\n\n" +
                           $"The algebra: {pointsFirstHalf} + x = {totalPoints}, so x = {pointsSecondHalf}!\n\n" +
                           $"Coach is gonna be IMPRESSED with your tracking skills! 📊👑",
            EncouragementMessages = new List<string>
            {
                "Think about what you need to ADD to the first half! 💭",
                "You're doing great! Subtraction will help here! 💪",
                "Almost there! Total minus first half equals second half! ✨"
            }
        };

        return challenge;
    }




    // INEQUALITIES - Like "Shopping & Budgeting" with constraints
    private CheerChallenge CreateFundraiserGoalChallenge(int difficulty, CheerSquadRank rank)
    {
        int pricePerItem = random.Next(4, 10);
        int goalAmount = random.Next(50, 120);
        int minItemsNeeded = (goalAmount + pricePerItem - 1) / pricePerItem; // Ceiling division

        string[] items = { "candy bars 🍫", "raffle tickets 🎟️", "spirit shirts 👕", "car washes 🚗", "bake sale treats 🧁" };
        string item = items[random.Next(items.Length)];

        var challenge = new CheerChallenge
        {
            Topic = AlgebraTopic.Inequalities,
            Difficulty = difficulty,
            Title = "🎯 FUNDRAISER GOAL!",
            Scenario = $"The squad needs to raise AT LEAST ${goalAmount} for new uniforms! 💰\n\n" +
                      $"You're selling {item} for ${pricePerItem} each.\n\n" +
                      $"How many do you need to sell to reach your goal?\n\n" +
                      $"Remember: you need ${goalAmount} or MORE! Every sale counts! 📣\n\n" +
                      $"The competition is coming up and we NEED those uniforms! Let's GO! ✨",
            InteractionType = ChallengeInteractionType.JumpCounter,
            CorrectAnswer = minItemsNeeded.ToString(),
            Steps = new List<ChallengeStep>
            {
                new ChallengeStep
                {
                    Instruction = "Look at your fundraising goal",
                    Hint = $"You need AT LEAST ${goalAmount}",
                    CheerVoice = $"Okay bestie, we need ${goalAmount} minimum! That's the target! 🎯"
                },
                new ChallengeStep
                {
                    Instruction = "Check how much each sale makes",
                    Hint = $"Each {item.Replace("🍫", "").Replace("🎟️", "").Replace("👕", "").Replace("🚗", "").Replace("🧁", "").Trim()} = ${pricePerItem}",
                    CheerVoice = $"Every sale brings in ${pricePerItem}! Ka-ching! 💰"
                },
                new ChallengeStep
                {
                    Instruction = "Figure out the minimum sales needed",
                    Hint = $"How many ${pricePerItem} sales to get to ${goalAmount}?",
                    CheerVoice = "Keep counting sales until you hit the goal! You've got this! 💪"
                }
            },
            SuccessMessage = $"YASSS! 🎉 You need to sell AT LEAST {minItemsNeeded} {item} to reach ${goalAmount}!\n\n" +
                           $"The algebra: {pricePerItem}x ≥ {goalAmount}, so x ≥ {minItemsNeeded}!\n\n" +
                           $"That's inequality math, bestie! You found the MINIMUM! 👑",
            EncouragementMessages = new List<string>
            {
                "Keep going! You're getting closer to the goal! 💪",
                "Almost there! A few more sales! ✨",
                "You're doing AMAZING! Keep counting! 🎯"
            }
        };

        return challenge;
    }

    // SYSTEMS OF EQUATIONS - Like "Travel & Time" with two variables
    private CheerChallenge CreateCompetitionScoringChallenge(int difficulty, CheerSquadRank rank)
    {
        int stuntPoints = random.Next(5, 10);
        int dancePoints = random.Next(3, 8);
        int totalScore = random.Next(40, 80);

        // Simple system: stunts worth more, dance worth less
        // For simplicity: if you do 'x' stunts at stuntPoints each and 'y' dances at dancePoints each
        // We'll make it simpler: "You scored X points from stunts, need total Y, how many dance points?"
        int stuntScore = stuntPoints * random.Next(3, 6);
        int danceScore = totalScore - stuntScore;
        int danceCount = danceScore / dancePoints;

        var challenge = new CheerChallenge
        {
            Topic = AlgebraTopic.SystemsOfEquations,
            Difficulty = difficulty,
            Title = "🏆 COMPETITION SCORING!",
            Scenario = $"You just competed and the scores are in! 📊\n\n" +
                      $"Your team scored {stuntScore} points from stunts! 💪\n\n" +
                      $"Your TOTAL score was {totalScore} points!\n\n" +
                      $"Each dance section is worth {dancePoints} points.\n\n" +
                      $"How many dance sections did you complete?\n\n" +
                      $"The judges are waiting for the breakdown! Let's figure it out! ✨",
            InteractionType = ChallengeInteractionType.DualBalance,
            CorrectAnswer = danceCount.ToString(),
            Steps = new List<ChallengeStep>
            {
                new ChallengeStep
                {
                    Instruction = "Look at your stunt points",
                    Hint = $"Stunts gave you {stuntScore} points",
                    CheerVoice = $"Okay, stunts scored {stuntScore} points! That's solid! 💪"
                },
                new ChallengeStep
                {
                    Instruction = "Check your total score",
                    Hint = $"Total score: {totalScore} points",
                    CheerVoice = $"Total was {totalScore}! That's what we're working with! 🎯"
                },
                new ChallengeStep
                {
                    Instruction = "Find the dance points",
                    Hint = $"Subtract: {totalScore} - {stuntScore} = dance points",
                    CheerVoice = "So how many points came from dance? Subtract the stunts! 💃"
                },
                new ChallengeStep
                {
                    Instruction = "Calculate dance sections",
                    Hint = $"Divide dance points by {dancePoints}",
                    CheerVoice = $"Now divide by {dancePoints} to get the number of sections! 🎵"
                }
            },
            SuccessMessage = $"PERFECT! 🎉 You completed {danceCount} dance sections!\n\n" +
                           $"The algebra: {stuntScore} + {dancePoints}x = {totalScore}, so x = {danceCount}!\n\n" +
                           $"You just solved a SYSTEM! The judges are impressed! 👑",
            EncouragementMessages = new List<string>
            {
                "Think about the two parts: stunts AND dance! 💭",
                "You're balancing TWO things! Keep going! 💪",
                "Almost there! Subtract, then divide! ✨"
            }
        };

        return challenge;
    }


    // EXPONENTS - Like "Video Games (Scoring/Levels)" with multipliers
    private CheerChallenge CreatePyramidHeightChallenge(int difficulty, CheerSquadRank rank)
    {
        int baseHeight = random.Next(2, 5);
        int levels = random.Next(2, 4);
        int totalHeight = baseHeight;
        for (int i = 1; i < levels; i++)
        {
            totalHeight *= 2; // Each level doubles
        }

        var challenge = new CheerChallenge
        {
            Topic = AlgebraTopic.Exponents,
            Difficulty = difficulty,
            Title = "🏔️ PYRAMID POWER!",
            Scenario = $"You're building a {levels}-level pyramid! 🏔️\n\n" +
                      $"The BASE level has {baseHeight} cheerleaders.\n\n" +
                      $"Each level UP doubles the difficulty points! 💪\n\n" +
                      $"Level 1: {baseHeight} points\n" +
                      $"Level 2: {baseHeight} × 2 points\n" +
                      $"Level 3: {baseHeight} × 2 × 2 points\n\n" +
                      $"What's the TOTAL difficulty at the top level?\n\n" +
                      $"This is POWER math, bestie! Literally! ✨",
            InteractionType = ChallengeInteractionType.PyramidBuilder,
            CorrectAnswer = totalHeight.ToString(),
            Steps = new List<ChallengeStep>
            {
                new ChallengeStep
                {
                    Instruction = "Start with the base",
                    Hint = $"Base level: {baseHeight} points",
                    CheerVoice = $"We start with {baseHeight} at the bottom! That's our foundation! 💪"
                },
                new ChallengeStep
                {
                    Instruction = "Double for each level up",
                    Hint = $"Multiply by 2 for each of the {levels - 1} levels above base",
                    CheerVoice = $"Now double it {levels - 1} times! Each level multiplies! 🚀"
                },
                new ChallengeStep
                {
                    Instruction = "Calculate the final power",
                    Hint = $"{baseHeight} × 2^{levels - 1} = ?",
                    CheerVoice = "See how the power GROWS? That's exponents, bestie! 💫"
                }
            },
            SuccessMessage = $"AMAZING! 🎉 The top level has {totalHeight} difficulty points!\n\n" +
                           $"The algebra: {baseHeight} × 2^{levels - 1} = {totalHeight}!\n\n" +
                           $"That's exponential growth! You're a MATH CHAMPION! 👑",
            EncouragementMessages = new List<string>
            {
                "Keep doubling! The power grows fast! 🚀",
                "You're multiplying! Watch it grow! 💪",
                "Almost there! One more double! ✨"
            }
        };

        return challenge;
    }

    // POLYNOMIALS - Like "Cooking & Recipes (Scaling)" with combining
    private CheerChallenge CreateRoutineTimingChallenge(int difficulty, CheerSquadRank rank)
    {
        int routine1Moves = random.Next(4, 8);
        int routine1Beats = random.Next(8, 16);
        int routine2Moves = random.Next(3, 6);
        int routine2Beats = random.Next(6, 12);
        int totalMoves = routine1Moves + routine2Moves;
        int totalBeats = routine1Beats + routine2Beats;

        var challenge = new CheerChallenge
        {
            Topic = AlgebraTopic.Polynomials,
            Difficulty = difficulty,
            Title = "🎵 ROUTINE MASHUP!",
            Scenario = $"Coach wants to combine TWO routines into ONE mega-routine! 🎵\n\n" +
                      $"Routine A has:\n" +
                      $"• {routine1Moves} dance moves 💃\n" +
                      $"• {routine1Beats} music beats 🎶\n\n" +
                      $"Routine B has:\n" +
                      $"• {routine2Moves} dance moves 💃\n" +
                      $"• {routine2Beats} music beats 🎶\n\n" +
                      $"When you combine them, how many TOTAL moves and beats?\n\n" +
                      $"Competition is Saturday! Let's figure out the timing! ✨",
            InteractionType = ChallengeInteractionType.RoutineCombiner,
            CorrectAnswer = $"{totalMoves} moves, {totalBeats} beats",
            Steps = new List<ChallengeStep>
            {
                new ChallengeStep
                {
                    Instruction = "Look at Routine A",
                    Hint = $"Routine A: {routine1Moves} moves, {routine1Beats} beats",
                    CheerVoice = $"Routine A has {routine1Moves} moves and {routine1Beats} beats! Got it! ✨"
                },
                new ChallengeStep
                {
                    Instruction = "Look at Routine B",
                    Hint = $"Routine B: {routine2Moves} moves, {routine2Beats} beats",
                    CheerVoice = $"Routine B has {routine2Moves} moves and {routine2Beats} beats! Check! 💫"
                },
                new ChallengeStep
                {
                    Instruction = "Add all the MOVES together",
                    Hint = $"{routine1Moves} + {routine2Moves} = ?",
                    CheerVoice = "Add up ALL the moves! Moves with moves! 💪"
                },
                new ChallengeStep
                {
                    Instruction = "Add all the BEATS together",
                    Hint = $"{routine1Beats} + {routine2Beats} = ?",
                    CheerVoice = "Now add up ALL the beats! Beats with beats! 🎵"
                }
            },
            SuccessMessage = $"MEGA-ROUTINE COMPLETE! 🎉\n\n" +
                           $"Total: {totalMoves} moves + {totalBeats} beats = PERFECTION!\n\n" +
                           $"The algebra: ({routine1Moves}m + {routine1Beats}b) + ({routine2Moves}m + {routine2Beats}b) = {totalMoves}m + {totalBeats}b!\n\n" +
                           $"You just combined like terms! That's polynomial math! 👑",
            EncouragementMessages = new List<string>
            {
                "Group the same types together! 💭",
                "Moves with moves, beats with beats! 🎵",
                "You're combining like a champion! ✨"
            }
        };

        return challenge;
    }
}

public class CheerChallenge
{
    public AlgebraTopic Topic { get; set; }
    public int Difficulty { get; set; }
    public string Title { get; set; } = "";
    public string Scenario { get; set; } = "";
    public ChallengeInteractionType InteractionType { get; set; }
    public string CorrectAnswer { get; set; } = "";
    public List<ChallengeStep> Steps { get; set; } = new();
    public string SuccessMessage { get; set; } = "";
    public List<string> EncouragementMessages { get; set; } = new();
    public int CurrentStep { get; set; } = 0;
}

public class ChallengeStep
{
    public string Instruction { get; set; } = "";
    public string Hint { get; set; } = "";
    public string CheerVoice { get; set; } = "";
    public bool Completed { get; set; } = false;
}

public enum ChallengeInteractionType
{
    BalanceScale,
    StepByStepSolver,
    JumpCounter,
    DualBalance,
    PyramidBuilder,
    RoutineCombiner,
    DragAndDrop,
    SliderAdjust
}

