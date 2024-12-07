namespace AxiteHR.Integration.GlobalClass.RedisKeys
{
	public static class CompanyRedisKeys
	{
		public static string CompanyUserGetId(int companyId, Guid userId) => $"CompanyUser:GetId:CompanyId:{companyId}:UserId:{userId}";

		public static string IsCompanyUserHasPermission(int companyUserId, int permissionId) => $"CompanyPermission:HasPermission:CompanyUserId:{companyUserId}:permissionId:{permissionId}";
		public static string IsCompanyUserHasAnyPermission(int companyUserId, string permissionIdString) => $"CompanyPermission:HasAnyPermission:CompanyUserId:{companyUserId}:permissionIdList:{permissionIdString}";
	}
}
