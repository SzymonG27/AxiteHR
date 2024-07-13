namespace AxiteHR.Services.AuthAPI.Models.EmployeeModels.Dto
{
	public class NewEmployeeResponseDto
	{
		public bool IsSucceeded { get; set; }
		public string ErrorMessage { get; set; } = string.Empty;
		public string EmployeeId { get; set; } = string.Empty;
	}
}
