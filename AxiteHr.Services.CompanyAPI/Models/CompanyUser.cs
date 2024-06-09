using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AxiteHr.Services.CompanyAPI.Models
{
	public class CompanyUser
	{
		[Key]
		public virtual int Id { get; set; }

		public virtual int CompanyId { get; set; }

		[ForeignKey(nameof(CompanyId))]
		public virtual Company Company { get; set; } = new Company();

		public virtual Guid UserId { get; set; }

		public virtual Guid InsUserId { get; set; }
		
		public virtual DateTime InsDate { get; set; }
	}
}