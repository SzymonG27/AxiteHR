using System.Text.Json;
using System.Text;
using RabbitMQ.Client;
using AxiteHR.Integration.BrokerMessageSender.Models;
using Serilog;

namespace AxiteHR.Integration.BrokerMessageSender.Senders
{
	public class RabbitMqMessageSender : IBrokerMessageSender<RabbitMqMessageSenderConfig>
	{
		private IConnection? connection;

		public Task PublishMessageAsync(MessageSenderModel<RabbitMqMessageSenderConfig> rabbitMqModel)
		{
			CreateConnectionIfNotExists(rabbitMqModel.Config);

			using var channel = connection!.CreateModel();
			channel.QueueDeclare(rabbitMqModel.Config.QueueName, false, false, false, null);
			var json = JsonSerializer.Serialize(rabbitMqModel.Message);
			var body = Encoding.UTF8.GetBytes(json);
			channel.BasicPublish("", rabbitMqModel.Config.QueueName, null, body);

			return Task.CompletedTask;
		}


		private void CreateConnectionIfNotExists(RabbitMqMessageSenderConfig rabbitMqConfig)
		{
			try
			{
				if (connection?.IsOpen == true)
				{
					return;
				}

				var factory = new ConnectionFactory
				{
					HostName = rabbitMqConfig.HostName,
					UserName = rabbitMqConfig.UserName,
					Password = rabbitMqConfig.Password,
					Port = rabbitMqConfig.Port
				};

				connection = factory.CreateConnection();
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Error while create connection to RabbitMQ");
			}
		}
	}
}
