namespace AxiteHR.Integration.GlobalClass.Redis.Keys
{
	public static class CompanyRedisKeys
	{
		//Single object
		public static string CompanyUserGetId(int companyId, Guid userId) => $"CompanyUser_GetId_CompanyId:{companyId}_UserId:{userId}";

		//List of objects
	}
}
