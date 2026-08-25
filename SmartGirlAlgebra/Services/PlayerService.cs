using System.Net.Http.Json;
using Microsoft.JSInterop;
using SmartGirlAlgebra.Models;

namespace SmartGirlAlgebra.Services;

/// <summary>
/// Owns the player's identity — a sync code, never an account. The code is created
/// silently the first time she answers something, so nothing is ever gated behind
/// a sign-up, and typing it on another device brings her progress with her.
///
/// Everything is scoped to a version: storage keys, the code prefix, and therefore
/// the totals. A younger child's streak never mixes with an older one's, and a code
/// from one version will not resolve in another.
///
/// Totals are kept locally so the UI never waits on the network; the server is
/// updated in the background and is allowed to fail quietly. Offline still plays.
/// </summary>
public class PlayerService
{
    private const string CodeHeader = "X-Player-Code";

    private readonly HttpClient _http;
    private readonly IJSRuntime _js;

    private string _profileId = "";
    private string _codePrefix = "SGA";

    public PlayerService(HttpClient http, IJSRuntime js)
    {
        _http = http;
        _js = js;
    }

    public string? Code { get; private set; }
    public PlayerResponse Stats { get; private set; } = new();

    public event Action? Changed;

    public bool HasIdentity => !string.IsNullOrEmpty(Code);

    private string CodeKey => $"sgaPlayerCode:{_profileId}";
    private string StatsKey => $"sgaPlayerStats:{_profileId}";

    /// <summary>
    /// Points the player at a version and loads that version's saved progress.
    /// Switching versions swaps identity wholesale rather than carrying anything over.
    /// </summary>
    public async Task UseProfileAsync(Profile profile)
    {
        if (_profileId == profile.Id) return;

        _profileId = profile.Id;
        _codePrefix = string.IsNullOrWhiteSpace(profile.CodePrefix) ? "SGA" : profile.CodePrefix;

        Code = null;
        Stats = new PlayerResponse();
        _http.DefaultRequestHeaders.Remove(CodeHeader);

        var cached = await LoadStatsAsync();
        if (cached is not null) Stats = cached;

        Code = await GetStoredCodeAsync();
        if (!string.IsNullOrEmpty(Code))
        {
            ApplyHeader(Code);

            // Reconcile with the server, taking whichever side has seen more work,
            // so playing offline on one device never erases the other.
            try
            {
                var remote = await _http.GetFromJsonAsync<PlayerResponse>("api/progress");
                if (remote is not null) Merge(remote);
            }
            catch
            {
                // Offline, or a cold API still waking up — local totals stand.
            }
        }

        Changed?.Invoke();
    }

    private void Merge(PlayerResponse remote)
    {
        Stats = new PlayerResponse
        {
            Code = remote.Code,
            TotalProblemsAttempted = Math.Max(Stats.TotalProblemsAttempted, remote.TotalProblemsAttempted),
            TotalCorrect = Math.Max(Stats.TotalCorrect, remote.TotalCorrect),
            BestStreak = Math.Max(Stats.BestStreak, remote.BestStreak),
            TotalScore = Math.Max(Stats.TotalScore, remote.TotalScore),
            CurrentStreak = remote.CurrentStreak,
            LastPlayedDate = remote.LastPlayedDate
        };
    }

    /// <summary>Creates a code if she doesn't have one yet. Safe to call repeatedly.</summary>
    public async Task EnsureIdentityAsync()
    {
        if (HasIdentity) return;

        try
        {
            var response = await _http.PostAsync($"api/identity/new?prefix={Uri.EscapeDataString(_codePrefix)}", null);
            if (!response.IsSuccessStatusCode) return;

            var player = await response.Content.ReadFromJsonAsync<PlayerResponse>();
            if (player is null || string.IsNullOrEmpty(player.Code)) return;

            Code = player.Code;
            Stats = player;
            await StoreCodeAsync(player.Code);
            ApplyHeader(player.Code);
            Changed?.Invoke();
        }
        catch
        {
            // No identity yet. She keeps playing; we'll try again next answer.
        }
    }

