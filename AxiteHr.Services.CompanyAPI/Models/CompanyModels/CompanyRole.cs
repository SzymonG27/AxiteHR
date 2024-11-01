using System.ComponentModel.DataAnnotations;

namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels
{
	public class CompanyRole
	{
		[Key]
		public virtual int Id { get; set; }

		[MaxLength(100)]
		public virtual string RoleName { get; set; } = string.Empty;

		public virtual bool IsMain { get; set; }

		public virtual bool IsVisible { get; set; }
	}
}
