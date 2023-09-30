using Amazon.S3;
using Gwizd.Services;

namespace Gwizd.Extensions;

public static class AppBuilderExtensions
{
    public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<IFileService, FileService>();
        mauiAppBuilder.Services.AddSingleton<ILocationService, LocationService>();

        mauiAppBuilder.Services.AddAWSService<IAmazonS3>();

        return mauiAppBuilder;
    }
}
