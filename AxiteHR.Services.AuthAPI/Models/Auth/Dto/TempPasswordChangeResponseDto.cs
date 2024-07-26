namespace AxiteHR.Services.AuthAPI.Models.Auth.Dto
{
	public class TempPasswordChangeResponseDto
	{
		public bool IsSucceeded { get; set; }
		public string ErrorMessage { get; set; } = string.Empty;
		public string UserId { get; set; } = string.Empty;
	}
}
