namespace SmartGirlAlgebra.Models;

/// <summary>
/// Tracks a user's progress on a specific problem
/// </summary>
public class UserProgress
{
    /// <summary>
    /// The problem being worked on
    /// </summary>
    public Problem Problem { get; set; }

    /// <summary>
    /// Number of attempts made
    /// </summary>
    public int Attempts { get; set; }

    /// <summary>
    /// Whether the problem was solved correctly
    /// </summary>
    public bool IsSolved { get; set; }

    /// <summary>
    /// Number of hints used
    /// </summary>
    public int HintsUsed { get; set; }

    /// <summary>
    /// When the user started this problem
    /// </summary>
    public DateTime StartedAt { get; set; }

    /// <summary>
    /// When the user completed this problem (if solved)
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    public UserProgress(Problem problem)
    {
        Problem = problem;
        Attempts = 0;
        IsSolved = false;
        HintsUsed = 0;
        StartedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Records an attempt to solve the problem
    /// </summary>
    public void RecordAttempt(decimal userAnswer)
    {
        Attempts++;
        
        if (Math.Abs(userAnswer - Problem.Answer) < 0.001m)
        {
            IsSolved = true;
            CompletedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Records that a hint was used
    /// </summary>
    public void UseHint()
    {
        HintsUsed++;
    }
}

