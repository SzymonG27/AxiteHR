using AxiteHR.Services.EmailAPI.Helpers;
using AxiteHR.Services.EmailAPI.Models;
using Azure.Messaging.ServiceBus;
using System.Text;
using System.Text.Json;

namespace AxiteHR.Services.EmailAPI.Messaging
{
	public class AzureServiceBusConsumer : IAzureServiceBusConsumer
	{
		private readonly ILogger<AzureServiceBusConsumer> _logger;
		private readonly ServiceBusProcessor _emailTempPasswordProcessor;

		public AzureServiceBusConsumer(IConfiguration configuration, ILogger<AzureServiceBusConsumer> logger)
		{
			ArgumentNullException.ThrowIfNull(configuration);
			ArgumentNullException.ThrowIfNull(logger);

			_logger = logger;

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
			var body = Encoding.UTF8.GetString(args.Message.Body);
			var userMessageBusDto = JsonSerializer.Deserialize<UserMessageBusDto>(body);

			try
			{
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