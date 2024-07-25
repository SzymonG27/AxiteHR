using AxiteHR.Services.EmailAPI.Helpers;
using AxiteHR.Services.EmailAPI.Models;
using AxiteHR.Services.EmailAPI.Services.EmployeeTempPassword;
using Azure.Messaging.ServiceBus;
using System.Text;
using System.Text.Json;

namespace AxiteHR.Services.EmailAPI.Messaging
{
	public class AzureServiceBusConsumer : IAzureServiceBusConsumer
	{
		private readonly ILogger<AzureServiceBusConsumer> _logger;
		private readonly ServiceBusProcessor _emailTempPasswordProcessor;
		private readonly IEmployeeTempPasswordService _employeeTempPasswordService;

		public AzureServiceBusConsumer(
			IConfiguration configuration,
			ILogger<AzureServiceBusConsumer> logger,
			IEmployeeTempPasswordService employeeTempPasswordService)
		{
			ArgumentNullException.ThrowIfNull(configuration);
			ArgumentNullException.ThrowIfNull(logger);
			ArgumentNullException.ThrowIfNull(employeeTempPasswordService);

			_logger = logger;
			_employeeTempPasswordService = employeeTempPasswordService;

			var serviceBusConnectionString = configuration.GetConnectionString(ConfigurationHelper.ServiceBusConnectionString);
			var emailTempPasswordQueue = configuration.GetSection(ConfigurationHelper.EmailTempPasswordQueue).Value;

			if (string.IsNullOrEmpty(serviceBusConnectionString))
			{
				_logger.LogError("Service bus connection string is not configured.");
				throw new InvalidOperationException("Service bus connection string is not configured.");
			}

			if (string.IsNullOrEmpty(emailTempPasswordQueue))
			{
				_logger.LogError("Email temporary password queue name is not configured.");
				throw new InvalidOperationException("Email temporary password queue name is not configured.");
			}

			var client = new ServiceBusClient(serviceBusConnectionString);
			_emailTempPasswordProcessor = client.CreateProcessor(emailTempPasswordQueue);
		}

		public async Task Start()
		{
			_emailTempPasswordProcessor.ProcessMessageAsync += OnEmailTempPasswordRequestReceived;
			_emailTempPasswordProcessor.ProcessErrorAsync += OnEmailTempPasswordErrorHandler;
			await _emailTempPasswordProcessor.StartProcessingAsync();
		}

		public async Task Stop()
		{
			await _emailTempPasswordProcessor.StopProcessingAsync();
			await _emailTempPasswordProcessor.DisposeAsync();
		}

		#region Private Methods
		private async Task OnEmailTempPasswordRequestReceived(ProcessMessageEventArgs args)
		{
			try
			{
				var body = Encoding.UTF8.GetString(args.Message.Body);
				var userMessageBusDto = JsonSerializer.Deserialize<UserTempPasswordMessageBusDto>(body);

				if (userMessageBusDto == null)
				{
					_logger.LogError("Email wasn't send. UserTempPasswordMessageBusDto was null. Identifier: {Identifier}", args.Identifier);
					return;
				}

				await _employeeTempPasswordService.EmailTempPasswordCreateAndLog(userMessageBusDto);

				await args.CompleteMessageAsync(args.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error while trying to process e-mail");
			}
		}

		private Task OnEmailTempPasswordErrorHandler(ProcessErrorEventArgs args)
		{
			_logger.LogError(args.Exception, "Error while trying to send e-mail");
			return Task.CompletedTask;
		}
		#endregion
	}
}