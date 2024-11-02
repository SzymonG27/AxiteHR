using AxiteHR.Services.EmailAPI.Data;
using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Security.Encryption;
using AxiteHR.Services.EmailAPI.Helpers;
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
		IEmailSender emailSender,
		IConfiguration configuration,
		IEncryptionService encryptionService) : IEmployeeTempPasswordService
	{
		public async Task EmailTempPasswordCreateAndLogAsync(UserTempPasswordMessageBusDto messageBusDto)
		{
			var decryptedPassword = DecryptTempPassword(messageBusDto.TempPassword);

			StringBuilder message = new();

			message.AppendLine("<br/>");
			message.Append(emailLocalizer[EmailResourcesKeys.Email_Welcome]);
			message.Append(" " + messageBusDto.FirstName + ",");

			message.AppendLine("<br/>");
			message.Append(emailLocalizer[EmailResourcesKeys.Email_ThanksForChoosingAxiteHr]);

			message.AppendLine("<br/>");
			message.Append(emailLocalizer[EmailResourcesKeys.Email_TempPasswordBeforeMessage]);
			message.Append(" " + decryptedPassword);

			var messageString = message.ToString();

			await emailSender.SendHtmlEmailAsync(messageBusDto.Email, emailLocalizer[EmailResourcesKeys.Subject_TempPassword], messageString);
			await LogEmail(messageString, messageBusDto.Email);
		}

		#region Private methods
		private string DecryptTempPassword(string tempPassword)
		{
			var encryptionKey = configuration.GetValue<string>(ConfigurationHelper.TempPasswordEncryptionKey) ??
				throw new ArgumentException($"Appsetting {ConfigurationHelper.TempPasswordEncryptionKey} isn't configured");

			return encryptionService.Decrypt(tempPassword, encryptionKey);
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
		#endregion
	}
}
