namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Response
{
	public record CompanyRoleUserToAttachResponseDto
	{
		public string FirstName { get; set; } = string.Empty;

		public string LastName { get; set; } = string.Empty;

		public string Mail { get; set; } = string.Empty;
	}
}
