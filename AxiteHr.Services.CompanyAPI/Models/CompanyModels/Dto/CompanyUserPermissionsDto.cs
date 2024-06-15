namespace AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto
{
	public record CompanyUserPermissionsDto
	{
		public CompanyUserDto CompanyUser { get; set; } = new CompanyUserDto();
		public IList<CompanyPermissionDto> CompanyUserPermissionList { get; set; } = [];
	}
}
