using AxiteHR.Services.ApplicationAPI.Models.Application;
using AxiteHR.Services.ApplicationAPI.Models.Application.Dto;

namespace AxiteHR.Services.ApplicationAPI.Maps
{
	public static class UserApplicationMap
	{
		public static UserApplication Map(CreateApplicationRequestDto dto, Guid insUserId, int companyUserId)
		{
			return new UserApplication
			{
				CompanyUserId = companyUserId,
				ApplicationType = dto.ApplicationType,
				ApplicationStatus = Models.Application.Enums.ApplicationStatus.New,
				DateFrom = dto.PeriodFrom,
				DateTo = dto.PeriodTo,
				Reason = dto.Reason,
				InsUserId = insUserId,
				InsDate = DateTime.UtcNow,
				UpdUserId = insUserId,
				UpdDate = DateTime.UtcNow
			};
		}
	}
}
