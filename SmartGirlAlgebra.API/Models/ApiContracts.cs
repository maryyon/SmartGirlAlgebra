namespace SmartGirlAlgebra.API.Models;

/// <summary>Returned whenever the server hands back a player's identity and stats.</summary>
public class PlayerResponse
{
    public string Code { get; set; } = string.Empty;
    public int TotalProblemsAttempted { get; set; }
    public int TotalCorrect { get; set; }
    public int CurrentStreak { get; set; }
    public int BestStreak { get; set; }
    public int TotalScore { get; set; }
    public DateTime? LastPlayedDate { get; set; }

    public static PlayerResponse From(Player p) => new()
    {
        Code = p.Code,
        TotalProblemsAttempted = p.TotalProblemsAttempted,
        TotalCorrect = p.TotalCorrect,
        CurrentStreak = p.CurrentStreak,
        BestStreak = p.BestStreak,
        TotalScore = p.TotalScore,
        LastPlayedDate = p.LastPlayedDate
    };
}

public class ClaimRequest
{
    public string Code { get; set; } = string.Empty;
}

/// <summary>Stats posted by the client after a practice session.</summary>
public class ProgressUpdate
{
    public int TotalProblemsAttempted { get; set; }
    public int TotalCorrect { get; set; }
    public int CurrentStreak { get; set; }
    public int BestStreak { get; set; }
    public int TotalScore { get; set; }
}
