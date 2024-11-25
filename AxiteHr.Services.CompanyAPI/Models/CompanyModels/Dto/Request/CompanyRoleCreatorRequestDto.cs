namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Request
{
	public record CompanyRoleCreatorRequestDto
	{
		public int CompanyId { get; set; }

		public Guid UserRequestedId { get; set; }

		public string RoleName { get; set; } = string.Empty;

		public string RoleNameEng { get; set; } = string.Empty;

		public string RoleNameTrimmed => RoleName.Trim();

		public string RoleNameEngTrimmed => RoleNameEng.Trim();
	}
}
