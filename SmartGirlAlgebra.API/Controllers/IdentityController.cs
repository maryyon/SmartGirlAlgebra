using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartGirlAlgebra.API.Data;
using SmartGirlAlgebra.API.Models;
using SmartGirlAlgebra.API.Services;

namespace SmartGirlAlgebra.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public IdentityController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Issues a brand new sync code. Called silently by the app the first time
    /// someone answers a problem — the player never has to ask for it.
    ///
    /// The prefix names the version, so progress never mixes between them.
    /// </summary>
    [HttpPost("new")]
    public async Task<ActionResult<PlayerResponse>> CreateNew([FromQuery] string? prefix = null)
    {
        // Retry on the vanishingly unlikely collision rather than trusting one draw.
        for (var attempt = 0; attempt < 5; attempt++)
        {
            var code = SyncCodeGenerator.Generate(prefix);
            if (await _context.Players.AnyAsync(p => p.Code == code)) continue;

            var player = new Player { Code = code };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return Ok(PlayerResponse.From(player));
        }

        return StatusCode(500, new { message = "Could not allocate a code, please try again." });
    }

    /// <summary>
    /// Claims an existing code on another device so progress follows the player.
    /// </summary>
    [HttpPost("claim")]
    public async Task<ActionResult<PlayerResponse>> Claim([FromBody] ClaimRequest request)
    {
        var code = SyncCodeGenerator.Normalize(request.Code);
        if (code == null)
        {
            return BadRequest(new { message = "That doesn't look like a code. Codes have a few letters, a dash, then six characters." });
        }

        var player = await _context.Players.FirstOrDefaultAsync(p => p.Code == code);
        if (player == null)
        {
            return NotFound(new { message = "We couldn't find that code. Check it and try again." });
        }

        player.DeviceCount++;
        player.LastSeenAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(PlayerResponse.From(player));
    }
}
