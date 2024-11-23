namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Response
{
	public record CompanyRoleListResponseDto
	{
		public int CompanyRoleId { get; set; }
		public string Name { get; set; } = string.Empty;
		public bool IsMain { get; set; }
		public int EmployeesCount { get; set; }
	}
}
