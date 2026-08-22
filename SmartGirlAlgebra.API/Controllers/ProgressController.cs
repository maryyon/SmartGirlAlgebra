using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartGirlAlgebra.API.Data;
using SmartGirlAlgebra.API.Models;
using SmartGirlAlgebra.API.Services;

namespace SmartGirlAlgebra.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProgressController : ControllerBase
{
    private const string CodeHeader = "X-Player-Code";

    private readonly ApplicationDbContext _context;

    public ProgressController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<PlayerResponse>> GetProgress()
    {
        var player = await ResolvePlayerAsync();
        if (player == null) return Unauthorized(new { message = "Unknown or missing player code." });

        return Ok(PlayerResponse.From(player));
    }

    /// <summary>
    /// Stores the player's running totals. The client owns the arithmetic; the server
    /// keeps whichever totals are higher so a stale device can never erase real progress.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<PlayerResponse>> UpdateProgress([FromBody] ProgressUpdate update)
    {
        var player = await ResolvePlayerAsync();
        if (player == null) return Unauthorized(new { message = "Unknown or missing player code." });

        player.TotalProblemsAttempted = Math.Max(player.TotalProblemsAttempted, update.TotalProblemsAttempted);
        player.TotalCorrect = Math.Max(player.TotalCorrect, update.TotalCorrect);
        player.TotalScore = Math.Max(player.TotalScore, update.TotalScore);
        player.BestStreak = Math.Max(player.BestStreak, update.BestStreak);

        // The current streak is genuinely current, so it takes the client's value.
        player.CurrentStreak = update.CurrentStreak;

        player.LastPlayedDate = DateTime.UtcNow;
        player.LastSeenAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(PlayerResponse.From(player));
    }

    private async Task<Player?> ResolvePlayerAsync()
    {
        if (!Request.Headers.TryGetValue(CodeHeader, out var raw)) return null;

        var code = SyncCodeGenerator.Normalize(raw.ToString());
        if (code == null) return null;

        return await _context.Players.FirstOrDefaultAsync(p => p.Code == code);
    }
}
