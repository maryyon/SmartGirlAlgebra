using SmartGirlAlgebra.Models;
using SmartGirlAlgebra.Services;
using Xunit;
using Xunit.Abstractions;

namespace SmartGirlAlgebra.Tests;

/// <summary>
/// Demo showing the Problem Generator creating random cheerleading problems
/// </summary>
public class ProblemGeneratorDemo
{
    private readonly ITestOutputHelper _output;
    private readonly ProblemGenerator _generator = new();

    public ProblemGeneratorDemo(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Demo_GenerateRandomProblems()
    {
        _output.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        _output.WriteLine("║  🎀 LAYER 4: PROBLEM GENERATOR 🎀                             ║");
        _output.WriteLine("║  Automatically creates cheerleading-themed algebra problems   ║");
        _output.WriteLine("╚════════════════════════════════════════════════════════════════╝");
        _output.WriteLine("");

        // Generate Easy Problems
        _output.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        _output.WriteLine("📗 EASY DIFFICULTY PROBLEMS");
        _output.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        _output.WriteLine("");

        for (int i = 1; i <= 3; i++)
        {
            var problem = _generator.GenerateLinearEquation(DifficultyLevel.Easy);
            DisplayProblem(i, problem);
        }

        // Generate Medium Problems
        _output.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        _output.WriteLine("📘 MEDIUM DIFFICULTY PROBLEMS");
        _output.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        _output.WriteLine("");

        for (int i = 1; i <= 2; i++)
        {
            var problem = _generator.GenerateLinearEquation(DifficultyLevel.Medium);
            DisplayProblem(i, problem);
        }

        // Generate Hard Problems
        _output.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        _output.WriteLine("📕 HARD DIFFICULTY PROBLEMS");
        _output.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        _output.WriteLine("");

        for (int i = 1; i <= 2; i++)
        {
            var problem = _generator.GenerateLinearEquation(DifficultyLevel.Hard);
            DisplayProblem(i, problem);
        }

        _output.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        _output.WriteLine("✨ KEY FEATURES:");
        _output.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        _output.WriteLine("");
        _output.WriteLine("✅ Randomly generates unique problems each time");
        _output.WriteLine("✅ All problems have cheerleading themes");
        _output.WriteLine("✅ Answers are always positive whole numbers (realistic)");
        _output.WriteLine("✅ Includes step-by-step solutions");
        _output.WriteLine("✅ Difficulty scales appropriately");
        _output.WriteLine("✅ Variety of problem types:");
        _output.WriteLine("   • Shopping/Budgeting");
        _output.WriteLine("   • Timing/Scheduling");
        _output.WriteLine("   • Fundraising");
        _output.WriteLine("   • Squad Management");
        _output.WriteLine("   • Competition Scoring");
        _output.WriteLine("   • Formation Planning");
        _output.WriteLine("");
        _output.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        _output.WriteLine("║  🎀 Infinite practice problems at your fingertips! 🎀         ║");
        _output.WriteLine("╚════════════════════════════════════════════════════════════════╝");
    }

    private void DisplayProblem(int number, CheerProblem problem)
    {
        _output.WriteLine($"PROBLEM {number}: {problem.Title}");
        _output.WriteLine("─────────────────────────────────────────────────────────────────");
        _output.WriteLine($"{problem.Story}");
        _output.WriteLine("");
        _output.WriteLine($"💭 Let {problem.Variable} = {problem.VariableDescription}");
        _output.WriteLine($"📝 Equation: {problem.Equation}");
        _output.WriteLine($"✨ Answer: {problem.Variable} = {problem.Answer}");
        _output.WriteLine("");
        _output.WriteLine("📋 Solution Steps:");
        foreach (var step in problem.Steps)
        {
            _output.WriteLine($"   {step}");
        }
        _output.WriteLine("");
    }

    [Fact]
    public void Demo_ShowVariety()
    {
        _output.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        _output.WriteLine("║  🎀 PROBLEM VARIETY DEMONSTRATION 🎀                          ║");
        _output.WriteLine("╚════════════════════════════════════════════════════════════════╝");
        _output.WriteLine("");
        _output.WriteLine("Generating 10 easy problems to show variety...");
        _output.WriteLine("");

        var titles = new HashSet<string>();
        var equations = new HashSet<string>();

        for (int i = 1; i <= 10; i++)
        {
            var problem = _generator.GenerateLinearEquation(DifficultyLevel.Easy);
            titles.Add(problem.Title);
            equations.Add(problem.Equation);
            _output.WriteLine($"{i}. {problem.Emoji} {problem.Title}");
            _output.WriteLine($"   Equation: {problem.Equation}");
            _output.WriteLine($"   Answer: {problem.Answer}");
            _output.WriteLine("");
        }

        _output.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        _output.WriteLine($"📊 VARIETY STATS:");
        _output.WriteLine($"   Unique problem types: {titles.Count}");
        _output.WriteLine($"   Unique equations: {equations.Count}");
        _output.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
    }
}

