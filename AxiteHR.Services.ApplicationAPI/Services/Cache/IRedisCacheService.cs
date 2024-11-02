namespace AxiteHR.Services.ApplicationAPI.Services.Cache
{
	public interface IRedisCacheService
	{
		Task SetObjectAsync<T>(string key, T value, TimeSpan expiry);

		Task<T?> GetObjectAsync<T>(string key);
	}
}
