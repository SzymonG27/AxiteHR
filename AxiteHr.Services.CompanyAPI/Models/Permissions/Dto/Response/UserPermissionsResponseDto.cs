namespace AxiteHR.Services.CompanyAPI.Models.Permissions.Dto.Response
{
	public record UserPermissionsResponseDto
	{
		public int? CompanyUserId { get; set; }
		public int CompanyId { get; set; }
		public Guid UserId { get; set; }
		public List<int> Permissions { get; set; } = [];
	}
}
