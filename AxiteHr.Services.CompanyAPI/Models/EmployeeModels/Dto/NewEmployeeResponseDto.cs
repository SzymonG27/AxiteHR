namespace AxiteHr.Services.CompanyAPI.Models.EmployeeModels.Dto
{
	public class NewEmployeeResponseDto
	{
		public bool IsSucceeded { get; set; }
		public string ErrorMessage { get; set; } = string.Empty;
		public int? EmployeeId { get; set; }
	}
}
