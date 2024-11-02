namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto
{
	public record CompanyRoleDto
	{
		public int Id { get; set; }

		public string RoleName { get; set; } = string.Empty;

		public bool IsMain { get; set; }

		public bool IsVisible { get; set; }
	}
}
