using AxiteHr.Services.EmailAPI.Data;
using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Services.EmailAPI.Models;
using AxiteHR.Services.EmailAPI.Services.EmailSender;
using Microsoft.Extensions.Localization;
using System.Text;

namespace AxiteHR.Services.EmailAPI.Services.EmployeeTempPassword
{
	public class EmployeeTempPasswordService(
		IServiceScopeFactory serviceScopeFactory,
		IStringLocalizer<EmailResources> emailLocalizer,
		ILogger<EmployeeTempPasswordService> logger,
		IEmailSender emailSender) : IEmployeeTempPasswordService
	{
		public async Task EmailTempPasswordCreateAndLog(UserTempPasswordMessageBusDto messageBusDto)
		{
			StringBuilder message = new();

			message.AppendLine("<br/>");
			message.Append(emailLocalizer[EmailResourcesKeys.Email_Welcome]);
			message.Append(" " + messageBusDto.FirstName + ",");

			message.AppendLine("<br/>");
			message.Append(emailLocalizer[EmailResourcesKeys.Email_ThanksForChoosingAxiteHr]);

			message.AppendLine("<br/>");
			message.Append(emailLocalizer[EmailResourcesKeys.Email_TempPasswordBeforeMessage]);
			message.Append(" " + messageBusDto.TempPassword);

			var messageString = message.ToString();

			await emailSender.SendHtmlEmailAsync(messageBusDto.Email, emailLocalizer[EmailResourcesKeys.Subject_TempPassword], messageString);
			await LogEmail(messageString, messageBusDto.Email);
		}

		private async Task LogEmail(string message, string email)
		{
			try
			{
				using var scope = serviceScopeFactory.CreateScope();
				var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
				EmailLogger emailLog = new()
				{
					Email = email,
					EmailSent = DateTime.UtcNow,
					Message = message
				};
				await dbContext.AddAsync(emailLog);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error when creating logs from mail to db");
			}
		}
	}
}
