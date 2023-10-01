using System.Net.Http.Json;

using Newtonsoft.Json;

namespace Gwizd.Clients;

public interface IPredictionApiClient
{
    Task<PredictionResponse> PredictAnimalDetails(Guid reportId);
}

public class PredictionApiClient : IPredictionApiClient
{
    private static HttpClient httpClient = new()
    {
        BaseAddress = new Uri("https://mezvs42ny3.execute-api.eu-central-1.amazonaws.com"),
    };

    public async Task<PredictionResponse> PredictAnimalDetails(Guid reportId)
    {
        using HttpResponseMessage response = await httpClient.PostAsJsonAsync(
            "default/predict",
            new PredictionRequest {Id = reportId.ToString()});

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();

        PredictionResponse prediction = JsonConvert.DeserializeObject<PredictionResponse>(jsonResponse);
        return prediction;
    }
}