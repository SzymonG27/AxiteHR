namespace AxiteHR.Services.AuthAPI.Models.Auth.Dto
{
	public record RegisterResponseDto
	{
		public bool IsRegisteredSuccessful { get; set; }
		public string ErrorMessage { get; set; } = string.Empty;
		public string UserId { get; set; } = string.Empty;
	}
}
