using SmartGirlAlgebra.Models;

namespace SmartGirlAlgebra.Services;

/// <summary>
/// Generates cheerleading-themed algebra problems
/// </summary>
public class ProblemGenerator
{
    private readonly Random _random = new();
    private readonly LinearEquationSolver _solver = new();
    private readonly EquationParser _parser = new();

    /// <summary>
    /// Generates a random linear equation problem with cheerleading theme
    /// </summary>
    public CheerProblem GenerateLinearEquation(DifficultyLevel difficulty)
    {
        return difficulty switch
        {
            DifficultyLevel.Easy => GenerateEasyProblem(),
            DifficultyLevel.Medium => GenerateMediumProblem(),
            DifficultyLevel.Hard => GenerateHardProblem(),
            _ => GenerateEasyProblem()
        };
    }

    private CheerProblem GenerateEasyProblem()
    {
        var templates = new[]
        {
            GeneratePomPomShopping(),
            GenerateRoutineTiming(),
            GenerateFundraiser(),
            GenerateSquadSize()
        };

        return templates[_random.Next(templates.Length)];
    }

    private CheerProblem GenerateMediumProblem()
    {
        var templates = new[]
        {
            GenerateCompetitionScoring(),
            GeneratePyramidBalance(),
            GeneratePracticeSchedule()
        };

        return templates[_random.Next(templates.Length)];
    }

    private CheerProblem GenerateHardProblem()
    {
        var templates = new[]
        {
            GenerateComplexFormation(),
            GenerateMultiStepFundraiser()
        };

        return templates[_random.Next(templates.Length)];
    }

    private CheerProblem GeneratePomPomShopping()
    {
        int pricePerPair = _random.Next(5, 15);
        int bagCost = _random.Next(10, 25);
        int totalMoney = _random.Next(40, 100);

        // Ensure we get a whole number answer
        int pairs = _random.Next(2, 8);
        totalMoney = pricePerPair * pairs + bagCost;

        string equation = $"{pricePerPair}x + {bagCost} = {totalMoney}";
        var solution = _solver.Solve(_parser.Parse(equation), "x");

        var problem = new CheerProblem
        {
            Title = "🛍️ POM-POM SHOPPING SPREE",
            Story = $"You're shopping for the squad! Each pair of pom-poms costs ${pricePerPair}, " +
                   $"and you also need to buy a team bag for ${bagCost}. Your coach gave you ${totalMoney}. " +
                   $"How many pairs of pom-poms can you buy?",
            Equation = equation,
            Variable = "x",
            VariableDescription = "number of pom-pom pairs",
            Answer = solution.Value,
            Steps = solution.Steps,
            Difficulty = DifficultyLevel.Easy,
            Emoji = "🛍️"
        };

        // Add 5th-grader friendly hints (4 hints - NO ANSWER!)
        problem.Hints.Add($"💡 HINT 1: First, let's figure out what we know! You have ${totalMoney} total to spend.");
        problem.Hints.Add($"💰 HINT 2: The team bag costs ${bagCost}. That's money you HAVE to spend no matter what, so subtract it first: ${totalMoney} - ${bagCost} = ${totalMoney - bagCost}");
        problem.Hints.Add($"🎀 HINT 3: Now you have ${totalMoney - bagCost} left over just for pom-poms!");
        problem.Hints.Add($"➗ HINT 4: Each pair of pom-poms costs ${pricePerPair}. To find how many pairs you can buy, divide the leftover money by the price. What is ${totalMoney - bagCost} ÷ ${pricePerPair}?");

        return problem;
    }

