namespace AxiteHR.Services.ApplicationAPI.Models.Application.Dto
{
	public class CreateApplicationResponseDto
	{
		public bool IsSucceeded { get; set; }
		public string ErrorMessage { get; set; } = string.Empty;
	}
}
