namespace AxiteHR.Integration.Storage.Helpers
{
	public static class ContentTypeHelper
	{
		public static string GetContentTypeFromExtension(string fileName)
		{
			var extension = Path.GetExtension(fileName)?.ToLowerInvariant();

			return extension switch
			{
				".pdf" => "application/pdf",
				_ => "application/octet-stream"
			};
		}
	}
}
