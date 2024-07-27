using System.Text.Json;

namespace AxiteHR.Services.EmailAPI.Helpers
{
	public static class JsonOptionsHelper
	{
		public static JsonSerializerOptions DefaultJsonSerializerOptions => new()
		{
			PropertyNameCaseInsensitive = true
		};
	}
}
