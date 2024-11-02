using StackExchange.Redis;
using System.Text.Json;

namespace AxiteHR.Services.ApplicationAPI.Services.Cache.Impl
{
	public class RedisCacheService(IConnectionMultiplexer redis) : IRedisCacheService
	{
		private readonly IDatabase _database = redis.GetDatabase();

		public async Task SetObjectAsync<T>(string key, T value, TimeSpan expiry)
		{
			var data = IsPrimitiveOrString(typeof(T))
				? value?.ToString()
				: JsonSerializer.Serialize(value);

			if (data == null)
			{
				return;
			}

			await _database.StringSetAsync(key, data, expiry);
		}

		public async Task<T?> GetObjectAsync<T>(string key)
		{
			var json = await _database.StringGetAsync(key);

			if (!json.HasValue)
				return default;

			return IsPrimitiveOrString(typeof(T))
				? (T)Convert.ChangeType(json.ToString(), typeof(T))
				: JsonSerializer.Deserialize<T>(json.ToString());
		}

		private static bool IsPrimitiveOrString(Type type)
		{
			return type.IsPrimitive || type == typeof(string) || type == typeof(decimal);
		}
	}
}
