using AxiteHR.Services.CompanyAPI.Infrastructure;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Request;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Response;

namespace AxiteHR.Services.CompanyAPI.Services.CompanyRole
{
	public interface ICompanyRoleService
	{
		/// <summary>
		/// Retrieves a paginated list of company roles based on the specified request and pagination parameters. 
		/// Filters roles based on the visibility, company association, and the user's access permissions.
		/// </summary>
		/// <param name="requestDto">
		/// An instance of <see cref="CompanyRoleListRequestDto"/> containing the company ID and the user requesting the data.
		/// </param>
		/// <param name="pagination">
		/// An instance of <see cref="Pagination"/> specifying the page number and items per page for the result set. 
		/// If <see cref="Pagination.ItemsPerPage"/> is not greater than 0, a default value of 10 is used.
		/// </param>
		/// <returns>
		/// A task that represents the asynchronous operation. 
		/// The task result contains a collection of <see cref="CompanyRoleListResponseDto"/> objects representing the paginated company roles.
		/// </returns>
		/// <remarks>
		/// <para>If the requesting user is not associated with the company, an empty collection is returned.</para>
		/// <para>If the user does not have permission to view the entire list, only roles associated with the user are returned.</para>
		/// <para>Roles are grouped by their ID, name, and main status, and include the count of associated employees.</para>
		/// <para>The method uses a left join to associate roles with users and ensures that only visible roles are returned.</para>
		/// </remarks>
		Task<IEnumerable<CompanyRoleListResponseDto>> GetListAsync(CompanyRoleListRequestDto requestDto, Pagination pagination);

		/// <summary>
		/// Retrieves the total count of company roles based on the specified request parameters. 
		/// Filters the count based on the visibility, company association, and the user's access permissions.
		/// </summary>
		/// <param name="requestDto">
		/// An instance of <see cref="CompanyRoleListRequestDto"/> containing the company ID and the user requesting the data.
		/// </param>
		/// <returns>
		/// <para>A task that represents the asynchronous operation.</para>
		/// <para>The task result contains the total count of company roles visible to the requesting user.</para>
		/// </returns>
		/// <remarks>
		/// <para>If the requesting user is not associated with the company, the method returns 0.</para>
		/// <para>If the user does not have permission to view the entire list, the count is limited to roles associated with the user.</para>
		/// <para>The method filters roles by their visibility and company association.</para>
		/// </remarks>
		Task<int> GetCountListAsync(CompanyRoleListRequestDto requestDto);

		Task<IEnumerable<CompanyUserDataDto>> GetEmployeeListAsync(int companyId, Guid userRequestedId, Pagination pagination, string bearerToken);

		Task<int> GetCountEmployeesAsync(int companyId, Guid userRequestedId);

		/// <summary>
		/// <para>Creates a new role for a company or links an existing role to the company.</para>
		/// <para>
		/// **Steps**
		/// <para>1. **Authorization**: Checks if the user belongs to the company and has permission to create roles.</para>
		/// <para>2. **Role Check**: If the role exists, it links it to the company. If not, it creates a new role and links it to the company.</para>
		/// <para>3. **Transaction**: All changes are made in a database transaction. If an error occurs, the transaction is rolled back.</para>
		/// </para>
		/// </summary>
		/// <param name="requestDto">Data for creating the role and linking it to the company.</param>
		/// <returns>A response with the result of the operation, including role and link IDs if successful.</returns>
		Task<CompanyRoleCreatorResponseDto> CreateAsync(CompanyRoleCreatorRequestDto requestDto);

		/// <summary>
		/// Returns the total number of employees eligible to be assigned to a role,
		/// excluding those with a main role in the company. Used for pagination.
		/// </summary>
		/// <param name="companyId">Id of company</param>
		/// <param name="userRequestedId">Id of requesting user</param>
		/// <returns>Total count of eligible employees.</returns>
		Task<int> GetCountOfEmployeesToAttachAsync(int companyId, Guid userRequestedId);

		/// <summary>
		/// Returns a paginated list of employees eligible to be assigned to a role,
		/// excluding those with a main role in the company.
		/// </summary>
		/// <param name="companyId">Id of company</param>
		/// <param name="userRequestedId">Id of requesting user</param>
		/// <param name="pagination">Pagination settings.</param>
		/// <param name="bearerToken">Authorization token for the Auth API.</param>
		/// <returns>List of user data eligible for role assignment.</returns>
		Task<IEnumerable<CompanyUserDataDto>> GetListOfEmployeesToAttachAsync(int companyId, Guid userRequestedId, Pagination pagination, string bearerToken);

		/// <summary>
		/// <para>Attaches a user to a role within a company, checking the appropriate permissions and conditions.</para>
		/// <para>
		/// The method performs the following steps:
		/// <para>1. Checks if the requesting user (requestDto.CompanyUserRequestedId) is a supervisor in the given role or has the necessary permissions to attach a user to the role.</para>
		/// <para>2. Verifies that the user to be attached (requestDto.CompanyUserToAttachId) is not already assigned to the main role within the company.</para>
		/// <para>3. If all conditions are met, adds the user to the role in the company by creating a new record in the CompanyUserRoles table.</para>
		/// <para>4. Returns a response with the result of the operation and any error messages if applicable.</para>
		/// </para>
		/// </summary>
		/// <param name="requestDto">An object containing the data for attaching the user to the role.</param>
		/// <returns>A response object containing the result of the operation and any error messages.</returns>
		Task<CompanyRoleAttachUserResponseDto> AttachUserAsync(CompanyRoleAttachUserRequestDto requestDto);
	}
}
