using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;
using SmartGirlAlgebra.Models;

namespace SmartGirlAlgebra.Services;

public class ProfileEntry
{
    public string Id { get; set; } = "";

    /// <summary>Repeated from the version's own file so the directory can list
    /// names without fetching all six. Keep the two in step when renaming.</summary>
    public string Name { get; set; } = "";
}

public class ProfileIndex
{
    public string Default { get; set; } = "layla";
    public List<ProfileEntry> Available { get; set; } = [];

    public bool Knows(string? id) =>
        !string.IsNullOrWhiteSpace(id) &&
        Available.Any(v => string.Equals(v.Id, id, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// The nearest version id to what was typed, for a "did you mean" prompt.
    /// One wrong or missing character counts as near; anything further is not
    /// suggested, because a bad guess is worse than none.
    /// </summary>
    public ProfileEntry? NearestTo(string? id)
    {
        if (string.IsNullOrWhiteSpace(id)) return null;

        var typed = id.Trim().ToLowerInvariant();
        ProfileEntry? best = null;
        var bestDistance = int.MaxValue;

        foreach (var entry in Available)
        {
            var d = Distance(typed, entry.Id.ToLowerInvariant());
            if (d >= bestDistance) continue;
            bestDistance = d;
            best = entry;
        }

        return bestDistance <= 1 ? best : null;
    }

    private static int Distance(string a, string b)
    {
        var prev = new int[b.Length + 1];
        var cur = new int[b.Length + 1];
        for (var j = 0; j <= b.Length; j++) prev[j] = j;

        for (var i = 1; i <= a.Length; i++)
        {
            cur[0] = i;
            for (var j = 1; j <= b.Length; j++)
            {
                var cost = a[i - 1] == b[j - 1] ? 0 : 1;
                cur[j] = Math.Min(Math.Min(cur[j - 1] + 1, prev[j] + 1), prev[j - 1] + cost);
            }

            (prev, cur) = (cur, prev);
        }

        return prev[b.Length];
    }

    /// <summary>
    /// Which version a bare domain lands on. smartgirlalgebra.com is Layla's own
    /// front door, so its root is the default; a version served from its own
    /// domain shouldn't make a child type a path to reach herself.
    /// </summary>
    public Dictionary<string, string> HostMap { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}

/// <summary>
/// Loads a version from wwwroot/content and paints its palette onto the page.
///
/// Everything that distinguishes one version from another — name, colours, fonts,
/// play mode, questions — lives in that JSON. Adding a version is adding a file and
/// listing it in profiles.json, with no rebuild and no deploy from a developer.
/// </summary>
public class ProfileService
{
    private static readonly JsonSerializerOptions Json = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _http;
    private readonly IJSRuntime _js;

    private ProfileIndex? _index;

    public ProfileService(HttpClient http, IJSRuntime js)
    {
        _http = http;
        _js = js;
    }

    public Profile? Current { get; private set; }

    public event Action? Changed;

    public async Task<ProfileIndex> GetIndexAsync()
    {
        if (_index is not null) return _index;

        try
        {
            _index = await _http.GetFromJsonAsync<ProfileIndex>("content/profiles.json", Json) ?? new ProfileIndex();
        }
        catch
        {
            _index = new ProfileIndex
            {
                Default = "layla",
                Available = [new ProfileEntry { Id = "layla", Name = "Smart Girl Algebra" }]
            };
        }

        return _index;
    }

    /// <summary>
    /// Loads a version by route segment. An unknown or missing segment falls back to
    /// the default rather than showing an error — a mistyped URL should still play.
    /// </summary>
    public async Task<Profile?> LoadAsync(string? id)
    {
        var index = await GetIndexAsync();

        // An explicit route segment always wins. Otherwise the domain decides,
        // and only then do we fall back to the default version.
        var wanted = string.IsNullOrWhiteSpace(id)
            ? ProfileForHost(index)
            : id.Trim().ToLowerInvariant();

        if (!index.Knows(wanted)) wanted = index.Default;

        if (Current?.Id == wanted) return Current;

        try
        {
            var profile = await _http.GetFromJsonAsync<Profile>($"content/{wanted}.json", Json);
            if (profile is null) return Current;

            // Tile colours come from the palette, not the content, so a version can be
            // re-skinned without touching a single question.
            var swatches = profile.Theme.Palette;
            for (var i = 0; i < profile.Levels.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(profile.Levels[i].Color)) continue;

                profile.Levels[i].Color = swatches.Count > 0
                    // A multi-colour version gives every level its own swatch.
                    ? swatches[i % swatches.Count]
                    // A two-tone version keeps the accent for the hardest level.
                    : i == profile.Levels.Count - 1
                        ? profile.Theme.Accent
                        : profile.Theme.Primary;
            }

            Current = profile;
            await ApplyThemeAsync(profile.Theme);
            await ConfigureTickleAsync(profile.Tickle);
            Changed?.Invoke();
            return profile;
        }
        catch
        {
            return Current;
        }
    }

    /// <summary>
    /// Maps the domain the app is being served from to a version. The host comes
    /// from the content client's base address, which is this app's own origin.
    /// A "www." prefix is ignored so both spellings land in the same place.
    /// </summary>
    private string ProfileForHost(ProfileIndex index)
    {
        var host = _http.BaseAddress?.Host;
        if (string.IsNullOrWhiteSpace(host)) return index.Default;

        if (index.HostMap.TryGetValue(host, out var byHost)) return byHost;

        if (host.StartsWith("www.", StringComparison.OrdinalIgnoreCase) &&
            index.HostMap.TryGetValue(host[4..], out var byBare))
        {
            return byBare;
        }

        return index.Default;
    }

    private async Task ConfigureTickleAsync(Tickle tickle)
    {
        try
        {
            await _js.InvokeVoidAsync("sgaTickle.configure",
                new
                {
                    lines = tickle.Lines,
                    rate = tickle.Rate,
                    taglines = tickle.Taglines.Select(t => new { text = t.Text, lang = t.Lang, weight = t.Weight })
                });
        }
        catch
        {
            // A version with no jokes still teaches maths.
        }
    }

    private async Task ApplyThemeAsync(Theme theme)
    {
        try
        {
            await _js.InvokeVoidAsync("sgaTheme.apply", theme.ToCssVariables(), theme.GoogleFonts);
        }
        catch
        {
            // Styling is not worth failing a page over; the defaults in sga.css stand.
        }
    }
}
