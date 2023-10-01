﻿using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;

namespace Gwizd.Clients;

public interface IAwsS3Client
{
    Task UploadFileAsync(string fileName, FileResult file);
}

public class AwsS3Client : IAwsS3Client
{
    private const string BucketName = "hack-yeah-animals";
    //private const string FilePath = "C:\\Users\\borowskz\\Desktop\\boar.jfif";
    private readonly IAmazonS3 _s3Client;

    public AwsS3Client(IConfiguration config)
    {
        var appSettings = config.GetRequiredSection("AppSettings").Get<AppSettings>();
        BasicAWSCredentials credentials = new BasicAWSCredentials(appSettings.AwsSettings.Key, appSettings.AwsSettings.Secret);
        _s3Client = new AmazonS3Client(credentials, RegionEndpoint.EUCentral1);
    }

    public async Task UploadFileAsync(string fileName, FileResult file)
    {
        try
        {
            var fileTransferUtility = new TransferUtility(_s3Client);
            await using Stream sourceStream = await file.OpenReadAsync();
            
            await fileTransferUtility.UploadAsync(new TransferUtilityUploadRequest
            {
                BucketName = BucketName,
                Key = $"animals/{fileName}.jpeg",
                InputStream = sourceStream
            });
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
        }

    }
}