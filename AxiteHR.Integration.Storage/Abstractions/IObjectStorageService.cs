namespace AxiteHR.Integration.Storage.Abstractions
{
	public interface IObjectStorageService
	{
		Task<string> UploadAsync(Stream stream, string fileName, string contentType, string bucket, CancellationToken cancellationToken = default);
		Task<Stream> DownloadAsync(string fileName, string bucket, CancellationToken cancellationToken = default);
		Task DeleteAsync(string fileName, string bucket, CancellationToken cancellationToken = default);
	}
}
