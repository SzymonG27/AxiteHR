namespace AxiteHr.Integration.GlobalClass.Auth
{
	public static class Roles
	{
		public const string Admin = "Admin";
		public const string User = "User";
		public const string UserFromCompany = "UserFromCompany";

		internal static readonly List<string> ManageCompanyEnable = [Admin, User];
	}
}