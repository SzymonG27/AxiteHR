using AxiteHR.Services.CompanyAPI.Infrastructure;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto;

namespace AxiteHR.Services.CompanyAPI.Services.CompanyUser
{
	public interface ICompanyUserService
	{
		/// <summary>
		/// Get enumerable dto of employees contains in company, exclude user who is searching
		/// </summary>
		/// <param name="companyId">Company identifier</param>
		/// <param name="excludedUserId">User identifier to exclude from list</param>
		/// <param name="paginationInfo">Pagination info</param>
		/// <param name="bearerToken">Token for auth data request</param>
		/// <returns>IEnumerable of CompanyUserViewDto</returns>
		Task<IEnumerable<CompanyUserViewDto>> GetCompanyUserViewDtoListAsync(int companyId, Guid excludedUserId, Pagination paginationInfo, string bearerToken);

		/// <summary>
		/// Get count of employees contains in company, exclude user who is searching
		/// </summary>
		/// <param name="companyId">Company identifier</param>
		/// <param name="excludedUserId">User identifier to exclude from count</param>
		/// <returns>Company users count</returns>
		Task<int> GetCompanyUsersCountAsync(int companyId, Guid excludedUserId);

		/// <summary>
		/// Determines whether a specified user is associated with a given company.
		/// </summary>
		/// <param name="userId">The unique identifier of the user to check.</param>
		/// <param name="companyId">The unique identifier of the company to check against.</param>
		/// <returns>
		/// A boolean value wrapped in an observable indicating whether the user is a member of the company:
		/// <list type="bullet">
		/// <item>
		/// <description><c>true</c> - The user is a member of the specified company.</description>
		/// </item>
		/// <item>
		/// <description><c>false</c> - The user is not a member of the specified company.</description>
		/// </item>
		/// </list>
		/// </returns>
		Task<bool> IsUserInCompanyAsync(Guid userId, int companyId);

		/// <summary>
		/// Checks if a user has permission to manage applications for a specified company user.
		/// Steps:
		/// 1. Attempts to retrieve the company user by the provided companyUserId. If not found, returns false.
		/// 2. If the retrieved company user's UserId matches the provided insUserId, returns true, allowing the user to manage their own application.
		/// 3. Retrieves the main role of the insUserId within the company:
		///    - If the role is not found or the user is not a supervisor, returns false.
		/// 4. Retrieves the main role of the target company user.
		/// 5. If both roles match (same CompanyRoleId), returns true, allowing management rights based on role compatibility.
		/// Returns:
		/// True if the user can manage applications for the specified company user; otherwise, false.
		/// </summary>
		/// <param name="companyUserId">The ID of the company user to be managed.</param>
		/// <param name="insUserId">The ID of the user attempting to manage applications.</param>
		/// <returns>Task<bool> indicating whether the user can manage applications for the specified company user.</returns>
		Task<bool> IsUserCanManageApplicationsAsync(int companyUserId, Guid insUserId);
	}
}
