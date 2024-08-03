namespace AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto
{
	public record CompanyUsersDto
	{
		public CompanyDto Company { get; set; } = new CompanyDto();
		public IList<CompanyUser> UserList { get; set; } = [];
	}
}
