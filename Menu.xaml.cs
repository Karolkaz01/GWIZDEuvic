namespace Gwizd;

public partial class MenuPage : ContentPage
{
    public MenuPage()
	{
        InitializeComponent();
    }

    private void OnReportButtonClicked(object sender, EventArgs e)
    {
    }


 //   private async void OnReportButtonClicked(object sender, EventArgs e)
	//{
 //       var fileService = new FileService();
 //       var fileResult = await fileService.TakePhotoAsync();

 //       LoadingIndicator.IsRunning = true;
 //       LoadingIndicator.IsVisible = true;

 //       var reportId = Guid.NewGuid();
 //       var awsClient = new AwsS3Client();
 //       await awsClient.UploadFileAsync(reportId.ToString(), fileResult);


 //       //call Bartek's endpoint for prediction


 //       LoadingIndicator.IsRunning = false;
 //       LoadingIndicator.IsVisible = false;
 //   }
}