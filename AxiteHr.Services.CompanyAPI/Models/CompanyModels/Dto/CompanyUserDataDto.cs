namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto
{
	public class CompanyUserDataDto
	{
		public int CompanyUserId { get; set; }
		public string UserId { get; set; } = string.Empty;
		public string UserName { get; set; } = string.Empty;
		public string UserEmail { get; set; } = string.Empty;
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
	}
}
