using Microsoft.JSInterop;
using SmartGirlAlgebra.Models;

namespace SmartGirlAlgebra.Services;

/// <summary>
/// Remembers which lessons a child has actually mastered, per version.
///
/// Mastery is earned on FIRST attempts only. Guessing through the options until
/// something turns green teaches nothing, so a lesson only unlocks the next one
/// when she got enough right the first time she saw each question.
///
/// Stored per version, like everything else, so a younger child's progress never
/// mixes with an older one's.
/// </summary>
public class LessonProgress
{
    private readonly IJSRuntime _js;

    private string _profileId = "";
    private HashSet<int> _mastered = [];

    public LessonProgress(IJSRuntime js) => _js = js;

    public event Action? Changed;

    private string Key => $"sgaMastered:{_profileId}";

    public async Task UseProfileAsync(string profileId)
    {
        if (_profileId == profileId) return;
        _profileId = profileId;
        _mastered = await LoadAsync();
        Changed?.Invoke();
    }

    public bool IsMastered(int lessonId) => _mastered.Contains(lessonId);

    /// <summary>
    /// A lesson is available if it's the first, already mastered, or the one
    /// straight after the last mastered lesson. She can revisit anything she has
    /// finished, but cannot skip ahead of what she has shown she understands.
    /// </summary>
    public bool IsUnlocked(IReadOnlyList<Lesson> lessons, int lessonId)
    {
        if (lessons.Count == 0) return false;
        if (lessons[0].Id == lessonId) return true;
        if (_mastered.Contains(lessonId)) return true;

        var index = -1;
        for (var i = 0; i < lessons.Count; i++)
        {
            if (lessons[i].Id != lessonId) continue;
            index = i;
            break;
        }

        return index > 0 && _mastered.Contains(lessons[index - 1].Id);
    }

    public async Task MarkMasteredAsync(int lessonId)
    {
        if (!_mastered.Add(lessonId)) return;
        await SaveAsync();
        Changed?.Invoke();
    }

    private async Task<HashSet<int>> LoadAsync()
    {
        try
        {
            var raw = await _js.InvokeAsync<string?>("localStorage.getItem", Key);
            if (string.IsNullOrWhiteSpace(raw)) return [];

            return raw.Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(s => int.TryParse(s, out var n) ? n : -1)
                      .Where(n => n >= 0)
                      .ToHashSet();
        }
        catch
        {
            return [];
        }
    }

    private async Task SaveAsync()
    {
        try
        {
            await _js.InvokeVoidAsync("localStorage.setItem", Key, string.Join(",", _mastered));
        }
        catch { /* private browsing — progress lasts the session */ }
    }
}
