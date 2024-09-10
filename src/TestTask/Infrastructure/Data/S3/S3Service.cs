using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;

namespace Infrastructure.Data.S3;

public class S3Service : IS3Service
{
    private readonly IAmazonS3 _s3Client;
    private readonly S3Options _s3Options;

    public S3Service(IAmazonS3 s3Client, IOptions<S3Options> s3Options)
    {
        _s3Client = s3Client;
        _s3Options = s3Options.Value;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        var putRequest = new PutObjectRequest
        {
            BucketName = _s3Options.BucketName,
            Key = fileName,
            InputStream = fileStream,
            ContentType = "image/jpeg"
        };

        var response = await _s3Client.PutObjectAsync(putRequest);
        Console.WriteLine(response.HttpStatusCode);
        return response.HttpStatusCode == System.Net.HttpStatusCode.OK 
                                        ? $"http://localhost:4566/{_s3Options.BucketName}/{fileName}" 
                                        : null;
    }

    public async Task EnsureFileBucketExistsAsync()
    {
        await _s3Client.EnsureBucketExistsAsync(_s3Options.BucketName);
    }
}

public interface IS3Service
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName);
    Task EnsureFileBucketExistsAsync();
}