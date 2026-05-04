using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace WebsiteTour.Services;

public interface IRecommendationClient
{
    Task<IReadOnlyList<int>> GetRecommendedTourIdsAsync(
        int userId,
        int topN,
        CancellationToken cancellationToken = default
    );
}

public class RecommendationClient : IRecommendationClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<RecommendationClient> _logger;

    public RecommendationClient(HttpClient httpClient, ILogger<RecommendationClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IReadOnlyList<int>> GetRecommendedTourIdsAsync(
        int userId,
        int topN,
        CancellationToken cancellationToken = default
    )
    {
        const int maxAttempts = 3;
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                using var response = await _httpClient.GetAsync(
                    $"recommend/{userId}?top_n={topN}",
                    cancellationToken
                );
                response.EnsureSuccessStatusCode();

                var payload = await response.Content.ReadFromJsonAsync<RecommendationResponse>(
                    cancellationToken: cancellationToken
                );
                return payload?.RecommendedTourIds ?? [];
            }
            catch (Exception ex) when (attempt < maxAttempts)
            {
                _logger.LogWarning(
                    ex,
                    "Recommendation API failed (attempt {Attempt}/{MaxAttempts}) for user {UserId}",
                    attempt,
                    maxAttempts,
                    userId
                );
                await Task.Delay(TimeSpan.FromMilliseconds(200 * attempt), cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Recommendation API failed after {MaxAttempts} attempts for user {UserId}",
                    maxAttempts,
                    userId
                );
                return [];
            }
        }

        return [];
    }

    private sealed class RecommendationResponse
    {
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        [JsonPropertyName("recommended_tour_ids")]
        public List<int> RecommendedTourIds { get; set; } = [];
    }
}
