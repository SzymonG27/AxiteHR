using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AxiteHR.Services.CompanyAPI.Models.CompanyModels
{
	public class Company
	{
		[Key]
		public virtual int Id { get; set; }

		[MaxLength(100)]
		public virtual string CompanyName { get; set; } = string.Empty;

		[ForeignKey(nameof(CompanyLevel))]
		public virtual int CompanyLevelId { get; set; }

		public virtual CompanyLevel CompanyLevel { get; set; } = new();

		public virtual Guid InsUserId { get; set; }

		public virtual DateTime InsDate { get; set; }

		public virtual Guid UpdUserId { get; set; }

		public virtual DateTime UpdDate { get; set; }
	}
}