namespace AxiteHR.Services.AuthAPI.Models.Dto
{
	public class LoginResponseDto
	{
		public UserDto User { get; set; } = new UserDto();
		public bool IsLoggedSuccessful { get; set; }
		public string ErrorMessage { get; set; } = string.Empty;
		public string Token { get; set; } = string.Empty;
	}
}
