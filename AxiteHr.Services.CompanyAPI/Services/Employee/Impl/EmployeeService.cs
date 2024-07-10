using AxiteHr.Services.CompanyAPI.Helpers;
using AxiteHr.Services.CompanyAPI.Models.EmployeeModels.Dto;
using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace AxiteHr.Services.CompanyAPI.Services.Employee.Impl
{
	public class EmployeeService(
		IHttpClientFactory httpClientFactory,
		IStringLocalizer<CompanyResources> companyLocalizer) : IEmployeeService
	{
		public async Task<NewEmployeeResponseDto> CreateNewEmployee(NewEmployeeRequestDto requestDto)
		{
			var client = httpClientFactory.CreateClient(HttpClientNameHelper.Auth);
			var jsonRequestDto = JsonConvert.SerializeObject(requestDto);
			var stringContent = new StringContent(jsonRequestDto, System.Text.Encoding.UTF8, "application/json");
			var response = await client.PostAsync(ApiLinkHelper.RegisterNewEmployee, stringContent);

			var responseBody = await response.Content.ReadAsStringAsync();

			return
				JsonConvert.DeserializeObject<NewEmployeeResponseDto>(responseBody) ??
				new NewEmployeeResponseDto
				{
					IsSucceeded = false,
					ErrorMessage = companyLocalizer[CompanyResourcesKeys.NewEmployeeRequestDto_RegisterError]
				};
		}
	}
}
