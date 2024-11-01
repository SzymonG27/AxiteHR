namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto
{
	public record CompanyUserRolesDto
	{
		public CompanyUserDto CompanyUser { get; set; } = new();
		public IList<CompanyRoleDto> CompanyUserRoleList { get; set; } = [];
	}
}
