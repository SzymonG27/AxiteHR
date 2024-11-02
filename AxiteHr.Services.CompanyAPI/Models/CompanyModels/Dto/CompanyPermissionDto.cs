namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto
{
	public record CompanyPermissionDto
	{
		public int Id { get; set; }

		public string PermissionName { get; set; } = string.Empty;
	}
}