    private CheerProblem GenerateRoutineTiming()
    {
        int secondsPer8Count = 8;
        int openingPoseSeconds = _random.Next(8, 20);
        int totalSeconds = _random.Next(40, 90);

        // Ensure whole number answer
        int eightCounts = _random.Next(3, 8);
        totalSeconds = secondsPer8Count * eightCounts + openingPoseSeconds;

        string equation = $"{secondsPer8Count}x + {openingPoseSeconds} = {totalSeconds}";
        var solution = _solver.Solve(_parser.Parse(equation), "x");

        var problem = new CheerProblem
        {
            Title = "⏱️ ROUTINE TIMING CHALLENGE",
            Story = $"Your routine has x eight-counts of dance moves (each is 8 seconds), " +
                   $"plus a {openingPoseSeconds}-second opening pose. The total routine must be exactly {totalSeconds} seconds. " +
                   $"How many eight-counts do you need?",
            Equation = equation,
            Variable = "x",
            VariableDescription = "number of eight-counts",
            Answer = solution.Value,
            Steps = solution.Steps,
            Difficulty = DifficultyLevel.Easy,
            Emoji = "⏱️"
        };

        // Add 5th-grader friendly hints
        problem.Hints.Add($"💡 HINT 1: Let's break this down! Your routine is {totalSeconds} seconds total.");
        problem.Hints.Add($"⏱️ HINT 2: The opening pose takes {openingPoseSeconds} seconds. That happens no matter what, so subtract it: {totalSeconds} - {openingPoseSeconds} = {totalSeconds - openingPoseSeconds} seconds left for dancing!");
        problem.Hints.Add($"💃 HINT 3: Now you have {totalSeconds - openingPoseSeconds} seconds for your eight-counts.");
        problem.Hints.Add($"➗ HINT 4: Each eight-count is {secondsPer8Count} seconds. To find how many eight-counts fit, divide: {totalSeconds - openingPoseSeconds} ÷ {secondsPer8Count}. What do you get?");

        return problem;
    }

    private CheerProblem GenerateFundraiser()
    {
        int pricePerItem = _random.Next(2, 6);
        int alreadyRaised = _random.Next(20, 50);
        int goal = _random.Next(80, 200);

        // Ensure whole number answer
        int itemsNeeded = _random.Next(10, 40);
        goal = pricePerItem * itemsNeeded + alreadyRaised;

        string[] items = { "cupcakes", "cookies", "brownies", "pom-pom keychains", "spirit bracelets" };
        string item = items[_random.Next(items.Length)];

        string equation = $"{pricePerItem}x + {alreadyRaised} = {goal}";
        var solution = _solver.Solve(_parser.Parse(equation), "x");

        var problem = new CheerProblem
        {
            Title = "🧁 FUNDRAISER CHALLENGE",
            Story = $"Your squad is selling {item} for ${pricePerItem} each. You've already raised ${alreadyRaised} " +
                   $"from a car wash. Your goal is ${goal} for new uniforms. How many {item} do you need to sell?",
            Equation = equation,
            Variable = "x",
            VariableDescription = $"number of {item}",
            Answer = solution.Value,
            Steps = solution.Steps,
            Difficulty = DifficultyLevel.Easy,
            Emoji = "🧁"
        };

        // Add 5th-grader friendly hints
        problem.Hints.Add($"💡 HINT 1: Your goal is to raise ${goal} total!");
        problem.Hints.Add($"💰 HINT 2: You already have ${alreadyRaised} from the car wash. Great start! Now subtract that from your goal: ${goal} - ${alreadyRaised} = ${goal - alreadyRaised}");
        problem.Hints.Add($"🎯 HINT 3: You still need ${goal - alreadyRaised} more dollars!");
        problem.Hints.Add($"➗ HINT 4: Each {item.TrimEnd('s')} sells for ${pricePerItem}. To find how many you need to sell, divide the money you still need by the price: ${goal - alreadyRaised} ÷ ${pricePerItem}");

        return problem;
    }

