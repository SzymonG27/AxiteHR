using AxiteHr.Services.CompanyAPI.Data;
using AxiteHr.Services.CompanyAPI.Helpers;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Const;
using AxiteHr.Services.CompanyAPI.Models.EmployeeModels.Dto;
using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Text.Json;

namespace AxiteHr.Services.CompanyAPI.Services.Employee.Impl
{
	public class EmployeeService(
		IHttpClientFactory httpClientFactory,
		IStringLocalizer<CompanyResources> companyLocalizer,
		ILogger<EmployeeService> logger,
		AppDbContext dbContext) : IEmployeeService
	{
		public async Task<NewEmployeeResponseDto> CreateNewEmployeeAsync(NewEmployeeRequestDto requestDto, string token)
		{
			NewEmployeeResponseDto? newEmployeeResponseDto;

			await using var transaction = await dbContext.Database.BeginTransactionAsync();
			try
			{
				newEmployeeResponseDto = await CreateNewAuthEmployeeAsync(requestDto, token);

				if (newEmployeeResponseDto?.IsSucceeded != true || string.IsNullOrEmpty(newEmployeeResponseDto.EmployeeId))
				{
					return newEmployeeResponseDto ??
						new NewEmployeeResponseDto
						{
							IsSucceeded = false,
							ErrorMessage = companyLocalizer[CompanyResourcesKeys.NewEmployeeRequestDto_RegisterError]
						};
				}

				await AssignEmployeeToCompanyAsync(newEmployeeResponseDto.EmployeeId, requestDto);

				await dbContext.SaveChangesAsync();
				await transaction.CommitAsync();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error while trying to create an new employee to company ID: {CompanyId}", requestDto.CompanyId);

				await transaction.RollbackAsync();

				newEmployeeResponseDto = new NewEmployeeResponseDto
				{
					IsSucceeded = false,
					ErrorMessage = companyLocalizer[CompanyResourcesKeys.NewEmployeeRequestDto_RegisterError]
				};
			}

			return newEmployeeResponseDto;
		}

		#region Private Methods
		private async Task<NewEmployeeResponseDto?> CreateNewAuthEmployeeAsync(NewEmployeeRequestDto requestDto, string token)
		{
			var client = httpClientFactory.CreateClient(HttpClientNameHelper.Auth);
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

			var jsonRequestDto = JsonSerializer.Serialize(requestDto);
			var stringContent = new StringContent(jsonRequestDto, System.Text.Encoding.UTF8, "application/json");
			var response = await client.PostAsync(ApiLinkHelper.RegisterNewEmployee, stringContent);

			var responseBody = await response.Content.ReadAsStringAsync();

			return JsonSerializer.Deserialize<NewEmployeeResponseDto>(responseBody);
		}

		private async Task AssignEmployeeToCompanyAsync(string employeeId, NewEmployeeRequestDto requestDto)
		{
			var company = await dbContext.Companies.SingleAsync(x => x.Id == requestDto.CompanyId);
			var companyUser = await CreateCompanyUserForEmployeeAsync();
			await AddEmployeeRolePermission();

			async Task<CompanyUser> CreateCompanyUserForEmployeeAsync()
			{
				CompanyUser newCompanyUser = new()
				{
					Company = company,
					UserId = Guid.Parse(employeeId),
					InsUserId = Guid.Parse(requestDto.InsUserId),
					InsDate = DateTime.UtcNow
				};
				await dbContext.CompanyUsers.AddAsync(newCompanyUser);
				return newCompanyUser;
			}

			async Task AddEmployeeRolePermission()
			{
				var companyEmployeePermission = await dbContext.CompanyPermissions.SingleAsync(x => x.Id == (int)PermissionDictionary.Employee);
				CompanyUserPermission newCompanyUserPermission = new()
				{
					CompanyUser = companyUser,
					CompanyPermission = companyEmployeePermission,
				};
				await dbContext.CompanyUserPermissions.AddAsync(newCompanyUserPermission);
			}
		}
		#endregion
	}
}
