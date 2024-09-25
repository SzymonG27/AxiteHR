using AxiteHR.Services.ApplicationAPI.Models.Application;
using AxiteHR.Services.ApplicationAPI.Models.Application.Dto;

namespace AxiteHR.Services.ApplicationAPI.Maps
{
	public static class UserApplicationMap
	{
		public static UserApplication Map(CreateApplicationRequestDto dto)
		{
			return new UserApplication
			{
				CompanyUserId = dto.CompanyUserId,
				ApplicationType = dto.ApplicationType,
				ApplicationStatus = Models.Application.Enums.ApplicationStatus.New,
				DateFrom = dto.PeriodFrom,
				DateTo = dto.PeriodTo,
				InsUserId = dto.InsUserId,
				InsDate = DateTime.UtcNow,
				UpdUserId = dto.InsUserId,
				UpdDate = DateTime.UtcNow
			};
		}
	}
}
