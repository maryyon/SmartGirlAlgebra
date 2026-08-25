using System.Security.Cryptography;

namespace SmartGirlAlgebra.API.Services;

/// <summary>
/// Generates sync codes like SGA-7K4M2P or JD-4M2P7K.
///
/// The prefix names the version the code belongs to. That is what keeps progress
/// separate between versions without a database column: a code issued for one
/// version simply will not be found by another, because the prefix differs.
///
/// The alphabet leaves out characters that are easy to confuse when a child reads
/// a code off one screen and types it on another (no O/0, no I/1/L).
/// </summary>
public static class SyncCodeGenerator
{
    private const string Alphabet = "ABCDEFGHJKMNPQRSTUVWXYZ23456789";
    private const int BodyLength = 6;

    private const int MinPrefix = 2;
    private const int MaxPrefix = 4;

    public const string DefaultPrefix = "SGA";

    /// <summary>Prefixes come from client-supplied content, so they are constrained.</summary>
    public static string SanitisePrefix(string? prefix)
    {
        if (string.IsNullOrWhiteSpace(prefix)) return DefaultPrefix;

        var cleaned = new string(prefix
            .ToUpperInvariant()
            .Where(char.IsAsciiLetter)
            .ToArray());

        if (cleaned.Length < MinPrefix || cleaned.Length > MaxPrefix) return DefaultPrefix;

        return cleaned;
    }

    public static string Generate(string? prefix)
    {
        var chars = new char[BodyLength];
        for (var i = 0; i < BodyLength; i++)
        {
            chars[i] = Alphabet[RandomNumberGenerator.GetInt32(Alphabet.Length)];
        }

        return $"{SanitisePrefix(prefix)}-{new string(chars)}";
    }

    /// <summary>
    /// Accepts what a child actually types — lower case, missing dash, stray spaces —
    /// and returns the canonical form, or null if it cannot be one of our codes.
    /// The prefix is preserved, since it identifies the version.
    /// </summary>
    public static string? Normalize(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;

        var cleaned = new string(input
            .ToUpperInvariant()
            .Where(char.IsLetterOrDigit)
            .ToArray());

        // A code is a letters-only prefix followed by exactly six body characters.
        if (cleaned.Length < MinPrefix + BodyLength) return null;
        if (cleaned.Length > MaxPrefix + BodyLength) return null;

        var body = cleaned[^BodyLength..];
        var prefix = cleaned[..^BodyLength];

        if (prefix.Length < MinPrefix || !prefix.All(char.IsAsciiLetter)) return null;
        if (body.Any(c => !Alphabet.Contains(c))) return null;

        return $"{prefix}-{body}";
    }
}
