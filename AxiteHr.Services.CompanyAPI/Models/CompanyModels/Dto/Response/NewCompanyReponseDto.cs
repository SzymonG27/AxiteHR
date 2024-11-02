namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Response
{
	public record NewCompanyReponseDto
	{
		public bool IsSucceeded { get; set; }

		public string ErrorMessage { get; set; } = string.Empty;
	}
}
