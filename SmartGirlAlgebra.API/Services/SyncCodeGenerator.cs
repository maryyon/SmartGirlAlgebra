using System.Security.Cryptography;

namespace SmartGirlAlgebra.API.Services;

/// <summary>
/// Generates sync codes like SGA-7K4M2P. The alphabet leaves out characters that
/// are easy to confuse when a kid reads a code off one screen and types it on another
/// (no O/0, no I/1/L).
/// </summary>
public static class SyncCodeGenerator
{
    private const string Alphabet = "ABCDEFGHJKMNPQRSTUVWXYZ23456789";
    private const int Length = 6;

    public static string Generate()
    {
        var chars = new char[Length];
        for (var i = 0; i < Length; i++)
        {
            chars[i] = Alphabet[RandomNumberGenerator.GetInt32(Alphabet.Length)];
        }

        return $"SGA-{new string(chars)}";
    }

    /// <summary>
    /// Accepts what a kid actually types — lower case, missing dash, stray spaces —
    /// and returns the canonical form, or null if it can't be one of our codes.
    /// </summary>
    public static string? Normalize(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;

        var cleaned = new string(input
            .ToUpperInvariant()
            .Where(char.IsLetterOrDigit)
            .ToArray());

        if (cleaned.StartsWith("SGA")) cleaned = cleaned[3..];
        if (cleaned.Length != Length) return null;
        if (cleaned.Any(c => !Alphabet.Contains(c))) return null;

        return $"SGA-{cleaned}";
    }
}
