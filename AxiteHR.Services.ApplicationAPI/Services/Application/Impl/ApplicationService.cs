using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Services.ApplicationAPI.Data;
using AxiteHR.Services.ApplicationAPI.Maps;
using AxiteHR.Services.ApplicationAPI.Models.Application;
using AxiteHR.Services.ApplicationAPI.Models.Application.Dto;
using AxiteHR.Services.ApplicationAPI.Models.Application.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Serilog;

namespace AxiteHR.Services.ApplicationAPI.Services.Application.Impl
{
	public class ApplicationService(
		AppDbContext dbContext,
		IStringLocalizer<ApplicationResources> applicationLocalizer) : IApplicationService
	{
		private const double WorkHoursPerDay = 8.0; //TODO Can be configured for user

		public async Task<CreateApplicationResponseDto> CreateNewUserApplicationAsync(CreateApplicationRequestDto createApplicationRequestDto)
		{
			await using var transaction = await dbContext.Database.BeginTransactionAsync();
			try
			{
				var applicationTypeThatIntestects = await GetApplicationTypeThatIntersectsWithPeriodAsync(createApplicationRequestDto);
				if (applicationTypeThatIntestects.Count > 0
					&& IsApplicationTypeIntersectWithAnother(createApplicationRequestDto.ApplicationType, applicationTypeThatIntestects))
				{
					return new CreateApplicationResponseDto
					{
						IsSucceeded = false,
						ErrorMessage = applicationLocalizer[ApplicationResourcesKeys.CreateApplication_ApplicationIntersectsError]
					};
				}

				var userDaysOff = await GetUserDaysOffAsync(createApplicationRequestDto);
				if (userDaysOff == null || !IsUserHaveEnoughDaysOff(createApplicationRequestDto, userDaysOff, out decimal workingDaysEquivalent))
				{
					return new CreateApplicationResponseDto
					{
						IsSucceeded = false,
						ErrorMessage = applicationLocalizer[ApplicationResourcesKeys.CreateApplication_NotEnoughDaysOffError]
					};
				}

				var createdUserApplication = UserApplicationMap.Map(createApplicationRequestDto);
				await dbContext.UserApplications.AddAsync(createdUserApplication);
				userDaysOff.DaysOff -= workingDaysEquivalent;

				await dbContext.SaveChangesAsync();
				await transaction.CommitAsync();

				return new CreateApplicationResponseDto
				{
					IsSucceeded = true
				};
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Error while trying to create a new user application.", createApplicationRequestDto);
				await transaction.RollbackAsync();

				return new CreateApplicationResponseDto
				{
					IsSucceeded = false,
					ErrorMessage = applicationLocalizer[ApplicationResources.CreateApplication_InternalError]
				};
			}
		}

		#region Private Methods
		private async Task<List<ApplicationType>> GetApplicationTypeThatIntersectsWithPeriodAsync(CreateApplicationRequestDto dto)
		{
			return await dbContext.UserApplications
				.AsNoTracking()
				.Where(x => x.CompanyUserId == dto.CompanyUserId)
				.Where(x => x.ApplicationStatus != ApplicationStatus.Canceled)
				.Where(x => x.DateFrom <= dto.PeriodTo && x.DateTo >= dto.PeriodFrom)
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
			switch (applicationType)
			{
				case ApplicationType.Vacation:
				case ApplicationType.Inaccessibility:
				case ApplicationType.UnpaidBreak:
					return applicationTypesThatIntersects
						.Exists(x => x is ApplicationType.Vacation || x is ApplicationType.Inaccessibility || x is ApplicationType.UnpaidBreak);
				case ApplicationType.HomeWork:
					return applicationTypesThatIntersects
						.Exists(x => x is ApplicationType.HomeWork);
				default:
					return false;

			}
		}

		private static bool IsUserHaveEnoughDaysOff(CreateApplicationRequestDto createApplicationRequestDto,
			UserCompanyDaysOff? userDaysOff,
			out decimal workingDaysEquivalent)
		{
			int workingDays = GetWorkingDays(createApplicationRequestDto.PeriodFrom, createApplicationRequestDto.PeriodTo);

			double startHour = createApplicationRequestDto.PeriodFrom.TimeOfDay.TotalHours;
			double endHour = createApplicationRequestDto.PeriodTo.TimeOfDay.TotalHours;

			double firstDayHours = WorkHoursPerDay - startHour;

			double lastDayHours = endHour;

			double totalWorkingHours = firstDayHours + lastDayHours + ((workingDays - 2) * WorkHoursPerDay);
			workingDaysEquivalent = (decimal)(totalWorkingHours / WorkHoursPerDay);

			return userDaysOff != null && userDaysOff.DaysOff >= workingDaysEquivalent;
		}

		private static int GetWorkingDays(DateTime start, DateTime end)
		{
			int totalDays = (int)(end.Date - start.Date).TotalDays + 1;
			int fullWeekCount = totalDays / 7;
			int workingDays = fullWeekCount * 5;

			int remainingDays = totalDays % 7;
			DateTime tempDate = start.Date.AddDays(fullWeekCount * 7);

			for (int i = 0; i < remainingDays; i++)
			{
				if (tempDate.DayOfWeek != DayOfWeek.Saturday && tempDate.DayOfWeek != DayOfWeek.Sunday)
					workingDays++;
				tempDate = tempDate.AddDays(1);
			}
			return workingDays;
		}

		private async Task<UserCompanyDaysOff?> GetUserDaysOffAsync(CreateApplicationRequestDto dto)
		{
			return await dbContext.UserCompanyDaysOffs
				.Where(x => x.ApplicationType == dto.ApplicationType)
				.Where(x => x.CompanyUserId == dto.CompanyUserId)
				.SingleOrDefaultAsync();
		}
		#endregion Private Methods
	}
}
