using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;
using SmartGirlAlgebra.Models;

namespace SmartGirlAlgebra.Services;

public class ProfileIndex
{
    public string Default { get; set; } = "layla";
    public List<string> Available { get; set; } = [];
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
            _index = new ProfileIndex { Default = "layla", Available = ["layla"] };
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

        var wanted = string.IsNullOrWhiteSpace(id) ? index.Default : id.Trim().ToLowerInvariant();
        if (!index.Available.Contains(wanted)) wanted = index.Default;

        if (Current?.Id == wanted) return Current;

        try
        {
            var profile = await _http.GetFromJsonAsync<Profile>($"content/{wanted}.json", Json);
            if (profile is null) return Current;

            // Tile colours come from the palette, not the content, so a version can be
            // re-skinned without touching a single question. The last level is the
            // hardest, so it takes the accent.
            for (var i = 0; i < profile.Levels.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(profile.Levels[i].Color)) continue;
                profile.Levels[i].Color = i == profile.Levels.Count - 1
                    ? profile.Theme.Accent
                    : profile.Theme.Primary;
            }

            Current = profile;
            await ApplyThemeAsync(profile.Theme);
            Changed?.Invoke();
            return profile;
        }
        catch
        {
            return Current;
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
