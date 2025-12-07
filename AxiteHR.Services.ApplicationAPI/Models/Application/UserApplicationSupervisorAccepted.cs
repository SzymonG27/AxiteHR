namespace AxiteHR.Services.ApplicationAPI.Models.Application
{
	public class UserApplicationSupervisorAccepted
	{
		public virtual int Id { get; set; }

		public virtual int UserApplicationId { get; set; }

		public virtual UserApplication UserApplication { get; set; } = new();

		/// <summary>
		/// UserId of supervisor who accepted application
		/// </summary>
		public virtual int SupervisorAcceptedId { get; set; }
	}
}
