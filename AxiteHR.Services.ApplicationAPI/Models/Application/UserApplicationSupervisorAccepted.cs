using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AxiteHR.Services.ApplicationAPI.Models.Application
{
	public class UserApplicationSupervisorAccepted
	{
		[Key]
		public virtual int Id { get; set; }

		[ForeignKey(nameof(UserApplication))]
		public virtual int UserApplicationId { get; set; }

		public virtual UserApplication UserApplication { get; set; } = new();

		/// <summary>
		/// UserId of supervisor who accepted application
		/// </summary>
		public virtual int SupervisorAcceptedId { get; set; }
	}
}
