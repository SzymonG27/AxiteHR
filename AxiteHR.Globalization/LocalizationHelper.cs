using System.Globalization;

namespace AxiteHR.Globalization
{
	public static class LocalizationHelper
	{
		private static LocalizationService _localizationService = GetLocalizationService();

		private static LocalizationService GetLocalizationService()
		{
			if (_localizationService == null)
			{
				_localizationService = new LocalizationService(new CsvResourceProvider("Messages/Resources.csv"));
			}
			return _localizationService;
		}

		public static string GetLocalizedString(string key)
		{
			return _localizationService.GetString(key);
		}

		public static string GetLocalizedString(string key, CultureInfo culture)
		{
			return _localizationService.GetString(key, culture);
		}
	}
}
