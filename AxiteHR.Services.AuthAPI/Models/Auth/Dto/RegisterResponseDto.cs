namespace AxiteHR.Services.AuthAPI.Models.Auth.Dto
{
	public class RegisterResponseDto
	{
		public bool IsRegisteredSuccessful { get; set; }
		public string ErrorMessage { get; set; } = string.Empty;
	}
}
