namespace AxiteHR.Integration.GlobalClass.Redis.Keys
{
	public static class CompanyRedisKeys
	{
		//Single object
		public static string CompanyUserGetId(int companyId, Guid userId) => $"CompanyUser:GetId:CompanyId:{companyId}:UserId:{userId}";

		public static string IsCompanyUserHasPermission(int companyUserId, string permissionId) => $"CompanyPermission:HasPermission:CompanyUserId:{companyUserId}:permissionId:{permissionId}";
		public static string IsCompanyUserHasPermissionDeletePattern(int companyUserId) => $"CompanyPermission:HasPermission:CompanyUserId:{companyUserId}:permissionId:*";

		public static string IsCompanyUserHasAnyPermission(int companyUserId, string permissionIdString) => $"CompanyPermission:HasAnyPermission:CompanyUserId:{companyUserId}:permissionIdList:{permissionIdString}";
		public static string IsCompanyUserHasAnyPermissionDeletePattern(int companyUserId) => $"CompanyPermission:HasAnyPermission:CompanyUserId:{companyUserId}:permissionIdList:*";

		public static string AllUserPermissions(int companyUserId) => $"CompanyPermission:AllUserPermissions:CompanyUserId:{companyUserId}";

		//List of objects
	}
}
