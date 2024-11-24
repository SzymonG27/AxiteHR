namespace AxiteHR.Integration.GlobalClass.RedisKeys
{
	public static class CompanyRedisKeys
	{
		public static string CompanyUserGetId(int companyId, Guid userId) => $"CompanyUser_GetId_CompanyId:{companyId}_UserId:{userId}";

		public static string IsCompanyUserHasPermission(int companyUserId, int permissionId) => $"CompanyPermission_HasPermission_CompanyUserId:{companyUserId}_permissionId:{permissionId}";
		public static string IsCompanyUserHasAnyPermission(int companyUserId, string permissionIdString) => $"CompanyPermission_HasAnyPermission_CompanyUserId:{companyUserId}_permissionIdList:{permissionIdString}";
	}
}
