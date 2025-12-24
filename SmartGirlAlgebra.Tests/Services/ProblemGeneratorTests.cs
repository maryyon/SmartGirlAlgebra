using SmartGirlAlgebra.Models;
using SmartGirlAlgebra.Services;
using Xunit;

namespace SmartGirlAlgebra.Tests.Services;

public class ProblemGeneratorTests
{
    private readonly ProblemGenerator _generator = new();

    [Fact]
    public void GenerateLinearEquation_Easy_ShouldReturnValidProblem()
    {
        var problem = _generator.GenerateLinearEquation(DifficultyLevel.Easy);
        
        Assert.NotNull(problem);
        Assert.NotEmpty(problem.Title);
        Assert.NotEmpty(problem.Story);
        Assert.NotEmpty(problem.Equation);
        Assert.NotEmpty(problem.Variable);
        Assert.NotEmpty(problem.VariableDescription);
        Assert.True(problem.Answer > 0);
        Assert.NotEmpty(problem.Steps);
        Assert.Equal(DifficultyLevel.Easy, problem.Difficulty);
    }

    [Fact]
    public void GenerateLinearEquation_Medium_ShouldReturnValidProblem()
    {
        var problem = _generator.GenerateLinearEquation(DifficultyLevel.Medium);
        
        Assert.NotNull(problem);
        Assert.NotEmpty(problem.Title);
        Assert.NotEmpty(problem.Story);
        Assert.NotEmpty(problem.Equation);
        Assert.Equal(DifficultyLevel.Medium, problem.Difficulty);
    }

    [Fact]
    public void GenerateLinearEquation_Hard_ShouldReturnValidProblem()
    {
        var problem = _generator.GenerateLinearEquation(DifficultyLevel.Hard);
        
        Assert.NotNull(problem);
        Assert.NotEmpty(problem.Title);
        Assert.NotEmpty(problem.Story);
        Assert.NotEmpty(problem.Equation);
        Assert.Equal(DifficultyLevel.Hard, problem.Difficulty);
    }

    [Fact]
    public void GenerateLinearEquation_ShouldHaveCorrectAnswer()
    {
        var problem = _generator.GenerateLinearEquation(DifficultyLevel.Easy);
        
        // Verify the answer is correct by solving the equation
        var parser = new EquationParser();
        var solver = new LinearEquationSolver();
        
        var equation = parser.Parse(problem.Equation);
        var solution = solver.Solve(equation, problem.Variable);
        
        Assert.Equal(solution.Value, problem.Answer);
    }

    [Fact]
    public void GenerateLinearEquation_ShouldHaveSteps()
    {
        var problem = _generator.GenerateLinearEquation(DifficultyLevel.Easy);
        
        Assert.NotEmpty(problem.Steps);
        Assert.Contains("Original equation", problem.Steps[0]);
        Assert.Contains("Solution", problem.Steps[^1]);
    }

    [Fact]
    public void GenerateLinearEquation_ShouldHaveEmoji()
    {
        var problem = _generator.GenerateLinearEquation(DifficultyLevel.Easy);
        
        Assert.NotEmpty(problem.Emoji);
    }

    [Fact]
    public void GenerateLinearEquation_MultipleGenerations_ShouldVary()
    {
        var problems = new List<CheerProblem>();
        
        for (int i = 0; i < 10; i++)
        {
            problems.Add(_generator.GenerateLinearEquation(DifficultyLevel.Easy));
        }
        
        // Should have some variety in the problems
        var uniqueTitles = problems.Select(p => p.Title).Distinct().Count();
        Assert.True(uniqueTitles > 1, "Should generate different types of problems");
    }

    [Fact]
    public void GenerateLinearEquation_ShouldHaveCheerleadingTheme()
    {
        var problem = _generator.GenerateLinearEquation(DifficultyLevel.Easy);
        
        // Story should contain cheerleading-related words
        var story = problem.Story.ToLower();
        var hasCheerTheme = story.Contains("squad") || 
                           story.Contains("pom-pom") || 
                           story.Contains("cheer") ||
                           story.Contains("routine") ||
                           story.Contains("pyramid") ||
                           story.Contains("practice") ||
                           story.Contains("competition") ||
                           story.Contains("fundraiser");
        
        Assert.True(hasCheerTheme, "Problem should have cheerleading theme");
    }

    [Fact]
    public void GenerateLinearEquation_AnswerShouldBePositive()
    {
        for (int i = 0; i < 20; i++)
        {
            var problem = _generator.GenerateLinearEquation(DifficultyLevel.Easy);
            Assert.True(problem.Answer > 0, "Answer should be positive for real-world context");
        }
    }

    [Fact]
    public void GenerateLinearEquation_AnswerShouldBeReasonable()
    {
        for (int i = 0; i < 20; i++)
        {
            var problem = _generator.GenerateLinearEquation(DifficultyLevel.Easy);
            Assert.True(problem.Answer < 1000, "Answer should be reasonable (not too large)");
        }
    }
}

