using AxiteHR.Services.ApplicationAPI.Models.Application.Enums;

namespace AxiteHR.Services.ApplicationAPI.Models.Application
{
	public class UserCompanyDaysOff
	{
		public virtual int Id { get; set; }

		public virtual int CompanyUserId { get; set; }

		public virtual ApplicationType ApplicationType { get; set; }

		public virtual decimal? DaysOff { get; set; }

		public virtual Guid InsUserId { get; set; }

		public virtual DateTime InsDate { get; set; }

		public virtual Guid UpdUserId { get; set; }

		public virtual DateTime UpdDate { get; set; }
	}
}