    private CheerProblem GenerateSquadSize()
    {
        int multiplier = 2;
        int adjustment = _random.Next(-5, 6);
        int varsitySize = _random.Next(10, 20);

        // JV = (Varsity - adjustment) / multiplier
        int jvSize = _random.Next(5, 12);
        varsitySize = multiplier * jvSize + adjustment;

        string equation = $"{multiplier}x + {adjustment} = {varsitySize}";
        var solution = _solver.Solve(_parser.Parse(equation), "x");

        var problem = new CheerProblem
        {
            Title = "👯 SQUAD SIZE PUZZLE",
            Story = $"The JV squad has x members. The Varsity squad has {multiplier} times as many members as JV" +
                   (adjustment >= 0 ? $", plus {adjustment} extra" : $", minus {-adjustment}") +
                   $". If Varsity has {varsitySize} members, how many are on JV?",
            Equation = equation,
            Variable = "x",
            VariableDescription = "JV squad members",
            Answer = solution.Value,
            Steps = solution.Steps,
            Difficulty = DifficultyLevel.Easy,
            Emoji = "👯"
        };

        // Add 5th-grader friendly hints
        problem.Hints.Add($"💡 HINT 1: We know Varsity has {varsitySize} members total.");
        problem.Hints.Add($"👯 HINT 2: Varsity has {multiplier} times as many as JV" + (adjustment >= 0 ? $", plus {adjustment} extra." : $", minus {-adjustment}.") + $" So first, let's deal with that extra part: {varsitySize} " + (adjustment >= 0 ? $"- {adjustment}" : $"+ {-adjustment}") + $" = {varsitySize - adjustment}");
        problem.Hints.Add($"🎯 HINT 3: Now we have {varsitySize - adjustment}, which is {multiplier} times the JV size.");
        problem.Hints.Add($"➗ HINT 4: To find the JV size, divide by {multiplier}: {varsitySize - adjustment} ÷ {multiplier}. What's the answer?");

        return problem;
    }

    private CheerProblem GenerateCompetitionScoring()
    {
        int stuntMultiplier = 2;
        int spiritBonus = _random.Next(10, 25);
        int totalScore = _random.Next(60, 100);

        // x + 2x + bonus = total, so 3x = total - bonus
        int tumblingScore = _random.Next(15, 30);
        totalScore = tumblingScore + (stuntMultiplier * tumblingScore) + spiritBonus;

        string equation = $"x + {stuntMultiplier}x + {spiritBonus} = {totalScore}";
        var solution = _solver.Solve(_parser.Parse(equation), "x");

        var problem = new CheerProblem
        {
            Title = "🏆 COMPETITION SCORE MYSTERY",
            Story = $"At regionals, your squad scored points for tumbling (x points) and stunts ({stuntMultiplier}x points). " +
                   $"You also got a {spiritBonus}-point bonus for spirit. Your total score was {totalScore} points. " +
                   $"What was your tumbling score?",
            Equation = equation,
            Variable = "x",
            VariableDescription = "tumbling points",
            Answer = solution.Value,
            Steps = solution.Steps,
            Difficulty = DifficultyLevel.Medium,
            Emoji = "🏆"
        };

        // Add 5th-grader friendly hints
        problem.Hints.Add($"💡 HINT 1: Your total score is {totalScore} points. This includes tumbling, stunts, AND the spirit bonus!");
        problem.Hints.Add($"🎁 HINT 2: The spirit bonus is {spiritBonus} points. That's extra! So subtract it first: {totalScore} - {spiritBonus} = {totalScore - spiritBonus} points from tumbling and stunts.");
        problem.Hints.Add($"🤸 HINT 3: Now here's the tricky part! Tumbling is x points, and stunts are {stuntMultiplier} times that (so {stuntMultiplier}x). Together they equal {totalScore - spiritBonus}. That means x + {stuntMultiplier}x = {totalScore - spiritBonus}");
        problem.Hints.Add($"➗ HINT 4: When you add x + {stuntMultiplier}x, you get {1 + stuntMultiplier}x. So {1 + stuntMultiplier}x = {totalScore - spiritBonus}. Now divide both sides by {1 + stuntMultiplier}: {totalScore - spiritBonus} ÷ {1 + stuntMultiplier}");

        return problem;
    }

