using System.ComponentModel.DataAnnotations;

namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels
{
	public class CompanyRole
	{
		[Key]
		public virtual int Id { get; set; }

		[MaxLength(100)]
		public virtual string RoleName { get; set; } = string.Empty;

		[MaxLength(100)]
		public virtual string RoleNameEng { get; set; } = string.Empty;
	}
}
