namespace AxiteHR.Services.CompanyAPI.Models.Permissions.Dto.Request
{
	public record CreatePermissionGroupDto
	{
		public int CompanyId { get; set; }
		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }
		public List<int> PermissionIds { get; set; } = [];
	}
}
