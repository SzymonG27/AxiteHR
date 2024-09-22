using System.ComponentModel.DataAnnotations;

namespace AxiteHR.Services.ApplicationAPI.Models.Application
{
	public class UserCompanyDaysOff
	{
		[Key]
		public virtual int Id { get; set; }

		public virtual Guid UserId { get; set; }

		public virtual int CompanyId { get; set; }

		public virtual bool IsVacationDaysUnlimited { get; set; }

		public virtual decimal? VacationDaysOff { get; set; }

		public virtual bool IsInaccessibilityDaysUnlimited { get; set; }

		public virtual decimal? InaccessibilityDaysOff { get; set; }

		public virtual Guid InsUserId { get; set; }

		public virtual DateTime InsDate { get; set; }

		public virtual Guid UpdUserId { get; set; }

		public virtual DateTime UpdDate { get; set; }
	}
}
