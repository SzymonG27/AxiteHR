namespace AxiteHR.Services.ApplicationAPI.Models.Application.Dto
{
	public record CreateApplicationResponseDto
	{
		public bool IsSucceeded { get; set; }
		public string ErrorMessage { get; set; } = string.Empty;
	}
}
