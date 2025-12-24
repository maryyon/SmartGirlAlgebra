using SmartGirlAlgebra.Models;
using SmartGirlAlgebra.Services;
using Xunit;
using Xunit.Abstractions;

namespace SmartGirlAlgebra.Tests;

/// <summary>
/// Demo showing what Layer 1 and Layer 2 can do
/// </summary>
public class DemoProgram
{
    private readonly ITestOutputHelper _output;

    public DemoProgram(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Demo_Layer1_DataModels()
    {
        _output.WriteLine("=== LAYER 1: DATA MODELS ===\n");

        // Create terms manually
        _output.WriteLine("Creating terms:");
        var term1 = new Term(3, "x");
        var term2 = new Term(5);
        _output.WriteLine($"  Term 1: {term1}");  // Output: 3x
        _output.WriteLine($"  Term 2: {term2}");  // Output: 5

        // Create an expression manually
        _output.WriteLine("\nCreating expression:");
        var expr = new Expression();
        expr.AddTerm(new Term(2, "x"));
        expr.AddTerm(new Term(-3));
        _output.WriteLine($"  Expression: {expr}");  // Output: 2x - 3

        // Create a problem
        _output.WriteLine("\nCreating problem:");
        var problem = new Problem("Solve: 2x + 3 = 11", 4, DifficultyLevel.Easy, "LinearEquation");
        _output.WriteLine($"  Question: {problem.Question}");
        _output.WriteLine($"  Answer: {problem.Answer}");
        _output.WriteLine($"  Difficulty: {problem.Difficulty}");

        // Track user progress
        _output.WriteLine("\nTracking user progress:");
        var progress = new UserProgress(problem);
        progress.RecordAttempt(3);  // Wrong answer
        _output.WriteLine($"  Attempt 1: Wrong (Solved: {progress.IsSolved})");
        progress.RecordAttempt(4);  // Correct answer
        _output.WriteLine($"  Attempt 2: Correct (Solved: {progress.IsSolved})");
        _output.WriteLine($"  Total attempts: {progress.Attempts}");
    }

    [Fact]
    public void Demo_Layer2_ExpressionEngine()
    {
        _output.WriteLine("=== LAYER 2: EXPRESSION ENGINE ===\n");

        var parser = new ExpressionParser();
        var simplifier = new ExpressionSimplifier();
        var evaluator = new ExpressionEvaluator();

        // DEMO 1: Parse a string into an expression
        _output.WriteLine("DEMO 1: Parsing");
        string input = "2x + 3";
        var expr = parser.Parse(input);
        _output.WriteLine($"  Input: \"{input}\"");
        _output.WriteLine($"  Parsed: {expr}");
        _output.WriteLine($"  Number of terms: {expr.Terms.Count}");

        // DEMO 2: Simplify an expression
        _output.WriteLine("\nDEMO 2: Simplifying");
        string complexInput = "2x + 3x + 5 + 2";
        var complexExpr = parser.Parse(complexInput);
        _output.WriteLine($"  Input: \"{complexInput}\"");
        _output.WriteLine($"  Before simplify: {complexExpr}");
        var simplified = simplifier.Simplify(complexExpr);
        _output.WriteLine($"  After simplify: {simplified}");  // Output: 5x + 7

        // DEMO 3: Evaluate an expression
        _output.WriteLine("\nDEMO 3: Evaluating");
        string evalInput = "2x + 3";
        var evalExpr = parser.Parse(evalInput);
        _output.WriteLine($"  Expression: \"{evalInput}\"");
        _output.WriteLine($"  When x = 5:");
        var result = evaluator.Evaluate(evalExpr, "x", 5);
        _output.WriteLine($"    Result: {result}");  // Output: 13 (2*5 + 3)
        _output.WriteLine($"  When x = 10:");
        result = evaluator.Evaluate(evalExpr, "x", 10);
        _output.WriteLine($"    Result: {result}");  // Output: 23 (2*10 + 3)

        // DEMO 4: Complete workflow
        _output.WriteLine("\nDEMO 4: Complete Workflow");
        string workflow = "3x + 2x - 5 + 10";
        _output.WriteLine($"  Original: \"{workflow}\"");
        
        var step1 = parser.Parse(workflow);
        _output.WriteLine($"  Step 1 - Parsed: {step1}");
        
        var step2 = simplifier.Simplify(step1);
        _output.WriteLine($"  Step 2 - Simplified: {step2}");  // Output: 5x + 5
        
        var step3 = evaluator.Evaluate(step2, "x", 3);
        _output.WriteLine($"  Step 3 - Evaluated (x=3): {step3}");  // Output: 20 (5*3 + 5)
    }
}

