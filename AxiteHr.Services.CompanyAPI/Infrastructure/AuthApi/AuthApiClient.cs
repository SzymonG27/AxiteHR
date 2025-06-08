using AxiteHR.Services.CompanyAPI.Helpers;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto;
using AxiteHR.Services.CompanyAPI.Models.EmployeeModels.Dto;
using System.Text.Json;

namespace AxiteHR.Services.CompanyAPI.Infrastructure.AuthApi
{
	public class AuthApiClient(IHttpClientFactory httpClientFactory) : IAuthApiClient
	{
		public async Task<IEnumerable<CompanyUserDataDto>> GetUserDataListDtoAsync(IList<CompanyUserUserRelation> companyUserRelations, string bearerToken)
		{
			var client = httpClientFactory.CreateClient(HttpClientNameHelper.Auth);
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

			var jsonRequestDto = JsonSerializer.Serialize(companyUserRelations.Select(x => x.UserId));
			var stringContent = new StringContent(jsonRequestDto, System.Text.Encoding.UTF8, "application/json");

			var response = await client.PostAsync(ApiLinkHelper.GetUserDataListViews, stringContent);

			var responseBody = await response.Content.ReadAsStringAsync();

			var companyUserViewDtos = JsonSerializer.Deserialize<IEnumerable<CompanyUserDataDto>>(responseBody, JsonOptionsHelper.DefaultJsonSerializerOptions) ?? [];

			foreach (var companyUserViewDto in companyUserViewDtos)
			{
				companyUserViewDto.CompanyUserId = companyUserRelations.Single(x => x.UserId.ToString() == companyUserViewDto.UserId).CompanyUserId;
			}

			return companyUserViewDtos.OrderBy(x => x.CompanyUserId);
		}
	}
}
