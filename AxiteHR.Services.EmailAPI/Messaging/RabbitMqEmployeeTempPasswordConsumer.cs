using AxiteHR.Integration.BrokerMessageSender.Models;
using AxiteHR.Services.EmailAPI.Helpers;
using AxiteHR.Services.EmailAPI.Models;
using AxiteHR.Services.EmailAPI.Services.EmployeeTempPassword;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace AxiteHR.Services.EmailAPI.Messaging
{
	public class RabbitMqEmployeeTempPasswordConsumer : BackgroundService
	{
		private readonly ILogger<RabbitMqEmployeeTempPasswordConsumer> _logger;
		private readonly IEmployeeTempPasswordService _employeeTempPasswordService;
		private readonly IConnection _connection;
		private readonly IModel _channel;
		private readonly string _queueName;
		private bool _disposed;

		public RabbitMqEmployeeTempPasswordConsumer(
			ILogger<RabbitMqEmployeeTempPasswordConsumer> logger,
			IConfiguration configuration,
			IEmployeeTempPasswordService employeeTempPasswordService,
			IOptions<RabbitMqMessageSenderConfig> rabbitMqMessageSenderConfigOptions)
		{
			_logger = logger;
			_employeeTempPasswordService = employeeTempPasswordService;
			var rabbitMqMessageSenderConfig = rabbitMqMessageSenderConfigOptions.Value;

			if (rabbitMqMessageSenderConfig == null)
			{
				_logger.LogError("RabbitMQ config section is not configured.");
				throw new InvalidOperationException("RabbitMQ config section is not configured.");
			}

			if (IsRabbitMqConfigCompletedCorrectly(rabbitMqMessageSenderConfig))
			{
				_logger.LogError("RabbitMQ configuration values are not properly set.");
				throw new InvalidOperationException("RabbitMQ configuration values are not properly set.");
			}

			var factory = new ConnectionFactory
			{
				HostName = rabbitMqMessageSenderConfig.HostName,
				UserName = rabbitMqMessageSenderConfig.UserName,
				Password = rabbitMqMessageSenderConfig.Password,
				Port = rabbitMqMessageSenderConfig.Port
			};

			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();

			_queueName = configuration.GetValue<string>(ConfigurationHelper.EmailTempPasswordQueue)!;

			_channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

			_channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			stoppingToken.ThrowIfCancellationRequested();

			var consumer = new EventingBasicConsumer(_channel);

			consumer.Received += (_, ea) =>
			{
				Task.Run(async () =>
				{
					var messageId = ea.BasicProperties?.MessageId ?? "unknown";
					try
					{
						var content = JsonSerializer.Deserialize<UserTempPasswordMessageBusDto>(ea.Body.ToArray());
						if (content == null)
						{
							_logger.LogWarning("Message body was null, messageId: {MessageId}", messageId);
						}
						else
						{
							_logger.LogInformation("Received message with ID {MessageId}", messageId);
							await HandleMessage(content);
						}

						_channel.BasicAck(ea.DeliveryTag, false);
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, "Error processing message with ID {MessageId}", messageId);
						_channel.BasicNack(ea.DeliveryTag, false, requeue: true);
					}
				}, stoppingToken);
			};

			_channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

			stoppingToken.Register(() =>
			{
				_channel.Close();
				_connection.Close();
			});

			return Task.CompletedTask;
		}

		protected virtual void DisposeConnection(bool disposing)
		{
			if (_disposed)
			{
				return;
			}

			if (disposing)
			{
				_channel.Dispose();
				_connection.Dispose();
			}

			_disposed = true;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "<Pending>")]
		public override void Dispose()
		{
			DisposeConnection(true);
			base.Dispose(); //base.Dispose() has SuppressFinalize inside
		}

		#region Private Methods
		private async Task HandleMessage(UserTempPasswordMessageBusDto model)
		{
			await _employeeTempPasswordService.EmailTempPasswordCreateAndLogAsync(model);
		}

		private static bool IsRabbitMqConfigCompletedCorrectly(RabbitMqMessageSenderConfig rabbitMqMessageSenderConfig)
		{
			return string.IsNullOrEmpty(rabbitMqMessageSenderConfig.HostName) ||
				string.IsNullOrEmpty(rabbitMqMessageSenderConfig.UserName) ||
				string.IsNullOrEmpty(rabbitMqMessageSenderConfig.Password) ||
				rabbitMqMessageSenderConfig.Port == 0;
		}
		#endregion
	}
}