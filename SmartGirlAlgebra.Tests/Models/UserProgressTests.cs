using SmartGirlAlgebra.Models;
using Xunit;

namespace SmartGirlAlgebra.Tests.Models;

public class UserProgressTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaults()
    {
        var problem = new Problem("Test", 5, DifficultyLevel.Easy, "Test");
        var progress = new UserProgress(problem);
        
        Assert.Equal(problem, progress.Problem);
        Assert.Equal(0, progress.Attempts);
        Assert.False(progress.IsSolved);
        Assert.Equal(0, progress.HintsUsed);
        Assert.Null(progress.CompletedAt);
        Assert.True(progress.StartedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void RecordAttempt_WithCorrectAnswer_ShouldMarkAsSolved()
    {
        var problem = new Problem("Test", 5, DifficultyLevel.Easy, "Test");
        var progress = new UserProgress(problem);
        
        progress.RecordAttempt(5);
        
        Assert.Equal(1, progress.Attempts);
        Assert.True(progress.IsSolved);
        Assert.NotNull(progress.CompletedAt);
    }

    [Fact]
    public void RecordAttempt_WithIncorrectAnswer_ShouldNotMarkAsSolved()
    {
        var problem = new Problem("Test", 5, DifficultyLevel.Easy, "Test");
        var progress = new UserProgress(problem);
        
        progress.RecordAttempt(3);
        
        Assert.Equal(1, progress.Attempts);
        Assert.False(progress.IsSolved);
        Assert.Null(progress.CompletedAt);
    }

    [Fact]
    public void RecordAttempt_MultipleAttempts_ShouldIncrementCounter()
    {
        var problem = new Problem("Test", 5, DifficultyLevel.Easy, "Test");
        var progress = new UserProgress(problem);
        
        progress.RecordAttempt(3);
        progress.RecordAttempt(4);
        progress.RecordAttempt(5);
        
        Assert.Equal(3, progress.Attempts);
        Assert.True(progress.IsSolved);
    }

    [Fact]
    public void UseHint_ShouldIncrementHintsUsed()
    {
        var problem = new Problem("Test", 5, DifficultyLevel.Easy, "Test");
        var progress = new UserProgress(problem);
        
        progress.UseHint();
        progress.UseHint();
        
        Assert.Equal(2, progress.HintsUsed);
    }
}

