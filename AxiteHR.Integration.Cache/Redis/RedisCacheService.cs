using AxiteHR.Integration.GlobalClass.Redis;
using StackExchange.Redis;
using System.Text.Json;

namespace AxiteHR.Integration.Cache.Redis
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

		public async Task PushRightObjectAsync<T>(string key, T value)
		{
			await _database.ListRightPushAsync(key, JsonSerializer.Serialize(value));
		}

		public async Task<List<T?>> GetObjectListAsync<T>(string key)
		{
			var objectList = await _database.ListRangeAsync(key);
			return objectList
				.Select(n => JsonSerializer.Deserialize<T>(n!))
				.Where(n => !EqualityComparer<T?>.Default.Equals(n, default))
				.ToList();
		}

		public async Task RemoveFromObjectListAsync<T>(string key, string id)
			where T : RedisObjectList
		{
			var objectList = await _database.ListRangeAsync(key);

			foreach (var obj in objectList)
			{
				var objectDeserialized = JsonSerializer.Deserialize<T>(obj!);
				if (objectDeserialized?.Id == id)
				{
					await _database.ListRemoveAsync(key, id);
					break;
				}
			}
		}

		private static bool IsPrimitiveOrString(Type type)
		{
			return type.IsPrimitive || type == typeof(string) || type == typeof(decimal);
		}
	}
}
