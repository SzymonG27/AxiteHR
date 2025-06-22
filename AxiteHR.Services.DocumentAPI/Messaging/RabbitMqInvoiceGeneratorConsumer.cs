using AxiteHR.Integration.BrokerMessageSender.Models;
using AxiteHR.Services.DocumentAPI.Helpers;
using AxiteHR.Services.DocumentAPI.Models.Invoice.Dto;
using AxiteHR.Services.DocumentAPI.Services.Invoice;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text.Json;

namespace AxiteHR.Services.DocumentAPI.Messaging
{
	public class RabbitMqInvoiceGeneratorConsumer(
		IInvoiceGeneratorService invoiceGeneratorService,
		IConfiguration configuration,
		IOptions<RabbitMqMessageSenderConfig> rabbitMqMessageSenderConfigOptions) : BackgroundService, IAsyncDisposable
	{
		private readonly RabbitMqMessageSenderConfig _rabbitMqConfig = rabbitMqMessageSenderConfigOptions.Value;

		private IConnection _connection = default!;
		private IChannel _channel = default!;
		private string _queueName = default!;
		private bool _disposed;
		private bool _initialized;

		public async Task InitializeAsync()
		{
			if (_initialized)
				return;

			if (_rabbitMqConfig == null)
			{
				Log.Error("RabbitMQ config section is not configured.");
				throw new InvalidOperationException("RabbitMQ config section is not configured.");
			}

			if (IsRabbitMqConfigCompletedCorrectly(_rabbitMqConfig))
			{
				Log.Error("RabbitMQ configuration values are not properly set.");
				throw new InvalidOperationException("RabbitMQ configuration values are not properly set.");
			}

			var factory = new ConnectionFactory
			{
				HostName = _rabbitMqConfig.HostName,
				UserName = _rabbitMqConfig.UserName,
				Password = _rabbitMqConfig.Password,
				Port = _rabbitMqConfig.Port
			};

			_connection = await factory.CreateConnectionAsync();
			_channel = await _connection.CreateChannelAsync();

			_queueName = configuration.GetValue<string>(ConfigurationHelper.InvoiceGeneratorQueue)!;

			await _channel.QueueDeclareAsync(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

			await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 5, global: false);

			_initialized = true;
		}

		public async ValueTask DisposeAsync()
		{
			if (_disposed) return;

			if (_channel is not null)
				await _channel.DisposeAsync();

			if (_connection is not null)
				await _connection.DisposeAsync();

			_disposed = true;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			await InitializeAsync();

			stoppingToken.ThrowIfCancellationRequested();

			var consumer = new AsyncEventingBasicConsumer(_channel);

			consumer.ReceivedAsync += async (_, ea) =>
			{
				var messageId = ea.BasicProperties?.MessageId ?? "unknown";

				try
				{
					var content = JsonSerializer.Deserialize<InvoiceGeneratorDto>(ea.Body.ToString());
					if (content == null)
					{
						Log.Warning("Message body was null, messageId: {MessageId}", messageId);
					}
					else
					{
						Log.Information("Received message with ID {MessageId}", messageId);
						await HandleMessageAsync(content);
					}

					await _channel.BasicAckAsync(ea.DeliveryTag, false);
				}
				catch (Exception ex)
				{
					Log.Error(ex, "Error processing message with ID {MessageId}", messageId);
					await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true);
				}
			};

			await _channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

			stoppingToken.Register(async () =>
			{
				await _channel.CloseAsync();
				await _connection.CloseAsync();
			});
		}

		private async Task HandleMessageAsync(InvoiceGeneratorDto model)
		{
			await invoiceGeneratorService.GenerateInvoiceAsync(model);
		}

		private static bool IsRabbitMqConfigCompletedCorrectly(RabbitMqMessageSenderConfig rabbitMqMessageSenderConfig)
		{
			return string.IsNullOrEmpty(rabbitMqMessageSenderConfig.HostName) ||
				string.IsNullOrEmpty(rabbitMqMessageSenderConfig.UserName) ||
				string.IsNullOrEmpty(rabbitMqMessageSenderConfig.Password) ||
				rabbitMqMessageSenderConfig.Port == 0;
		}
	}
}
