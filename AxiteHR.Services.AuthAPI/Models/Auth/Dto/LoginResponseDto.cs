namespace AxiteHR.Services.AuthAPI.Models.Auth.Dto
{
	public record LoginResponseDto
	{
		public bool IsLoggedSuccessful { get; set; }
		public bool IsTempPasswordToChange { get; set; }
		public string ErrorMessage { get; set; } = string.Empty;
		public string Token { get; set; } = string.Empty;
		public string? UserId { get; set; }
	}
}
