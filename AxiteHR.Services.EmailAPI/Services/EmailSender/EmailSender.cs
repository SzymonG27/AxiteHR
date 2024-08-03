using AxiteHR.Services.EmailAPI.Models.SenderOptions;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace AxiteHR.Services.EmailAPI.Services.EmailSender
{
	public class EmailSender(
		ILogger<EmailSender> logger,
		IOptions<MailSenderOptions> mailSenderOptions) : IEmailSender
	{
		private readonly MailSenderOptions _mailSenderOptions = mailSenderOptions.Value;

		public async Task SendHtmlEmailAsync(string emailTo, string subject, string body)
		{
			try
			{
				var emailToSend = new MimeMessage();
				emailToSend.From.Add(MailboxAddress.Parse(_mailSenderOptions.From));
				emailToSend.To.Add(MailboxAddress.Parse(emailTo));
				emailToSend.Subject = subject;
				emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html)
				{
					Text = body
				};

				using var emailClient = new SmtpClient();
				await emailClient.ConnectAsync(_mailSenderOptions.SmtpServer, _mailSenderOptions.Port, MailKit.Security.SecureSocketOptions.StartTls);
				await emailClient.AuthenticateAsync(_mailSenderOptions.UserName, _mailSenderOptions.Password);

				await emailClient.SendAsync(emailToSend);

				await emailClient.DisconnectAsync(true);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error during send html mail");
			}
		}
	}
}
