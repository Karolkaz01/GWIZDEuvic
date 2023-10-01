using Gwizd.Clients;
using Gwizd.Services;

namespace Gwizd;

public partial class MenuPage : ContentPage
{
    private readonly IFileService _fileService;
    private readonly IAwsS3Client _awsS3Client;
    private readonly IApiClient _apiClient;
    //private readonly MainPage _mainPage;

    public MenuPage(IFileService fileService, IAwsS3Client awsS3Client, IApiClient apiClient/*, MainPage mainPage*/)
	{
        InitializeComponent();
        _awsS3Client = awsS3Client;
        _fileService = fileService;
        _apiClient = apiClient;
        //_mainPage = mainPage;
    }

    private async void OnReportButtonClicked(object sender, EventArgs e)
    {
        var fileResult = await _fileService.TakePhotoAsync();

        LoadingIndicator.IsRunning = true;
        LoadingIndicator.IsVisible = true;

        var reportId = Guid.NewGuid();
        await _awsS3Client.UploadFileAsync(reportId.ToString(), fileResult);

        var prediction = await _apiClient.PredictAnimalDetails(reportId);

        LoadingIndicator.IsRunning = false;
        LoadingIndicator.IsVisible = false;

        await Navigation.PushAsync(new FormPage(reportId, prediction, _awsS3Client, _apiClient/*, _mainPage*/));
    }
}