using AxiteHR.Services.AuthAPI.Models.Dto;

namespace AxiteHR.Services.AuthAPI.Services
{
	public interface IAuthService
	{
		Task<LoginResponseDto> Login(LoginRequestDto loginRequest);
		Task<RegisterResponseDto> Register(RegisterRequestDto registerRequest);
	}
}
