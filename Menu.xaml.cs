using Gwizd.Clients;
using Gwizd.Services;

namespace Gwizd;

public partial class MenuPage : ContentPage
{
    private readonly IFileService _fileService;
    private readonly IAwsS3Client _awsS3Client;
    private readonly IPredictionApiClient _predictionApiClient;

    public MenuPage(IFileService fileService, IAwsS3Client awsS3Client, IPredictionApiClient predictionApiClient)
	{
        InitializeComponent();
        _awsS3Client = awsS3Client;
        _fileService = fileService;
        _predictionApiClient = predictionApiClient;
    }

    private async void OnReportButtonClicked(object sender, EventArgs e)
    {
        var fileResult = await _fileService.TakePhotoAsync();

        LoadingIndicator.IsRunning = true;
        LoadingIndicator.IsVisible = true;

        var reportId = Guid.NewGuid();
        await _awsS3Client.UploadFileAsync(reportId.ToString(), fileResult);

        var prediction = await _predictionApiClient.PredictAnimalDetails(reportId);

        LoadingIndicator.IsRunning = false;
        LoadingIndicator.IsVisible = false;
    }
}