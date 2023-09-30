namespace Gwizd;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private void OnReportButtonClicked(object sender, EventArgs e)
	{
        //SemanticScreenReader.Announce(CounterBtn.Text);
        TakePhoto();
    }

    public static async void TakePhoto()
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {

            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo != null)
            {
                //TODO: upload to AWS S3, send to Bartek's endpoint 

                // save the file into local storage
                //string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                //using Stream sourceStream = await photo.OpenReadAsync();
                //using FileStream localFileStream = File.OpenWrite(localFilePath);

                //await sourceStream.CopyToAsync(localFileStream);
            }
        }
    }
}

