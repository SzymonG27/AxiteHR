namespace AxiteHR.Services.AuthAPI.Models.Auth.Dto
{
	public class LoginResponseDto
	{
		public bool IsLoggedSuccessful { get; set; }
		public string ErrorMessage { get; set; } = string.Empty;
		public string Token { get; set; } = string.Empty;
	}
}
