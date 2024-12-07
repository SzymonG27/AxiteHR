namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Response
{
	public record CompanyRoleCreatorResponseDto
	{
		public bool IsSucceeded { get; set; }

		public string ErrorMessage { get; set; } = string.Empty;

		public int CompanyRoleId { get; set; }

		public int CompanyRoleCompanyId { get; set; }
	}
}
