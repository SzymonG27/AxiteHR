using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AxiteHr.Services.CompanyAPI.Models
{
	public class CompanyPermission
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public virtual int Id { get; set; }

		public virtual string PermissionName { get; set; } = string.Empty;
	}
}
