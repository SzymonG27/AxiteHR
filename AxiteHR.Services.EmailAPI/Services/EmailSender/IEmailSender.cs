namespace AxiteHR.Services.EmailAPI.Services.EmailSender
{
	public interface IEmailSender
	{
		Task SendHtmlEmailAsync(string emailTo, string subject, string body);
	}
}
