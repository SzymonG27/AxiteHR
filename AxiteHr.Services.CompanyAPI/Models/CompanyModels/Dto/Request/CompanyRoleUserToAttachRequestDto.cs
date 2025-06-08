namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Request
{
	public record CompanyRoleUserToAttachRequestDto
	{
		public int CompanyId { get; set; }

		public Guid UserRequestedId { get; set; }
	}
}
