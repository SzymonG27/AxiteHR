using AxiteHr.Services.CompanyAPI.Models.EmployeeModels.Dto;

namespace AxiteHr.Services.CompanyAPI.Services.Employee
{
	public interface IEmployeeService
	{
		Task<NewEmployeeResponseDto> CreateNewEmployeeAsync(NewEmployeeRequestDto requestDto, string token);
	}
}
