namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels
{
	public class CompanyUserPermission
	{
		public virtual int Id { get; set; }

		public virtual int CompanyUserId { get; set; }

		public virtual CompanyUser CompanyUser { get; set; } = new();

		public virtual int CompanyPermissionId { get; set; }

		public virtual CompanyPermission CompanyPermission { get; set; } = new();

		public virtual Guid InsUserId { get; set; }

		public virtual DateTime InsDate { get; set; }
	}
}
