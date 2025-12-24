using SmartGirlAlgebra.Models;
using Xunit;

namespace SmartGirlAlgebra.Tests.Models;

public class ProblemTests
{
    [Fact]
    public void Constructor_ShouldSetAllProperties()
    {
        var problem = new Problem("Solve for x", 5, DifficultyLevel.Easy, "LinearEquation");
        
        Assert.NotEqual(Guid.Empty, problem.Id);
        Assert.Equal("Solve for x", problem.Question);
        Assert.Equal(5, problem.Answer);
        Assert.Equal(DifficultyLevel.Easy, problem.Difficulty);
        Assert.Equal("LinearEquation", problem.ProblemType);
    }

    [Fact]
    public void Constructor_ShouldGenerateUniqueIds()
    {
        var problem1 = new Problem("Question 1", 1, DifficultyLevel.Easy, "Type1");
        var problem2 = new Problem("Question 2", 2, DifficultyLevel.Easy, "Type1");
        
        Assert.NotEqual(problem1.Id, problem2.Id);
    }
}

