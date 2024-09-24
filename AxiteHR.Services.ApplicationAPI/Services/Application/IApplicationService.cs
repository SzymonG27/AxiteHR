using AxiteHR.Services.ApplicationAPI.Models.Application.Dto;

namespace AxiteHR.Services.ApplicationAPI.Services.Application
{
	public interface IApplicationService
	{
		Task<CreateApplicationResponseDto> CreateNewUserApplicationAsync(CreateApplicationRequestDto createApplicationRequestDto);
	}
}
