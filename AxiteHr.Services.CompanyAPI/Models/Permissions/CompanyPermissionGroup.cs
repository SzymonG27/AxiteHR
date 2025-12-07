using AxiteHR.Services.CompanyAPI.Models.CompanyModels;

namespace AxiteHR.Services.CompanyAPI.Models.Permissions
{
	public class CompanyPermissionGroup
	{
		public virtual int Id { get; set; }
		public virtual int CompanyId { get; set; }
		public virtual Company Company { get; set; } = new();
		public virtual string Name { get; set; } = string.Empty;
		public virtual string? Description { get; set; }
		public virtual bool IsActive { get; set; } = true;
		public virtual Guid InsUserId { get; set; }
		public virtual DateTime InsDate { get; set; }
		public virtual Guid UpdUserId { get; set; }
		public virtual DateTime UpdDate { get; set; }

		public ICollection<CompanyUserPermission> UserPermissions { get; set; } = [];

		public virtual ICollection<CompanyPermissionGroupPermission> Permissions { get; set; } = [];
	}
}
