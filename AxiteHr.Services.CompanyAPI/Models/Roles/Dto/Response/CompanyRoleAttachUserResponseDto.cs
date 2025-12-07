namespace AxiteHR.Services.CompanyAPI.Models.Roles.Dto.Response
{
	public record CompanyRoleAttachUserResponseDto
	{
		public bool IsSucceeded { get; set; }

		public string ErrorMessage { get; set; } = string.Empty;

		public int UserRoleId { get; set; }
	}
}
