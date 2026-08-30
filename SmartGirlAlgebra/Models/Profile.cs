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

    /// <summary>
    /// Names of the drifting background icons for this version. They are drawn in
    /// the version's own colours, so they must suit the theme rather than the
    /// subject alone. See ThemeIcons for the available names.
    /// </summary>
    public string[] Icons { get; set; } = [];

    public string LevelPickerTitle { get; set; } = "Choose your level";
    public string LevelPickerSubtitle { get; set; } = "";
    public string FooterNote { get; set; } = "";

    public Theme Theme { get; set; } = new();

    /// <summary>This version's own voice. Layla's cheer language does not belong
    /// in a basketball version, so these are per-version rather than shared.</summary>
    public Encouragement Encouragement { get; set; } = new();

    /// <summary>Verses shown at the milestones, pitched at this version's age.</summary>
    public List<Verse> Verses { get; set; } = [];

    /// <summary>
    /// The teaching track: ideas taught, then applied, then quietly checked.
    /// Empty means this version is practice-only and no Learn button appears.
    /// </summary>
    public List<Lesson> Lessons { get; set; } = [];

    /// <summary>
    /// Whether instructions get a "read it to me" button that says them aloud and
    /// lights each word as it goes. On for versions whose readers can't yet.
    /// </summary>
    public bool ReadAloud { get; set; }

    /// <summary>
    /// Whether words above the version's reading level are underlined and can be
    /// tapped to hear them said slowly. For readers who can manage most of a
    /// sentence but get stopped by one word in it.
    /// </summary>
    public bool TapToRead { get; set; }

    /// <summary>
    /// Whether the read-along leaves the last word for the child to read out
    /// loud, and listens for him saying it. Needs <see cref="ReadAloud"/> on.
    /// </summary>
    public bool ListenBack { get; set; }

    /// <summary>
    /// The silly lines this version says, once in a great while, when a button
    /// is tapped. Age-matched per version; the tagline is the same for everyone
    /// because it is hers.
    /// </summary>
    public Tickle Tickle { get; set; } = new();

    /// <summary>
    /// Use the generated semester instead of the stored lessons. Problems are
    /// built fresh with new numbers every time and every step is typed, so there
    /// is nothing to guess at and nothing to memorise.
    /// </summary>
    public bool Semester { get; set; }

    public List<Level> Levels { get; set; } = [];

    public Level? LevelById(int id) => Levels.FirstOrDefault(l => l.Id == id);
}

/// <summary>
/// What the app says at each moment that matters. Kept out of code because the
/// right words differ by age and by subject: a six-year-old tapping an answer and
/// a ninth-grader showing her working need different sentences, and neither wants
/// the other's.
/// </summary>
public class Encouragement
{
    /// <summary>After a correct line or a correct tap.</summary>
    public string[] Correct { get; set; } = [];

    /// <summary>Prefixed to the diagnostic when something is wrong. The diagnostic
    /// itself stays shared, because what went wrong is about the maths, not the theme.</summary>
    public string[] Wrong { get; set; } = [];

    /// <summary>On finishing a level.</summary>
    public string[] Solved { get; set; } = [];
}

public class Verse
{
    public string Text { get; set; } = "";
    public string Ref { get; set; } = "";
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

    /// <summary>
    /// Extra brand colours, for versions that shouldn't be two-tone. Level tiles
    /// and the drifting icons cycle through these, so an art version can actually
    /// look like a paint box instead of one hue repeated. Empty means two-tone:
    /// primary and accent only.
    ///
    /// These sit behind white text, so keep them deep enough to carry it.
    /// </summary>
    public List<string> Palette { get; set; } = [];

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

/// <summary>
/// A giggle and then the signature. Kept as a pair on purpose: the lines can be
/// swapped or added to forever without touching the sentence that ends them.
/// </summary>
public class Tickle
{
    public string[] Lines { get; set; } = [];

    /// <summary>
    /// Hers, in three languages. Changed here, it changes everywhere.
    /// English carries most of the time; the other two turn up now and then, and
    /// only on a device that actually has a voice for them.
    /// </summary>
    public List<Tagline> Taglines { get; set; } =
    [
        new() { Text = "Who loves you more than a roadtrip in an RV, sweetheart?", Lang = "en-US", Weight = 7 },
        new() { Text = "¿Quién te quiere más que un viaje en la casa rodante, mi amor?", Lang = "es-US", Weight = 1.5 },
        new() { Text = "Who love yuh more dan a road trip inna di RV, mi sweetheart?", Lang = "en-JM", Weight = 1.5, Strict = true }
    ];

    public double Rate { get; set; } = 1.0;
}

/// <summary>One language's version of the signature.</summary>
public class Tagline
{
    public string Text { get; set; } = "";
    public string Lang { get; set; } = "en-US";

    /// <summary>Relative to the others. English is deliberately the common one.</summary>
    public double Weight { get; set; } = 1;

    /// <summary>
    /// The region has to match, not just the language. Patwa needs a genuinely
    /// Jamaican voice; read by an American one it is the thing that offends.
    /// Until such a voice exists on the device, the line simply doesn't play.
    /// </summary>
    public bool Strict { get; set; }
}
