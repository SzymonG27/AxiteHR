using Amazon.S3;
using Amazon.S3.Model;
using AxiteHR.Integration.Storage.Abstractions;
using AxiteHR.Integration.Storage.Configuration;
using Microsoft.Extensions.Options;

namespace AxiteHR.Integration.Storage.Providers.Minio
{
    public class MinioService : IObjectStorageService
	{
		private readonly IAmazonS3 _s3Client;
		private readonly MinioConfig _config;

		public MinioService(IOptions<MinioConfig> options)
		{
			_config = options.Value;

			_s3Client = new AmazonS3Client(
				_config.AccessKey,
				_config.SecretKey,
				new AmazonS3Config
				{
					ServiceURL = _config.Endpoint,
					ForcePathStyle = true
				}
			);
		}

		public async Task<string> UploadAsync(Stream stream, string fileName, string contentType, string bucket, CancellationToken cancellationToken = default)
		{
			var putRequest = new PutObjectRequest
			{
				BucketName = bucket,
				Key = fileName,
				InputStream = stream,
				ContentType = contentType,
				AutoCloseStream = true
			};

			await _s3Client.PutObjectAsync(putRequest, cancellationToken);

			return $"{_config.Endpoint}/{bucket}/{fileName}";
		}

		public async Task<Stream> DownloadAsync(string fileName, string bucket, CancellationToken cancellationToken = default)
		{
			var getRequest = new GetObjectRequest
			{
				BucketName = bucket,
				Key = fileName
			};

			using var response = await _s3Client.GetObjectAsync(getRequest, cancellationToken);

			var memoryStream = new MemoryStream();
			await response.ResponseStream.CopyToAsync(memoryStream, cancellationToken);
			memoryStream.Position = 0;

			return memoryStream;
		}

		public async Task DeleteAsync(string fileName, string bucket, CancellationToken cancellationToken = default)
		{
			var deleteRequest = new DeleteObjectRequest
			{
				BucketName = bucket,
				Key = fileName
			};

			await _s3Client.DeleteObjectAsync(deleteRequest, cancellationToken);
		}
	}
}
