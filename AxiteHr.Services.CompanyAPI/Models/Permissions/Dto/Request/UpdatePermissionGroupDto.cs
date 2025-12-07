namespace AxiteHR.Services.CompanyAPI.Models.Permissions.Dto.Request
{
	public record UpdatePermissionGroupDto
	{
		public int GroupId { get; set; }
		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }
		public bool IsActive { get; set; }
	}
}
