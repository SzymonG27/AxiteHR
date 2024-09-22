using AxiteHR.Services.ApplicationAPI.Models.Application.Enums;
using System.ComponentModel.DataAnnotations;

namespace AxiteHR.Services.ApplicationAPI.Models.Application
{
	public class UserApplication
	{
		[Key]
		public virtual int Id { get; set; }

		public virtual Guid UserId { get; set; }

		public virtual ApplicationType ApplicationType { get; set; }

		public virtual ApplicationStatus ApplicationStatus { get; set; }

		public virtual Guid InsUserId { get; set; }

		public virtual DateTime InsDate { get; set; }

		public virtual Guid UpdUserId { get; set; }

		public virtual DateTime UpdDate { get; set; }
	}
}
