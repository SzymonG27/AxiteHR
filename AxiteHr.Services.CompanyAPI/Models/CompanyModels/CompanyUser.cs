namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels
{
	public class CompanyUser
	{
		public virtual int Id { get; set; }

		public virtual int CompanyId { get; set; }

		public virtual Company Company { get; set; } = new();

		public virtual Guid UserId { get; set; }

		public virtual Guid InsUserId { get; set; }

		public virtual DateTime InsDate { get; set; }
	}
}