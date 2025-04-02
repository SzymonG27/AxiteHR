using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Integration.Cache.Redis;
using AxiteHR.Integration.GlobalClass.Redis.Keys;
using AxiteHR.Integration.JwtTokenHandler;
using AxiteHR.Services.ApplicationAPI.Data;
using AxiteHR.Services.ApplicationAPI.Extensions;
using AxiteHR.Services.ApplicationAPI.Helpers;
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
		IHttpClientFactory httpClientFactory,
		IStringLocalizer<ApplicationResources> applicationLocalizer,
		IJwtDecode jwtDecode,
		IRedisCacheService redisCacheService) : IApplicationService
	{
		private const double WorkHoursPerDay = 8.0; //TODO Can be configured for user

		public async Task<CreateApplicationResponseDto> CreateNewUserApplicationAsync(
			CreateApplicationRequestDto createApplicationRequestDto,
			string bearerToken,
			string acceptLanguage)
		{
			await using var transaction = await dbContext.Database.BeginTransactionAsync();
			try
			{
				var insUserId = jwtDecode.GetUserIdFromToken(bearerToken);
				if (insUserId is null)
				{
					var param = new
					{
						request = createApplicationRequestDto,
						bearerToken
					};
					Log.Error("Error while creating new user application. Token was null or userId from token was null. Param: {Param}", param);
					return new CreateApplicationResponseDto
					{
						IsSucceeded = false,
						ErrorMessage = applicationLocalizer[ApplicationResources.CreateApplication_InternalError]
					};
				}

				var companyUserId = await GetCompanyUserIdAsync(
					createApplicationRequestDto.CompanyId,
					createApplicationRequestDto.UserId,
					bearerToken,
					acceptLanguage);
				if (companyUserId is null)
				{
					var param = new
					{
						request = createApplicationRequestDto,
						bearerToken
					};
					Log.Error("Error while creating new user application. UserId is not exists in company. Param: {Param}", param);

					//Internal error, we don't expect such situation
					return new CreateApplicationResponseDto
					{
						IsSucceeded = false,
						ErrorMessage = applicationLocalizer[ApplicationResources.CreateApplication_InternalError]
					};
				}

				var isUserCanManageApplication = await IsUserCanManageApplicationForCompanyUserAsync(
					bearerToken,
					acceptLanguage,
					companyUserId.Value,
					insUserId.Value);
				if (!isUserCanManageApplication)
				{
					return new CreateApplicationResponseDto
					{
						IsSucceeded = false,
						ErrorMessage = applicationLocalizer[ApplicationResourcesKeys.CreateApplication_UserDontHavePermissionToCreateApplication]
					};
				}

				var applicationTypeThatIntestects = await GetApplicationTypeThatIntersectsWithPeriodAsync(
					createApplicationRequestDto,
					companyUserId.Value);
				if (applicationTypeThatIntestects.Count > 0
					&& IsApplicationTypeIntersectWithAnother(createApplicationRequestDto.ApplicationType, applicationTypeThatIntestects))
				{
					return new CreateApplicationResponseDto
					{
						IsSucceeded = false,
						ErrorMessage = applicationLocalizer[ApplicationResourcesKeys.CreateApplication_ApplicationIntersectsError]
					};
				}

				UserCompanyDaysOff? userDaysOff = null;
				decimal workingDaysEquivalent = 0;
				if (!createApplicationRequestDto.ApplicationType.IsTypeThatDontCountDaysOff())
				{
					userDaysOff = await GetUserDaysOffAsync(createApplicationRequestDto, companyUserId.Value);
					if (userDaysOff == null || !IsUserHaveEnoughDaysOff(createApplicationRequestDto, userDaysOff, out workingDaysEquivalent))
					{
						return new CreateApplicationResponseDto
						{
							IsSucceeded = false,
							ErrorMessage = applicationLocalizer[ApplicationResourcesKeys.CreateApplication_NotEnoughDaysOffError]
						};
					}
				}

				var createdUserApplication = UserApplicationMap.Map(createApplicationRequestDto, insUserId.Value, companyUserId.Value);
				await dbContext.UserApplications.AddAsync(createdUserApplication);

				if (!createApplicationRequestDto.ApplicationType.IsTypeThatDontCountDaysOff())
				{
					userDaysOff!.DaysOff -= workingDaysEquivalent;
				}

				await dbContext.SaveChangesAsync();
				await transaction.CommitAsync();

				return new CreateApplicationResponseDto
				{
					IsSucceeded = true
				};
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Error while trying to create a new user application. Param: {CreateApplicationRequestDto}", createApplicationRequestDto);
				await transaction.RollbackAsync();

				return new CreateApplicationResponseDto
				{
					IsSucceeded = false,
					ErrorMessage = applicationLocalizer[ApplicationResources.CreateApplication_InternalError]
				};
			}
		}

		#region Private Methods
		private async Task<int?> GetCompanyUserIdAsync(int companyId, Guid userId, string token, string acceptLanguage)
		{
			var companyUserIdFromCache = await redisCacheService.GetObjectAsync<int>(CompanyRedisKeys.CompanyUserGetId(companyId, userId));
			if (companyUserIdFromCache is not default(int))
			{
				return companyUserIdFromCache;
			}

			var client = httpClientFactory.CreateClient(HttpClientNameHelper.Company);
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
			client.DefaultRequestHeaders.Add("Accept-Language", acceptLanguage);

			var requestUri = $"{ApiLinkHelper.CompanyGetCompanyUserId}/{companyId}/{userId}";
			var response = await client.GetAsync(requestUri);
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			if (string.IsNullOrEmpty(content))
			{
				return null;
			}

			return int.Parse(content);
		}

		private async Task<bool> IsUserCanManageApplicationForCompanyUserAsync(string token, string acceptLanguage, int companyUserId, Guid insUserId)
		{
			var client = httpClientFactory.CreateClient(HttpClientNameHelper.Company);
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
			client.DefaultRequestHeaders.Add("Accept-Language", acceptLanguage);

			var requestUri = $"{ApiLinkHelper.CompanyIsUserCanManageApplications}/{companyUserId}&{insUserId}";
			var response = await client.GetAsync(requestUri);
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			return bool.Parse(content);
		}

		private async Task<List<ApplicationType>> GetApplicationTypeThatIntersectsWithPeriodAsync(CreateApplicationRequestDto dto, int companyUserId)
		{
			return await dbContext.UserApplications
				.AsNoTracking()
				.Where(x => x.CompanyUserId == companyUserId)
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

		private async Task<UserCompanyDaysOff?> GetUserDaysOffAsync(CreateApplicationRequestDto dto, int companyUserId)
		{
			return await dbContext.UserCompanyDaysOffs
				.Where(x => x.ApplicationType == dto.ApplicationType)
				.Where(x => x.CompanyUserId == companyUserId)
				.SingleOrDefaultAsync();
		}
		#endregion Private Methods
	}
}
