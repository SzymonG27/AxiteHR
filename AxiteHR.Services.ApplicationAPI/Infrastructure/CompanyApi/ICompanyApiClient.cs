namespace AxiteHR.Services.ApplicationAPI.Infrastructure.CompanyApi
{
	public interface ICompanyApiClient
	{
		Task<int?> GetCompanyUserIdAsync(int companyId, Guid userId, string bearerToken, string acceptLanguage);

		Task<bool> IsUserCanManageApplicationForCompanyUserAsync(string token, string acceptLanguage, int companyUserId, Guid insUserId);
	}
}
