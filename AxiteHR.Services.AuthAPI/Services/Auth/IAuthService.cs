using AxiteHR.Services.AuthAPI.Models.Auth.Dto;
using AxiteHR.Services.AuthAPI.Models.EmployeeModels.Dto;

namespace AxiteHR.Services.AuthAPI.Services.Auth
{
	public interface IAuthService
	{
		/// <summary>
		/// This method is used to log in user (app and company)
		/// </summary>
		/// <param name="loginRequest"></param>
		/// <returns>LoginResponseDto</returns>
		Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest);

		/// <summary>
		/// This method is used to register app user
		/// </summary>
		/// <param name="registerRequest"></param>
		/// <returns>RegisterResponseDto</returns>
		Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto registerRequest);

		/// <summary>
		/// This method is used to register new employee for company
		/// </summary>
		/// <param name="newEmployeeRequestDto"></param>
		/// <returns>NewEmployeeResponseDto</returns>
		Task<NewEmployeeResponseDto> RegisterNewEmployeeAsync(NewEmployeeRequestDto newEmployeeRequestDto);

		/// <summary>
		/// This method is used to changing temporary employee password
		/// </summary>
		/// <param name="newPasswordChangeRequest"></param>
		/// <returns>TempPasswordChangeResponseDto</returns>
		Task<TempPasswordChangeResponseDto> TempPasswordChangeAsync(TempPasswordChangeRequestDto newPasswordChangeRequest);
	}
}
