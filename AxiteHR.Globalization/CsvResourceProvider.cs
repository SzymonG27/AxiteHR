using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace AxiteHR.Globalization
{
	public class CsvResourceProvider
	{
		private readonly Dictionary<string, Dictionary<string, string>> _resources;

		public CsvResourceProvider(string csvFilePath)
		{
			_resources = LoadResources(csvFilePath);
		}

		private static Dictionary<string, Dictionary<string, string>> LoadResources(string csvFilePath)
		{
			var resources = new Dictionary<string, Dictionary<string, string>>();

			using (var reader = new StreamReader(csvFilePath))
			using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
			{
				while (csv.Read())
				{
					var key = csv.GetField<string>(0);
					var plValue = csv.GetField<string>("pl-PL");
					var enValue = csv.GetField<string>("en-US");

					var translations = new Dictionary<string, string>
					{
						{ "pl-PL", plValue },
						{ "en-US", enValue }
					};

					resources[key] = translations;
				}
			}

			return resources;
		}

		public string GetString(string key, CultureInfo culture)
		{
			if (_resources.TryGetValue(key, out var translations) &&
				translations.TryGetValue(culture.Name, out var value))
			{
				return value;
			}

			// Optional: return key if no translation is found
			return key;
		}
	}
}
