using AxiteHR.Integration.Storage.Abstractions;
using AxiteHR.Integration.Storage.Providers.Minio;
using Microsoft.Extensions.DependencyInjection;

namespace AxiteHR.Integration.Storage.Factory
{
	public class StorageFactory : IStorageFactory
	{
		private readonly IServiceProvider _provider;

		public StorageFactory(IServiceProvider provider)
		{
			_provider = provider;
		}

		public IObjectStorageService Get(ObjectStorageType type) => type switch
		{
			ObjectStorageType.Minio => _provider.GetRequiredService<MinioService>(),
			_ => throw new NotSupportedException($"Storage type {type} is not supported.")
		};
	}
}
