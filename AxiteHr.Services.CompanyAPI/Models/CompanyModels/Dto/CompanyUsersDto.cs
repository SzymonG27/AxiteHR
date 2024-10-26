namespace AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto
{
	public record CompanyUsersDto
	{
		public CompanyDto Company { get; set; } = new();
		public IList<CompanyUser> UserList { get; set; } = [];
	}
}
