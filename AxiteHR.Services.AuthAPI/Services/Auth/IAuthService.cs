using AxiteHR.Services.AuthAPI.Models.Auth.Dto;

namespace AxiteHR.Services.AuthAPI.Services.Auth
{
	public interface IAuthService
	{
		Task<LoginResponseDto> Login(LoginRequestDto loginRequest);
		Task<RegisterResponseDto> Register(RegisterRequestDto registerRequest);
	}
}
