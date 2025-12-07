using AxiteHR.Services.CompanyAPI.Models.CompanyModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace AxiteHR.Services.CompanyAPI.Models.Roles
{
	public class CompanyUserRole
	{
		public virtual int Id { get; set; }

		public virtual int CompanyUserId { get; set; }

		public virtual CompanyUser CompanyUser { get; set; } = new();

		[ForeignKey(nameof(CompanyRoleCompany))]
		public virtual int CompanyRoleCompanyId { get; set; }

		public virtual CompanyRoleCompany CompanyRoleCompany { get; set; } = new();

		public virtual bool IsSupervisor { get; set; }

		public virtual Guid InsUserId { get; set; }

		public virtual DateTime InsDate { get; set; }
	}
}
