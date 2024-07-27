using System.Text.Json;

namespace AxiteHr.Services.CompanyAPI.Helpers
{
	public static class JsonOptionsHelper
	{
		public static JsonSerializerOptions DefaultJsonSerializerOptions => new()
		{
			PropertyNameCaseInsensitive = true
		};
	}
}
