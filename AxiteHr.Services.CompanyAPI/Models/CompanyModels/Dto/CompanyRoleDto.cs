namespace AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto
{
	public class CompanyRoleDto
	{
		public int Id { get; set; }

		public string RoleName { get; set; } = string.Empty;

		public bool IsMain { get; set; }

		public bool IsVisible { get; set; }
	}
}
