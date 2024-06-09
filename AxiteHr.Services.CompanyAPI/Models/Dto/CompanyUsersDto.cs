namespace AxiteHr.Services.CompanyAPI.Models.Dto
{
	public class CompanyUsersDto
	{
		public CompanyDto Company { get; set; } = new CompanyDto();
		public IList<CompanyUser> UserList { get; set; } = new List<CompanyUser>();
	}
}
