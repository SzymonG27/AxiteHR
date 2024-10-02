using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AxiteHr.Services.CompanyAPI.Models.CompanyModels
{
	public class CompanyUserPermission
	{
		[Key]
		public virtual int Id { get; set; }

		[ForeignKey(nameof(CompanyUser))]
		public virtual int CompanyUserId { get; set; }

		public virtual CompanyUser CompanyUser { get; set; } = new();

		[ForeignKey(nameof(CompanyPermission))]
		public virtual int CompanyPermissionId { get; set; }

		public virtual CompanyPermission CompanyPermission { get; set; } = new();

		public virtual Guid InsUserId { get; set; }

		public virtual DateTime InsDate { get; set; }
	}
}
