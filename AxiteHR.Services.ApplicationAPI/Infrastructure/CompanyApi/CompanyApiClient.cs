using AxiteHR.Services.ApplicationAPI.Extensions;
using AxiteHR.Services.ApplicationAPI.Helpers;

namespace AxiteHR.Services.ApplicationAPI.Infrastructure.CompanyApi
{
	public class CompanyApiClient(IHttpClientFactory httpClientFactory) : ICompanyApiClient
	{
		public async Task<int?> GetCompanyUserIdAsync(int companyId, Guid userId, string bearerToken, string acceptLanguage)
		{
			var client = httpClientFactory.CreateClient(HttpClientNameHelper.Company);
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
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

		public async Task<bool> IsUserCanManageApplicationForCompanyUserAsync(string token, string acceptLanguage, int companyUserId, Guid insUserId)
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
	}
}