    private CheerProblem GeneratePyramidBalance()
    {
        int leftBase = _random.Next(2, 6);
        int rightBase = _random.Next(1, 4);
        int rightFlyers = _random.Next(5, 12);

        // leftBase + x = rightBase + rightFlyers
        int leftFlyers = rightBase + rightFlyers - leftBase;

        string equation = $"{leftBase} + x = {rightBase} + {rightFlyers}";
        var solution = _solver.Solve(_parser.Parse(equation), "x");

        var problem = new CheerProblem
        {
            Title = "🔺 PYRAMID FORMATION BALANCE",
            Story = $"Your squad is building a pyramid! The left side has {leftBase} cheerleaders plus some flyers (x). " +
                   $"The right side has {rightBase} cheerleader(s) plus {rightFlyers} flyers. " +
                   $"Both sides need the same number of people to balance!",
            Equation = equation,
            Variable = "x",
            VariableDescription = "flyers on left side",
            Answer = solution.Value,
            Steps = solution.Steps,
            Difficulty = DifficultyLevel.Medium,
            Emoji = "🔺"
        };

        // Add 5th-grader friendly hints
        problem.Hints.Add($"💡 HINT 1: For the pyramid to balance, both sides need the SAME number of people!");
        problem.Hints.Add($"🔺 HINT 2: The right side has {rightBase} cheerleaders plus {rightFlyers} flyers. Let's add those: {rightBase} + {rightFlyers} = {rightBase + rightFlyers} people on the right!");
        problem.Hints.Add($"⚖️ HINT 3: The left side has {leftBase} cheerleaders plus x flyers. So the left side is {leftBase} + x people.");
        problem.Hints.Add($"➗ HINT 4: Since both sides must be equal: {leftBase} + x = {rightBase + rightFlyers}. To find x, subtract {leftBase} from both sides: x = {rightBase + rightFlyers} - {leftBase}");

        return problem;
    }

    private CheerProblem GeneratePracticeSchedule()
    {
        int yourWeekend = _random.Next(3, 8);
        int rivalWeekday = _random.Next(1, 4);
        int rivalWeekend = _random.Next(8, 15);

        // x + yourWeekend = rivalWeekday + rivalWeekend
        int yourWeekday = rivalWeekday + rivalWeekend - yourWeekend;

        string equation = $"x + {yourWeekend} = {rivalWeekday} + {rivalWeekend}";
        var solution = _solver.Solve(_parser.Parse(equation), "x");

        var problem = new CheerProblem
        {
            Title = "📅 PRACTICE SCHEDULE BALANCE",
            Story = $"You practice x hours on weekdays and {yourWeekend} hours on weekends. " +
                   $"Your rival squad practices {rivalWeekday} hours on weekdays and {rivalWeekend} hours on weekends. " +
                   $"To stay competitive, you need to match their total hours!",
            Equation = equation,
            Variable = "x",
            VariableDescription = "weekday practice hours",
            Answer = solution.Value,
            Steps = solution.Steps,
            Difficulty = DifficultyLevel.Medium,
            Emoji = "📅"
        };

        // Add 5th-grader friendly hints
        problem.Hints.Add($"💡 HINT 1: You need to practice the SAME total hours as your rival squad to stay competitive!");
        problem.Hints.Add($"📊 HINT 2: Your rival practices {rivalWeekday} hours on weekdays and {rivalWeekend} hours on weekends. Let's add those: {rivalWeekday} + {rivalWeekend} = {rivalWeekday + rivalWeekend} total hours!");
        problem.Hints.Add($"⏰ HINT 3: You practice x hours on weekdays and {yourWeekend} hours on weekends. So your total is x + {yourWeekend} hours.");
        problem.Hints.Add($"➗ HINT 4: To match them: x + {yourWeekend} = {rivalWeekday + rivalWeekend}. Subtract {yourWeekend} from both sides: x = {rivalWeekday + rivalWeekend} - {yourWeekend}");

        return problem;
    }

