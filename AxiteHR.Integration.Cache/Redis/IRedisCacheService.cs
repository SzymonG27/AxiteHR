using AxiteHR.Integration.GlobalClass.Redis;

namespace AxiteHR.Integration.Cache.Redis
{
	public interface IRedisCacheService
	{
		Task SetObjectAsync<T>(string key, T value, TimeSpan expiry);

		Task<T?> GetObjectAsync<T>(string key);

		Task PushRightObjectAsync<T>(string key, T value) where T : RedisObjectList;

		Task<List<T?>> GetObjectListAsync<T>(string key) where T : RedisObjectList;

		Task RemoveFromObjectListAsync<T>(string key, string id) where T : RedisObjectList;
	}
}
