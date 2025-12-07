namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels
{
	public class Company
	{
		public virtual int Id { get; set; }

		public virtual string CompanyName { get; set; } = string.Empty;

		public virtual int CompanyLevelId { get; set; }

		public virtual CompanyLevel CompanyLevel { get; set; } = new();

		public virtual Guid InsUserId { get; set; }

		public virtual DateTime InsDate { get; set; }

		public virtual Guid UpdUserId { get; set; }

		public virtual DateTime UpdDate { get; set; }
	}
}