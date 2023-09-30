using Gwizd.Clients;
using Gwizd.Services;

namespace Gwizd;

public partial class MainPage : ContentPage
{
    public MainPage()
	{
        InitializeComponent();
    }

	private async void OnReportButtonClicked(object sender, EventArgs e)
	{
        //SemanticScreenReader.Announce(CounterBtn.Text);
        var fileService = new FileService();
        var fileResult = await fileService.TakePhotoAsync();

        var reportId = Guid.NewGuid();
        var awsClient = new AwsS3Client();
        await awsClient.UploadFileAsync(reportId.ToString(), fileResult);
    }
}