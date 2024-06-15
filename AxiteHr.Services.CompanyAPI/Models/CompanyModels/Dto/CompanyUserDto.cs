namespace AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto
{
	public record CompanyUserDto
	{
		public int Id { get; set; }

		public int CompanyId { get; set; }

		public CompanyDto Company { get; set; } = new CompanyDto();

		public Guid UserId { get; set; }
	}
}
