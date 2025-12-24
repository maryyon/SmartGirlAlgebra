using SmartGirlAlgebra.API.Data;
using SmartGirlAlgebra.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace SmartGirlAlgebra.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProgressController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProgressController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<UserProgress>> GetProgress()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var progress = await _context.UserProgress
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (progress == null)
        {
            // Create if doesn't exist
            progress = new UserProgress { UserId = userId };
            _context.UserProgress.Add(progress);
            await _context.SaveChangesAsync();
        }

        return Ok(progress);
    }

    [HttpPost("update")]
    public async Task<ActionResult<UserProgress>> UpdateProgress([FromBody] UserProgress updatedProgress)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var progress = await _context.UserProgress
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (progress == null)
        {
            progress = new UserProgress { UserId = userId };
            _context.UserProgress.Add(progress);
        }

        // Update fields
        progress.TotalProblemsAttempted = updatedProgress.TotalProblemsAttempted;
        progress.TotalCorrect = updatedProgress.TotalCorrect;
        progress.CurrentStreak = updatedProgress.CurrentStreak;
        progress.BestStreak = updatedProgress.BestStreak;
        progress.TotalScore = updatedProgress.TotalScore;
        progress.LastPlayedDate = DateTime.UtcNow;
        progress.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(progress);
    }
}

