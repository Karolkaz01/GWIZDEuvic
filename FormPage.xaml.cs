using Gwizd.Clients;
using CommunityToolkit.Maui.Views;

namespace Gwizd;

public partial class FormPage : ContentPage
{
    private readonly IAwsS3Client _awsS3Client;
    private readonly IApiClient _apiClient;
    private readonly PredictionResponse _predictionResponse;
    private readonly Guid _reportId;
    //private readonly MainPage _mainPage;

    private static bool _shouldClosePopup = false;

    public FormPage(Guid reportId, 
        PredictionResponse predictionResponse,
        IAwsS3Client awsS3Client, 
        IApiClient apiClient
        //,
        //MainPage mainPage
        )
    {
        _awsS3Client = awsS3Client;
        _apiClient = apiClient;
        _reportId = reportId;
        _predictionResponse = predictionResponse;
        //_mainPage = mainPage;

        InitializeComponent();
        RenderFile();

        var prediction = predictionResponse?.Body?.Labels?.FirstOrDefault();
        ((Entry)FindByName("SpeciesEntry")).Text = prediction?.Name ?? string.Empty;
        ((Entry)FindByName("RaceEntry")).Text = prediction?.Breed ?? string.Empty;
        ((Entry)FindByName("TypeEntry")).Text = prediction?.Type ?? string.Empty;
    }

    private async void Button_ClickedAsync(object sender, EventArgs e)
    {
        try
        {
            await _apiClient.PostReportedAnimal(_reportId, _predictionResponse);
            var popup = new ThankYouPopup();
            this.ShowPopup(popup);
        }
        catch (Exception ex)
        {
        }
    }

    private async void RenderFile()
    {
        var objectResponse = await _awsS3Client.GetFileAsync(_reportId.ToString());
        AnimalPhoto.Source = ImageSource.FromStream(() => objectResponse.ResponseStream);

    }
}