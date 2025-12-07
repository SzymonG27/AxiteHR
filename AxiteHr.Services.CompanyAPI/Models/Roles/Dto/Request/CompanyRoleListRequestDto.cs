namespace AxiteHR.Services.CompanyAPI.Models.Roles.Dto.Request
{
	public record CompanyRoleListRequestDto
	{
		public int CompanyId { get; set; }

		public Guid UserRequestedId { get; set; }

		public string? RoleName { get; set; }
	}
}
