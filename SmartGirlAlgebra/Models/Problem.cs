namespace SmartGirlAlgebra.Models;

/// <summary>
/// Difficulty level for problems
/// </summary>
public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard
}

/// <summary>
/// Represents an algebra problem to solve
/// </summary>
public class Problem
{
    /// <summary>
    /// Unique identifier for this problem
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The question text (e.g., "Solve for x")
    /// </summary>
    public string Question { get; set; }

    /// <summary>
    /// The correct answer
    /// </summary>
    public decimal Answer { get; set; }

    /// <summary>
    /// Difficulty level
    /// </summary>
    public DifficultyLevel Difficulty { get; set; }

    /// <summary>
    /// The type of problem (e.g., "LinearEquation", "Simplify")
    /// </summary>
    public string ProblemType { get; set; }

    public Problem(string question, decimal answer, DifficultyLevel difficulty, string problemType)
    {
        Id = Guid.NewGuid();
        Question = question;
        Answer = answer;
        Difficulty = difficulty;
        ProblemType = problemType;
    }
}

