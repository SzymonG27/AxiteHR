namespace AxiteHR.Services.CompanyAPI.Models.Roles.Dto.Response
{
	public record CompanyRoleCreatorResponseDto
	{
		public bool IsSucceeded { get; set; }

		public string ErrorMessage { get; set; } = string.Empty;

		public int CompanyRoleId { get; set; }

		public int CompanyRoleCompanyId { get; set; }
	}
}
