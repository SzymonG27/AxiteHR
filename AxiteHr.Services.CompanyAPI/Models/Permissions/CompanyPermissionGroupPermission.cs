namespace AxiteHR.Services.CompanyAPI.Models.Permissions
{
	public class CompanyPermissionGroupPermission
	{
		public virtual int Id { get; set; }
		public virtual int CompanyPermissionGroupId { get; set; }
		public virtual CompanyPermissionGroup CompanyPermissionGroup { get; set; } = new();
		public virtual int CompanyPermissionId { get; set; }
		public virtual CompanyPermission CompanyPermission { get; set; } = new();
		public virtual Guid InsUserId { get; set; }
		public virtual DateTime InsDate { get; set; }
	}
}
