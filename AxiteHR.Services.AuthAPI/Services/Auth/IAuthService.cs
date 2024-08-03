using AxiteHR.Services.AuthAPI.Models.Auth.Dto;
using AxiteHR.Services.AuthAPI.Models.EmployeeModels.Dto;

namespace AxiteHR.Services.AuthAPI.Services.Auth
{
	public interface IAuthService
	{
		Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest);

		Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto registerRequest);

		Task<NewEmployeeResponseDto> RegisterNewEmployeeAsync(NewEmployeeRequestDto newEmployeeRequestDto);

		Task<TempPasswordChangeResponseDto> TempPasswordChangeAsync(TempPasswordChangeRequestDto newPasswordChangeRequest);
	}
}
