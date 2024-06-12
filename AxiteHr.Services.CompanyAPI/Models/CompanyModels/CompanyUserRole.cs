using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AxiteHr.Services.CompanyAPI.Models.CompanyModels
{
	public class CompanyUserRole
	{
		[Key]
		public virtual int Id { get; set; }

		public virtual int CompanyUserId { get; set; }

		[ForeignKey(nameof(CompanyUserId))]
		public virtual CompanyUser CompanyUser { get; set; } = new CompanyUser();

		public virtual int CompanyRoleId { get; set; }

		[ForeignKey(nameof(CompanyRoleId))]
		public virtual CompanyRole CompanyRole { get; set; } = new CompanyRole();

		public virtual Guid InsUserId { get; set; }

		public virtual DateTime InsDate { get; set; }
	}
}
