namespace AxiteHR.Integration.GlobalClass.RedisKeys
{
	public static class CompanyRedisKeys
	{
		public static string CompanyUserGetId(int companyId, Guid userId) => $"CompanyUser_GetId_CompanyId:{companyId}_UserId:{userId}";
	}
}
