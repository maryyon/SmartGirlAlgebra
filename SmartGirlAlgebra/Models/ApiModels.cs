namespace SmartGirlAlgebra.Models;

// Authentication Models
public class RegisterRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}

// Progress Models
public class UserStats
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int TotalProblemsAttempted { get; set; }
    public int TotalCorrect { get; set; }
    public int CurrentStreak { get; set; }
    public int BestStreak { get; set; }
    public int TotalScore { get; set; }
    public DateTime? LastPlayedDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

