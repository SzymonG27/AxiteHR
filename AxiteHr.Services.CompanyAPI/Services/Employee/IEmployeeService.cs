using AxiteHr.Services.CompanyAPI.Models.EmployeeModels.Dto;

namespace AxiteHr.Services.CompanyAPI.Services.Employee
{
	public interface IEmployeeService
	{
		/// <summary>
		/// Create new employee request
		/// </summary>
		/// <param name="requestDto">DTO of new employee data</param>
		/// <param name="token">Token for creating auth user request</param>
		/// <param name="acceptLanguage">Accept language to header. Possible values = { "pl", "en" }</param>
		/// <returns>NewEmployeeResponseDto</returns>
		Task<NewEmployeeResponseDto> CreateNewEmployeeAsync(NewEmployeeRequestDto requestDto, string token, string acceptLanguage);
	}
}
