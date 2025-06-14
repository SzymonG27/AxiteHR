namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Response
{
	public record CompanyRoleAttachUserResponseDto
	{
		public bool IsSucceeded { get; set; }

		public string ErrorMessage { get; set; } = string.Empty;

		public int UserRoleId { get; set; }
	}
}
