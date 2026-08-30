using Microsoft.JSInterop;
using SmartGirlAlgebra.Models;

namespace SmartGirlAlgebra.Services;

/// <summary>
/// How far through the semester she is.
///
/// Progress is counted in PROBLEMS WORKED, not lessons opened. A skill counts as
/// learned once she has worked its problems all the way through, every step
/// typed, the required number of times — and because the numbers are different
/// every time, that cannot be reached by remembering anything.
///
/// Stored per version, like everything else.
/// </summary>
public class SkillProgress
{
    private readonly IJSRuntime _js;

    private string _profileId = "";
    private Dictionary<int, int> _worked = [];

    public SkillProgress(IJSRuntime js) => _js = js;

    public event Action? Changed;

    private string Key => $"sgaSkill:{_profileId}";

    public async Task UseProfileAsync(string profileId)
    {
        if (_profileId == profileId) return;

        _profileId = profileId;
        _worked = await LoadAsync();
        Changed?.Invoke();
    }

    /// <summary>How many problems of this skill she has worked to the end.</summary>
    public int Worked(int skillId) => _worked.GetValueOrDefault(skillId, 0);

    public bool IsPassed(SkillDef skill) => Worked(skill.Id) >= skill.ToPass;

    /// <summary>
    /// A skill is open if it is the first, already passed, or the one straight
    /// after the last one she passed. She can go back over anything she has
    /// finished, but she cannot skip ahead of what she has shown she can do.
    /// </summary>
    public bool IsUnlocked(int skillId)
    {
        var all = Curriculum.Skills;
        if (all.Length == 0) return false;
        if (all[0].Id == skillId) return true;

        var index = Array.FindIndex(all, s => s.Id == skillId);
        if (index < 0) return false;
        if (IsPassed(all[index])) return true;

        return IsPassed(all[index - 1]);
    }

    /// <summary>Where she is up to — the first skill she has not passed.</summary>
    public SkillDef? NextUp()
    {
        foreach (var s in Curriculum.Skills)
        {
            if (!IsPassed(s)) return s;
        }

        return null;
    }

    public int PassedCount() => Curriculum.Skills.Count(IsPassed);

    public async Task RecordWorkedAsync(int skillId)
    {
        _worked[skillId] = Worked(skillId) + 1;
        await SaveAsync();
        Changed?.Invoke();
    }

    private async Task<Dictionary<int, int>> LoadAsync()
    {
        try
        {
            var raw = await _js.InvokeAsync<string?>("localStorage.getItem", Key);
            if (string.IsNullOrWhiteSpace(raw)) return [];

            var map = new Dictionary<int, int>();

            foreach (var pair in raw.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                var bits = pair.Split(':');
                if (bits.Length != 2) continue;
                if (!int.TryParse(bits[0], out var id)) continue;
                if (!int.TryParse(bits[1], out var n)) continue;
                map[id] = n;
            }

            return map;
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
            var raw = string.Join(",", _worked.Select(kv => $"{kv.Key}:{kv.Value}"));
            await _js.InvokeVoidAsync("localStorage.setItem", Key, raw);
        }
        catch { /* private browsing — progress lasts the session */ }
    }
}
