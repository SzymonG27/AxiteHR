namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto
{
	public record CompanyUserPermissionsDto
	{
		public CompanyUserDto CompanyUser { get; set; } = new();
		public IList<CompanyPermissionDto> CompanyUserPermissionList { get; set; } = [];
	}
}
