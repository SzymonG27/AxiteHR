namespace AxiteHr.Services.CompanyAPI.Models.Dto
{
	public class CompanyUserPermissionsDto
	{
		public CompanyUserDto CompanyUser { get; set; } = new CompanyUserDto();
		public IList<CompanyPermissionDto> CompanyUserPermissionList { get; set; } = new List<CompanyPermissionDto>();
	}
}
