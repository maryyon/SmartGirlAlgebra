namespace SmartGirlAlgebra.API.Models;

public class UserProgress
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    
    public int TotalProblemsAttempted { get; set; }
    public int TotalCorrect { get; set; }
    public int CurrentStreak { get; set; }
    public int BestStreak { get; set; }
    public int TotalScore { get; set; }
    
    public DateTime? LastPlayedDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation property
    public ApplicationUser? User { get; set; }
}

