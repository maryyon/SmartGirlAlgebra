using System.Net.Http.Json;
using SmartGirlAlgebra.Models;

namespace SmartGirlAlgebra.Services;

public class ProgressService
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;

    public ProgressService(HttpClient httpClient, AuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    public async Task<UserStats?> GetProgressAsync()
    {
        try
        {
            if (!await _authService.IsAuthenticatedAsync())
                return null;

            var response = await _httpClient.GetAsync("api/progress");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserStats>();
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> UpdateProgressAsync(UserStats progress)
    {
        try
        {
            if (!await _authService.IsAuthenticatedAsync())
                return false;

            var response = await _httpClient.PostAsJsonAsync("api/progress/update", progress);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> IncrementProblemAsync(bool isCorrect, int scoreEarned = 0)
    {
        var progress = await GetProgressAsync();
        if (progress == null)
        {
            progress = new UserStats();
        }

        progress.TotalProblemsAttempted++;

        if (isCorrect)
        {
            progress.TotalCorrect++;
            progress.CurrentStreak++;
            progress.TotalScore += scoreEarned;

            if (progress.CurrentStreak > progress.BestStreak)
            {
                progress.BestStreak = progress.CurrentStreak;
            }
        }
        else
        {
            progress.CurrentStreak = 0;
        }

        return await UpdateProgressAsync(progress);
    }
}

