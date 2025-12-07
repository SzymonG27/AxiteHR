using System.ComponentModel.DataAnnotations;

namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels
{
	public class CompanyRole
	{
		public virtual int Id { get; set; }

		public virtual string RoleName { get; set; } = string.Empty;

		public virtual string RoleNameEng { get; set; } = string.Empty;
	}
}
