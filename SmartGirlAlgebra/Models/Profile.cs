using System.Text.Json.Serialization;

namespace SmartGirlAlgebra.Models;

/// <summary>How a child answers. Grade level decides this, not preference.</summary>
public enum PlayMode
{
    /// <summary>Show your working one line at a time, checked as you go.</summary>
    Steps,

    /// <summary>Tap the right answer. No typing, no notation, no equals sign.</summary>
    Choice
}

/// <summary>
/// One version of the app: its own name, palette, content and sync-code prefix,
/// served from its own route. Everything is data — adding a version is adding a
/// JSON file under wwwroot/content, with no rebuild.
/// </summary>
public class Profile
{
    /// <summary>Route segment and file name, e.g. "jd" serves /jd. Lower case.</summary>
    public string Id { get; set; } = "";

    public string Name { get; set; } = "";
    public string Tagline { get; set; } = "";

    /// <summary>Shown on the how-to-play card.</summary>
    public string HowToPlay { get; set; } = "";

    /// <summary>
    /// Prefix for this version's sync codes, e.g. "JD" gives JD-4M2P7K. This is
    /// what keeps progress separate between versions without a database column:
    /// a code from one version simply will not resolve in another.
    /// </summary>
    public string CodePrefix { get; set; } = "SGA";

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PlayMode Mode { get; set; } = PlayMode.Steps;

    /// <summary>Image shown at the top of the home screen, relative to wwwroot.</summary>
    public string Crest { get; set; } = "";

    public string LevelPickerTitle { get; set; } = "Choose your level";
    public string LevelPickerSubtitle { get; set; } = "";
    public string FooterNote { get; set; } = "";

    public Theme Theme { get; set; } = new();
    public List<Level> Levels { get; set; } = [];

    public Level? LevelById(int id) => Levels.FirstOrDefault(l => l.Id == id);
}

/// <summary>
/// A version's palette. Every colour in the stylesheet derives from these, so a
/// theme is applied by setting custom properties on :root — nothing else changes.
/// </summary>
public class Theme
{
    public string Primary { get; set; } = "#16284b";
    public string PrimaryHi { get; set; } = "#233a68";
    public string PrimaryFg { get; set; } = "#ffffff";
    public string Accent { get; set; } = "#ce2233";
    public string AccentFg { get; set; } = "#ffffff";

    public string Background { get; set; } = "#f1f3f7";
    public string Foreground { get; set; } = "#16284b";
    public string Card { get; set; } = "#ffffff";
    public string Secondary { get; set; } = "#d9dee7";
    public string SecondaryFg { get; set; } = "#16284b";
    public string Muted { get; set; } = "#e7ebf1";
    public string MutedFg { get; set; } = "#5c6880";
    public string Border { get; set; } = "#c3cbd8";

    /// <summary>Kept legible as feedback regardless of palette.</summary>
    public string Correct { get; set; } = "#12885f";
    public string CorrectBg { get; set; } = "#e4f6ee";
    public string Wrong { get; set; } = "#ce2233";
    public string WrongBg { get; set; } = "#fbe9eb";

    public string FontDisplay { get; set; } = "\"Baloo 2\", ui-rounded, \"Segoe UI\", system-ui, sans-serif";
    public string FontSans { get; set; } = "\"Nunito\", ui-sans-serif, system-ui, -apple-system, sans-serif";

    /// <summary>
    /// Google Fonts families this theme needs, e.g. ["Baloo 2:wght@600;700;800"].
    /// Loaded on demand so a version only pays for the fonts it uses.
    /// </summary>
    public List<string> GoogleFonts { get; set; } = [];

    /// <summary>Maps to the CSS custom properties the stylesheet reads.</summary>
    public Dictionary<string, string> ToCssVariables() => new()
    {
        ["--sga-primary"] = Primary,
        ["--sga-primary-hi"] = PrimaryHi,
        ["--sga-primary-fg"] = PrimaryFg,
        ["--sga-accent"] = Accent,
        ["--sga-accent-fg"] = AccentFg,
        ["--sga-background"] = Background,
        ["--sga-foreground"] = Foreground,
        ["--sga-card"] = Card,
        ["--sga-secondary"] = Secondary,
        ["--sga-secondary-fg"] = SecondaryFg,
        ["--sga-muted"] = Muted,
        ["--sga-muted-fg"] = MutedFg,
        ["--sga-border"] = Border,
        ["--sga-correct"] = Correct,
        ["--sga-correct-bg"] = CorrectBg,
        ["--sga-wrong"] = Wrong,
        ["--sga-wrong-bg"] = WrongBg,
        ["--sga-font-display"] = FontDisplay,
        ["--sga-font-sans"] = FontSans
    };
}
