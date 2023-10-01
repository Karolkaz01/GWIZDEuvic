using System.Reflection;
using Amazon.S3;

using Gwizd.Clients;
using Gwizd.Services;
using Microsoft.Extensions.Configuration;

namespace Gwizd.Extensions;

public static class AppBuilderExtensions
{
    public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<IFileService, FileService>();
        mauiAppBuilder.Services.AddSingleton<IPredictionApiClient, PredictionApiClient>();
        mauiAppBuilder.Services.AddSingleton<ILocationService, LocationService>();
        mauiAppBuilder.Services.AddSingleton<IAwsS3Client, AwsS3Client>();

        mauiAppBuilder.Services.AddAWSService<IAmazonS3>();

        mauiAppBuilder.Services.AddTransient<AppShell>();
        mauiAppBuilder.Services.AddTransient<MainPage>();
        mauiAppBuilder.Services.AddTransient<MenuPage>();

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterAppConfiguration(this MauiAppBuilder mauiAppBuilder)
    {
        var a = Assembly.GetExecutingAssembly();
        using var stream = a.GetManifestResourceStream("Gwizd.appsettings.json");

        var config = new ConfigurationBuilder()
                     .AddJsonStream(stream)
                     .Build();

        mauiAppBuilder.Configuration.AddConfiguration(config);

        return mauiAppBuilder;
    }
}
