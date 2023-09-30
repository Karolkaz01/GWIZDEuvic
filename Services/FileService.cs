namespace Gwizd.Services;

public interface IFileService
{
    Task<FileResult> TakePhotoAsync();
}

public class FileService : IFileService
{
    public async Task<FileResult> TakePhotoAsync()
    {
        FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
        return photo;
    }
}
