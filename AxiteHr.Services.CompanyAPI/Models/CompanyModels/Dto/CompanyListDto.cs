namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto
{
	public record CompanyListDto
	{
		public int Id { get; set; }
		public string CompanyName { get; set; } = string.Empty;
		public string InsDate { get; set; } = string.Empty;
		public int UserCount { get; set; }
	}
}