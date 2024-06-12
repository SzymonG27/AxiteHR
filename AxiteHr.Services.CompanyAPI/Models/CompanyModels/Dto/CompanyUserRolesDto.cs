namespace AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto
{
	public class CompanyUserRolesDto
	{
		public CompanyUserDto CompanyUser { get; set; } = new CompanyUserDto();
		public IList<CompanyRoleDto> CompanyUserRoleList { get; set; } = new List<CompanyRoleDto>();
	}
}
