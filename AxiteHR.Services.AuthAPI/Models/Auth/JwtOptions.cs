namespace AxiteHR.Services.AuthAPI.Models.Auth
{
	public class JwtOptions
	{
		public string Secret { get; set; } = string.Empty;
		public string Issuer { get; set; } = string.Empty;
		public string Audience { get; set; } = string.Empty;
		public int ExpiresInMins { get; set; }
	}
}