    private CheerProblem GenerateComplexFormation()
    {
        int coefficient = _random.Next(3, 6);
        int constant = _random.Next(-10, 11);
        int result = _random.Next(20, 50);

        string equation = $"{coefficient}x + {constant} = {result}";
        var solution = _solver.Solve(_parser.Parse(equation), "x");

        var problem = new CheerProblem
        {
            Title = "💫 COMPLEX FORMATION CHALLENGE",
            Story = $"You're choreographing a complex formation! Each section needs {coefficient} cheerleaders (x sections total)" +
                   (constant >= 0 ? $", plus {constant} spotters" : $", minus {-constant} who are resting") +
                   $". The total must be {result} people. How many sections do you need?",
            Equation = equation,
            Variable = "x",
            VariableDescription = "number of sections",
            Answer = solution.Value,
            Steps = solution.Steps,
            Difficulty = DifficultyLevel.Hard,
            Emoji = "💫"
        };

        // Add 5th-grader friendly hints
        problem.Hints.Add($"💡 HINT 1: You need {result} people total for this formation!");
        if (constant >= 0)
        {
            problem.Hints.Add($"👥 HINT 2: You have {constant} spotters who are extra (not in the sections). So subtract them first: {result} - {constant} = {result - constant} people in the sections.");
        }
        else
        {
            problem.Hints.Add($"👥 HINT 2: You have {-constant} people resting (not in the formation). So add them back: {result} + {-constant} = {result - constant} people needed in the sections.");
        }
        problem.Hints.Add($"🎯 HINT 3: Now you have {result - constant} people to divide into sections, and each section has {coefficient} cheerleaders.");
        problem.Hints.Add($"➗ HINT 4: To find how many sections you need, divide: {result - constant} ÷ {coefficient}. What's the answer?");

        return problem;
    }

    private CheerProblem GenerateMultiStepFundraiser()
    {
        int itemPrice = _random.Next(4, 10);
        int alreadyRaised = _random.Next(30, 80);

        // Ensure whole number answer
        int itemsNeeded = _random.Next(15, 40);
        int goal = itemPrice * itemsNeeded + alreadyRaised;

        string equation = $"{itemPrice}x + {alreadyRaised} = {goal}";
        var solution = _solver.Solve(_parser.Parse(equation), "x");

        var problem = new CheerProblem
        {
            Title = "💰 CHAMPIONSHIP FUNDRAISER",
            Story = $"Your squad needs ${goal} for the championship trip! You've raised ${alreadyRaised} from donations. " +
                   $"Now you're selling spirit wear for ${itemPrice} each. How many items must you sell to reach your goal?",
            Equation = equation,
            Variable = "x",
            VariableDescription = "spirit wear items",
            Answer = solution.Value,
            Steps = solution.Steps,
            Difficulty = DifficultyLevel.Hard,
            Emoji = "💰"
        };

        // Add 5th-grader friendly hints
        problem.Hints.Add($"💡 HINT 1: Your big goal is ${goal} for the championship trip! That's a lot of money!");
        problem.Hints.Add($"💰 HINT 2: Good news! You already have ${alreadyRaised} from donations. So you don't need to raise ALL of it. Subtract what you have: ${goal} - ${alreadyRaised} = ${goal - alreadyRaised}");
        problem.Hints.Add($"🎯 HINT 3: You still need ${goal - alreadyRaised} more dollars from selling spirit wear!");
        problem.Hints.Add($"➗ HINT 4: Each spirit wear item costs ${itemPrice}. To find how many you need to sell, divide the money you still need by the price: ${goal - alreadyRaised} ÷ ${itemPrice}");

        return problem;
    }
}

/// <summary>
/// Represents a cheerleading-themed algebra problem
/// </summary>
public class CheerProblem
{
    public string Title { get; set; } = "";
    public string Story { get; set; } = "";
    public string Equation { get; set; } = "";
    public string Variable { get; set; } = "";
    public string VariableDescription { get; set; } = "";
    public decimal Answer { get; set; }
    public List<string> Steps { get; set; } = new();
    public List<string> Hints { get; set; } = new();
    public DifficultyLevel Difficulty { get; set; }
    public string Emoji { get; set; } = "";
}

