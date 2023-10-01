using System.Net.Http.Json;

using Newtonsoft.Json;

namespace Gwizd.Clients;

public interface IApiClient
{
    Task<PredictionResponse> PredictAnimalDetails(Guid reportId);
    Task PostReportedAnimal(Guid reportId, PredictionResponse predictionResponse);
}

public class ApiClient : IApiClient
{
    private static HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("https://mezvs42ny3.execute-api.eu-central-1.amazonaws.com"),
    };

    public async Task<PredictionResponse> PredictAnimalDetails(Guid reportId)
    {
        using HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
            "default/predict",
            new PredictionRequest { Id = reportId.ToString() });

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();

        PredictionResponse prediction = JsonConvert.DeserializeObject<PredictionResponse>(jsonResponse);
        return prediction;
    }

    public async Task PostReportedAnimal(Guid reportId, PredictionResponse predictionResponse)
    {
        Location location = await Geolocation.Default.GetLastKnownLocationAsync();
        var prediction = predictionResponse.Body.Labels.First();
        var response = await _httpClient.PostAsJsonAsync("default/events",
            new AddEventDTO
            {
                id = reportId.ToString(),
                animal_type = prediction?.Type ?? string.Empty,
                label = prediction?.Name ?? string.Empty,
                breed = prediction?.Breed ?? string.Empty,
                event_type = "reported",
                lat = location?.Latitude ?? 0.0,
                lng = location?.Longitude ?? 0.0
            });

        response.EnsureSuccessStatusCode();
    }
}