using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AxiteHr.Services.CompanyAPI.Models.CompanyModels
{
	public class CompanyPermission
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public virtual int Id { get; set; }

		[MaxLength(100)]
		public virtual string PermissionName { get; set; } = string.Empty;
	}
}
