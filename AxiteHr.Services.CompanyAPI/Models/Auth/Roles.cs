namespace AxiteHr.Services.CompanyAPI.Models.Auth
{
	public static class Roles
	{
		public const string Admin = "Admin";
		public const string User = "User";
		public const string UserFromCompany = "UserFromCompany";

		internal static readonly List<string> ManageCompanyEnable = new()
		{
			Admin, User
		};
	}
}
