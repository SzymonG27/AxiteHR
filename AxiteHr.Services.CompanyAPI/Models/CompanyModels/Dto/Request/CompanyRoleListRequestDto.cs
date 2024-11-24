namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Request
{
	public record CompanyRoleListRequestDto
	{
		public int CompanyId { get; set; }

		public Guid UserRequestedId { get; set; }
	}
}
