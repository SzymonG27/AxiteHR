using AxiteHR.Services.ApplicationAPI.Data;
using AxiteHR.Services.ApplicationAPI.Models.Application.Dto;
using AxiteHR.Services.ApplicationAPI.Models.Application.Enums;
using Microsoft.EntityFrameworkCore;

namespace AxiteHR.Services.ApplicationAPI.Services.Application.Impl
{
	public class ApplicationService(AppDbContext dbContext) : IApplicationService
	{
		private const double WorkHoursPerDay = 8.0;

		public async Task<CreateApplicationResponseDto> CreateNewUserApplicationAsync(CreateApplicationRequestDto createApplicationRequestDto)
		{
			var applicationTypeThatIntestects = await GetApplicationTypeThatIntersectsWithPeriodAsync(createApplicationRequestDto);
			if (applicationTypeThatIntestects.Count > 0
				&& IsApplicationTypeIntersectWithAnother(createApplicationRequestDto.ApplicationType, applicationTypeThatIntestects))
			{
				return new CreateApplicationResponseDto
				{
					IsSucceeded = false,
					ErrorMessage = ""
				};
			}

			TimeSpan timeSpanOff = createApplicationRequestDto.PeriodTo - createApplicationRequestDto.PeriodFrom;
			double workingDaysEquivalent = timeSpanOff.TotalHours / WorkHoursPerDay;
			if (!await HasUserHaveDaysOffAsync(createApplicationRequestDto, workingDaysEquivalent))
			{
				return new CreateApplicationResponseDto
				{
					IsSucceeded = false,
					ErrorMessage = ""
				};
			}

			return new CreateApplicationResponseDto();
		}

		#region Private Methods
		private async Task<List<ApplicationType>> GetApplicationTypeThatIntersectsWithPeriodAsync(CreateApplicationRequestDto dto)
		{
			return await dbContext.UserApplications
				.AsNoTracking()
				.Where(x => x.CompanyUserId == dto.CompanyUserId)
				.Where(x => x.ApplicationStatus != ApplicationStatus.Canceled)
				.Where(x => x.DateFrom < dto.PeriodTo && x.DateTo > dto.PeriodFrom)
				.Select(x => x.ApplicationType)
				.ToListAsync();
		}


		/// <summary>
		/// This method checks whether a given application type can intersect with any of the application types specified 
		/// in the provided list of intersecting types. The method returns true if the application type is allowed to intersect 
		/// based on predefined rules.
		/// </summary>
		/// <param name="applicationType">The application type to check for intersections.</param>
		/// <param name="applicationTypesThatIntersects">A list of application types that are considered intersectable.</param>
		/// <returns>
		/// Returns <c>true</c> if the specified <paramref name="applicationType"/> can intersect with any type in 
		/// <paramref name="applicationTypesThatIntersects"/>. Returns <c>false</c> otherwise, including if no intersection
		/// rules apply for the provided application type.
		/// </returns>
		private static bool IsApplicationTypeIntersectWithAnother(ApplicationType applicationType, List<ApplicationType> applicationTypesThatIntersects)
		{
			return applicationType switch
			{
				ApplicationType.Vacation or ApplicationType.Inaccessibility => applicationTypesThatIntersects.Exists(x => x == ApplicationType.Vacation || x == ApplicationType.Inaccessibility),
				_ => false,
			};
		}

		private async Task<bool> HasUserHaveDaysOffAsync(CreateApplicationRequestDto dto, double workingDaysEquivalent)
		{
			var userDaysOff = await dbContext.UserCompanyDaysOffs
				.AsNoTracking()
				.Where(x => x.ApplicationType == dto.ApplicationType)
				.Where(x => x.CompanyUserId == dto.CompanyUserId)
				.Select(x => x.DaysOff)
				.SingleOrDefaultAsync();

			return userDaysOff != null && userDaysOff >= (decimal)workingDaysEquivalent;
		}
		#endregion Private Methods
	}
}
