namespace AxiteHR.Services.DocumentAPI.Models.Invoice.Dto
{
	public class LogoDto
	{
		private static readonly Dictionary<string, string> _magicMimePrefixes = new()
		{
			{ "iVBORw0KGgo", "image/png" },
			{ "/9j/", "image/jpeg" },
			{ "UklGR", "image/webp" },
			{ "R0lGOD", "image/gif" },
			{ "Qk", "image/bmp" }
		};

		public string Base64 { get; set; } = string.Empty;

		public string MimeType => GetMimeType(Base64);

		public string ViewDataUri => $"data:{MimeType};base64,{Base64}";

		private static string GetMimeType(string base64)
		{
			if (string.IsNullOrWhiteSpace(base64))
				return string.Empty;

			base64 = base64.Trim();

			var mimeTypeMatch = _magicMimePrefixes.FirstOrDefault(x => base64.StartsWith(x.Key));

			return !string.IsNullOrEmpty(mimeTypeMatch.Value) ? mimeTypeMatch.Value : "application/octet-stream";
		}
	}
}
