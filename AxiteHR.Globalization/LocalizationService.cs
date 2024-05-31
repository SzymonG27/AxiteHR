using System.Globalization;

namespace AxiteHR.Globalization
{
	public class LocalizationService
	{
		private readonly CsvResourceProvider _csvResourceProvider;

		public LocalizationService(CsvResourceProvider csvResourceProvider)
		{
			_csvResourceProvider = csvResourceProvider;
		}

		public string GetString(string key)
		{
			return _csvResourceProvider.GetString(key, CultureInfo.CurrentCulture);
		}

		public string GetString(string key, CultureInfo culture)
		{
			return _csvResourceProvider.GetString(key, culture);
		}
	}
}
