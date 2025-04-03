namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Request
{
	public record CompanyRoleAttachUserRequestDto
	{
		public int CompanyRoleCompanyId { get; set; }

		public int CompanyUserToAttachId { get; set; }

		public int CompanyUserRequestedId { get; set; }

		public Guid UserRequestedId { get; set; }
	}
}
