namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Request
{
	public record CompanyRoleCreatorRequestDto
	{
		public int CompanyId { get; set; }

		public Guid UserRequestedId { get; set; }
	}
}
