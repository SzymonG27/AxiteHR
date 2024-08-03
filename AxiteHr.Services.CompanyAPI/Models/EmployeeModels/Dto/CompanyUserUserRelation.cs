namespace AxiteHr.Services.CompanyAPI.Models.EmployeeModels.Dto
{
	public record CompanyUserUserRelation
	{
		public int CompanyUserId { get; set; }
		public Guid UserId { get; set; }
	}
}
