namespace AxiteHR.Services.EmailAPI.Models.SenderOptions
{
	public record MailSenderOptions
	{
		public string From { get; set; } = string.Empty;
		public string SmtpServer { get; set; } = string.Empty;
		public int Port { get; set; }
		public string UserName { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
	}
}