    /// <summary>Adopts an existing code typed in from another device.</summary>
    public async Task<(bool Ok, string? Error)> ClaimAsync(string code)
    {
        // Reject another version's code before troubling the server: the child would
        // otherwise be told "not found" for a code that is perfectly valid elsewhere.
        var typed = new string((code ?? "").ToUpperInvariant().Where(char.IsLetterOrDigit).ToArray());
        if (typed.Length > 6 && !typed.StartsWith(_codePrefix, StringComparison.OrdinalIgnoreCase))
        {
            return (false, $"That code belongs to a different app. This one's codes start with {_codePrefix}.");
        }

        try
        {
            var response = await _http.PostAsJsonAsync("api/identity/claim", new ClaimRequest { Code = code ?? "" });

            if (!response.IsSuccessStatusCode)
            {
                return (false, "We couldn't find that code. Check it and try again.");
            }

            var player = await response.Content.ReadFromJsonAsync<PlayerResponse>();
            if (player is null) return (false, "Something went wrong. Try again in a moment.");

            Code = player.Code;
            Stats = player;
            await StoreCodeAsync(player.Code);
            ApplyHeader(player.Code);
            await SaveStatsAsync();
            Changed?.Invoke();
            return (true, null);
        }
        catch
        {
            return (false, "We couldn't reach the squad right now. Check your connection.");
        }
    }

    /// <summary>
    /// Records one answered problem and pushes the new totals up. Called on every
    /// submission, so it must stay cheap and never throw into the UI.
    /// </summary>
    public async Task RecordAttemptAsync(bool correct, int pointsEarned)
    {
        await EnsureIdentityAsync();

        Stats.TotalProblemsAttempted++;

        if (correct)
        {
            Stats.TotalCorrect++;
            Stats.CurrentStreak++;
            Stats.TotalScore += pointsEarned;
            if (Stats.CurrentStreak > Stats.BestStreak) Stats.BestStreak = Stats.CurrentStreak;
        }
        else
        {
            Stats.CurrentStreak = 0;
        }

        Stats.LastPlayedDate = DateTime.UtcNow;
        await SaveStatsAsync();
        Changed?.Invoke();

        if (!HasIdentity) return;

        try
        {
            await _http.PostAsJsonAsync("api/progress", new ProgressUpdate
            {
                TotalProblemsAttempted = Stats.TotalProblemsAttempted,
                TotalCorrect = Stats.TotalCorrect,
                CurrentStreak = Stats.CurrentStreak,
                BestStreak = Stats.BestStreak,
                TotalScore = Stats.TotalScore
            });
        }
        catch
        {
            // Best effort. Her totals are already correct on screen and will
            // catch up on the next successful post.
        }
    }

    private void ApplyHeader(string code)
    {
        _http.DefaultRequestHeaders.Remove(CodeHeader);
        _http.DefaultRequestHeaders.Add(CodeHeader, code);
    }

    private async Task<string?> GetStoredCodeAsync()
    {
        try { return await _js.InvokeAsync<string?>("localStorage.getItem", CodeKey); }
        catch { return null; }
    }

    private async Task StoreCodeAsync(string code)
    {
        try { await _js.InvokeVoidAsync("localStorage.setItem", CodeKey, code); }
        catch { /* private browsing — she can still play this session */ }
    }

    private async Task<PlayerResponse?> LoadStatsAsync()
    {
        try
        {
            var json = await _js.InvokeAsync<string?>("localStorage.getItem", StatsKey);
            return string.IsNullOrEmpty(json)
                ? null
                : System.Text.Json.JsonSerializer.Deserialize<PlayerResponse>(json);
        }
        catch
        {
            return null;
        }
    }

    private async Task SaveStatsAsync()
    {
        try
        {
            var json = System.Text.Json.JsonSerializer.Serialize(Stats);
            await _js.InvokeVoidAsync("localStorage.setItem", StatsKey, json);
        }
        catch { /* storage unavailable — the session's totals still show */ }
    }
}
