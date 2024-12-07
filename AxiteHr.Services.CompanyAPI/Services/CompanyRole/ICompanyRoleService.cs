using AxiteHR.Services.CompanyAPI.Infrastructure;
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

		/// <summary>
		/// <para>Creates a new role for a company or links an existing role to the company.</para>
		/// <para>
		/// **Steps**
		/// <para>1. **Authorization**: Checks if the user belongs to the company and has permission to create roles.</para>
		/// <para>2. **Role Check**: If the role exists, it links it to the company. If not, it creates a new role and links it to the company.</para>
		/// <para>3. **Transaction**: All changes are made in a database transaction. If an error occurs, the transaction is rolled back.</para>
		/// </para>
		/// <para>
		/// **Parameters**
		/// <param name="requestDto">Data for creating the role and linking it to the company.</param>
		/// </para>
		/// <para>
		/// **Returns**
		/// <returns>A response with the result of the operation, including role and link IDs if successful.</returns>
		/// </para>
		/// </summary>
		Task<CompanyRoleCreatorResponseDto> CreateAsync(CompanyRoleCreatorRequestDto requestDto);
	}
}
