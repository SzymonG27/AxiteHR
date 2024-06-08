using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AxiteHr.Services.CompanyAPI.Models
{
	public class CompanyUserPermission
	{
		[Key]
		public virtual int Id { get; set; }

		public virtual int CompanyUserId { get; set; }

		[ForeignKey(nameof(CompanyUserId))]
		public virtual CompanyUser CompanyUser { get; set; } = new CompanyUser();

		public virtual int CompanyPermissionId { get; set; }

		public virtual CompanyPermission CompanyPermission { get; set; } = new CompanyPermission();
	}
}
