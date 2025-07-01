using System.Text.Json;

namespace AxiteHR.Services.DocumentAPI.Helpers
{
	public static class JsonOptionsHelper
	{
		public static JsonSerializerOptions DefaultJsonSerializerOptions => new()
		{
			PropertyNameCaseInsensitive = true
		};
	}
}
