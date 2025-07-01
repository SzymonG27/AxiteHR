namespace AxiteHR.Services.EmailAPI.Models
{
	public class EmailLogger
	{
		public virtual int Id { get; set; }
		public virtual string Email { get; set; } = string.Empty;
		public virtual string Message { get; set; } = string.Empty;
		public virtual DateTime? EmailSent { get; set; }
	}
}
