using Microsoft.Extensions.Localization;

namespace AxiteHR.Services.DocumentAPI.Extensions
{
	public static class StringLocalizerExtensions
	{
		public static Dictionary<string, string> GetTranslations(
		this IStringLocalizer localizer,
		IEnumerable<string> keys)
		{
			return keys.ToDictionary(key => key, key => localizer[key].Value);
		}
	}
}
