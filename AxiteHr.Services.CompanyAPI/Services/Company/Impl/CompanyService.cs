using AxiteHr.Services.CompanyAPI.Helpers;
using AxiteHr.Services.CompanyAPI.Infrastructure;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto;
using AxiteHr.Services.CompanyAPI.Models.EmployeeModels.Dto;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto;
using System.Text.Json;

namespace AxiteHr.Services.CompanyAPI.Services.Company.Impl
{
	public class CompanyService(
		IHttpClientFactory httpClientFactory,
		ICompanyRepository companyRepository) : ICompanyService
	{
		public IEnumerable<CompanyListDto> GetCompanyList(Guid userId)
		{
			return companyRepository.GetCompanyList(userId);
		}

		public async Task<IEnumerable<CompanyUserViewDto>> GetCompanyUserViewDtoListAsync(int companyId, Guid excludedUserId, Pagination paginationInfo, string bearerToken)
		{
			if (paginationInfo.ItemsPerPage <= 0)
			{
				paginationInfo.ItemsPerPage = 10;
			}

			var companyUserIds = await companyRepository.GetCompanyUserUserRealtionListAsync(companyId, excludedUserId, paginationInfo);

			if (companyUserIds.Count == 0)
			{
				return [];
			}

			return await CompanyUserViewDtoListApiRequest(companyUserIds, bearerToken);
		}

		public async Task<int> GetCompanyUsersCountAsync(int companyId, Guid excludedUserId)
		{
			return await companyRepository.GetCompanyUsersCountAsync(companyId, excludedUserId);
		}

		public async Task<CompanyForEmployeeDto> GetCompanyForEmployeeDtoAsync(Guid employeeId)
		{
			return await companyRepository.GetCompanyForEmployeeDtoAsync(employeeId);
		}

		public async Task<bool> IsUserInCompanyAsync(Guid userId, int companyId)
		{
			return await companyRepository.IsUserInCompanyAsync(userId, companyId);
		}

		public Task<bool> IsUserCanManageApplicationsAsync(int companyUserId, Guid insUserId)
		{
			//TODO
			return Task.FromResult(true);
		}

		#region Private Methods
		private async Task<IEnumerable<CompanyUserViewDto>> CompanyUserViewDtoListApiRequest(IList<CompanyUserUserRelation> companyUserRelations, string bearerToken)
		{
			var client = httpClientFactory.CreateClient(HttpClientNameHelper.Auth);
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

			var jsonRequestDto = JsonSerializer.Serialize(companyUserRelations.Select(x => x.UserId));
			var stringContent = new StringContent(jsonRequestDto, System.Text.Encoding.UTF8, "application/json");

			var response = await client.PostAsync(ApiLinkHelper.GetUserDataListViews, stringContent);

			var responseBody = await response.Content.ReadAsStringAsync();

			var companyUserViewDtos = JsonSerializer.Deserialize<IEnumerable<CompanyUserViewDto>>(responseBody, JsonOptionsHelper.DefaultJsonSerializerOptions) ?? [];

			foreach (var companyUserViewDto in companyUserViewDtos)
			{
				companyUserViewDto.CompanyUserId = companyUserRelations.Single(x => x.UserId.ToString() == companyUserViewDto.UserId).CompanyUserId;
			}

			return companyUserViewDtos.OrderBy(x => x.CompanyUserId);
		}
		#endregion
	}
}