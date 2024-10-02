using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AxiteHr.Services.CompanyAPI.Models.CompanyModels
{
	public class CompanyUser
	{
		[Key]
		public virtual int Id { get; set; }

		[ForeignKey(nameof(Company))]
		public virtual int CompanyId { get; set; }

		public virtual Company Company { get; set; } = new();

		public virtual Guid UserId { get; set; }

		public virtual Guid InsUserId { get; set; }

		public virtual DateTime InsDate { get; set; }
	}
}