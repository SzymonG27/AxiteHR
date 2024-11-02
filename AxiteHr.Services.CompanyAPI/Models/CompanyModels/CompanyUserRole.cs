using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels
{
	public class CompanyUserRole
	{
		[Key]
		public virtual int Id { get; set; }

		[ForeignKey(nameof(CompanyUser))]
		public virtual int CompanyUserId { get; set; }

		public virtual CompanyUser CompanyUser { get; set; } = new();

		[ForeignKey(nameof(CompanyRole))]
		public virtual int CompanyRoleId { get; set; }

		public virtual CompanyRole CompanyRole { get; set; } = new();

		public virtual bool IsSupervisor { get; set; }

		public virtual Guid InsUserId { get; set; }

		public virtual DateTime InsDate { get; set; }
	}
}
