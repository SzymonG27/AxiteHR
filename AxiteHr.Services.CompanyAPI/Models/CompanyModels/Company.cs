using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AxiteHr.Services.CompanyAPI.Models.CompanyModels
{
	public class Company
	{
		[Key]
		public virtual int Id { get; set; }

		public virtual string CompanyName { get; set; } = string.Empty;

		public virtual int CompanyLevelId { get; set; }

		[ForeignKey(nameof(CompanyLevelId))]
		public virtual CompanyLevel CompanyLevel { get; set; } = new CompanyLevel();

		public virtual Guid InsUserId { get; set; }

		public virtual DateTime InsDate { get; set; }

		public virtual Guid UpdUserId { get; set; }

		public virtual DateTime UpdDate { get; set; }
	}
}