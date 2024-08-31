namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto
{
	public record CompanyForEmployeeDto
	{
		public int CompanyId { get; set; }
		public string CompanyName { get; set; } = string.Empty;
	}
}
