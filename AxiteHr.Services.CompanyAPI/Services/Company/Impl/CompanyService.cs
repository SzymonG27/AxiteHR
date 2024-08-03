using AxiteHr.Services.CompanyAPI.Data;
using AxiteHr.Services.CompanyAPI.Helpers;
using AxiteHr.Services.CompanyAPI.Infrastructure;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Const;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto;
using AxiteHr.Services.CompanyAPI.Models.EmployeeModels.Dto;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AxiteHr.Services.CompanyAPI.Services.Company.Impl
{
	public class CompanyService(
		AppDbContext dbContext,
		IHttpClientFactory httpClientFactory) : ICompanyService
	{
		public IEnumerable<CompanyListDto> GetCompanyList(Guid userId)
		{
			var companyUsersCount = dbContext.CompanyUsers
				.GroupBy(cu => cu.CompanyId)
				.AsNoTracking()
				.Select(g => new
				{
					CompanyId = g.Key,
					UserCount = g.Count()
				});

			return [.. dbContext.CompanyUsers
				.Join(dbContext.CompanyUserPermissions,
					cu => cu.Id,
					cup => cup.CompanyUserId,
					(cu, cup) => new { cu, cup })
				.Where(x => x.cu.UserId == userId && x.cup.CompanyPermissionId == (int)PermissionDictionary.CompanyManager)
				.Join(dbContext.Companies,
					previous => previous.cu.CompanyId,
					c => c.Id,
					(previous, c) => new { previous.cu, previous.cup, c })
				.Join(companyUsersCount,
					previous => previous.cu.CompanyId,
					count => count.CompanyId,
					(previous, count) => new
					{
						CompanyId = previous.c.Id,
						previous.c.CompanyName,
						previous.c.InsDate,
						count.UserCount
					})
				.AsNoTracking()
				.Select(x => new CompanyListDto
				{
					Id = x.CompanyId,
					CompanyName = x.CompanyName,
					InsDate = x.InsDate.ToShortDateString(),
					UserCount = x.UserCount
				})];
		}

		public async Task<IEnumerable<CompanyUserViewDto>> GetCompanyUserViewDtoListAsync(int companyId, Guid excludedUserId, Pagination paginationInfo, string bearerToken)
		{
			if (paginationInfo.ItemsPerPage <= 0)
			{
				paginationInfo.ItemsPerPage = 10;
			}

			var companyUserIds = await dbContext.CompanyUsers
				.Where(x => x.CompanyId == companyId && x.UserId != excludedUserId)
				.OrderBy(x => x.Id)
				.Skip(paginationInfo.Page * paginationInfo.ItemsPerPage)
				.Take(paginationInfo.ItemsPerPage)
				.AsNoTracking()
				.Select(x => new CompanyUserUserRelation { CompanyUserId = x.Id, UserId = x.UserId })
				.ToListAsync();

			if (companyUserIds.Count == 0)
			{
				return [];
			}

			return await CompanyUserViewDtoListApiRequest(companyUserIds, bearerToken);
		}

		public async Task<int> GetCompanyUsersCountAsync(int companyId, Guid excludedUserId)
		{
			return await dbContext.CompanyUsers
				.Where(x => x.CompanyId == companyId && x.UserId != excludedUserId)
				.AsNoTracking()
				.CountAsync();
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