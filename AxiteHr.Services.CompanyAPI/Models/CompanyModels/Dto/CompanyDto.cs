namespace AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto
{
	public record CompanyDto
	{
		public int Id { get; set; }

		public string CompanyName { get; set; } = string.Empty;

		public int CompanyLevelId { get; set; }

		public CompanyLevelDto CompanyLevel { get; set; } = new CompanyLevelDto();
	}
}
