using System.ComponentModel.DataAnnotations;

namespace SmartGirlAlgebra.API.Models;

/// <summary>
/// A player, identified only by a generated sync code. No personal information
/// is collected or stored — the code is the entire identity.
/// </summary>
public class Player
{
    public int Id { get; set; }

    /// <summary>Sync code in the form SGA-XXXXXX. Unique, and acts as the credential.</summary>
    [MaxLength(16)]
    public string Code { get; set; } = string.Empty;

    public int TotalProblemsAttempted { get; set; }
    public int TotalCorrect { get; set; }
    public int CurrentStreak { get; set; }
    public int BestStreak { get; set; }
    public int TotalScore { get; set; }

    /// <summary>Number of times this code has been claimed on a new device.</summary>
    public int DeviceCount { get; set; } = 1;

    public DateTime? LastPlayedDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastSeenAt { get; set; } = DateTime.UtcNow;
}
