using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels
{
	public class CompanyRoleCompany
	{
		[Key]
		public virtual int Id { get; set; }

		[ForeignKey(nameof(Company))]
		public virtual int CompanyId { get; set; }

		public virtual Company Company { get; set; } = new();

		[ForeignKey(nameof(CompanyRole))]
		public virtual int CompanyRoleId { get; set; }

		public virtual CompanyRole CompanyRole { get; set; } = new();

		public virtual bool IsMain { get; set; }

		public virtual bool IsVisible { get; set; }
	}
}
