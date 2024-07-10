using AxiteHr.Services.CompanyAPI.Models.EmployeeModels.Dto;

namespace AxiteHr.Services.CompanyAPI.Services.Employee
{
	public interface IEmployeeService
	{
		Task<NewEmployeeResponseDto> CreateNewEmployee(NewEmployeeRequestDto requestDto);
	}
}
