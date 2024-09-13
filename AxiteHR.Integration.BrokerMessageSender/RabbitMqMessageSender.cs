using System.Text.Json;
using System.Text;
using RabbitMQ.Client;
using AxiteHR.Integration.BrokerMessageSender.Models;

namespace AxiteHR.Integration.BrokerMessageSender
{
	public class RabbitMqMessageSender(IConnection connection) : IBrokerMessageSender
	{
		public void PublishMessage(RabbitMqMessageSenderModel rabbitMqModel)
		{
			CreateConnectionIfNotExists(rabbitMqModel);

			using var channel = connection.CreateModel();
			channel.QueueDeclare(rabbitMqModel.QueueName, false, false, false, null);
			var json = JsonSerializer.Serialize(rabbitMqModel.Message);
			var body = Encoding.UTF8.GetBytes(json);
			channel.BasicPublish("", rabbitMqModel.QueueName, null, body);
		}


		private void CreateConnectionIfNotExists(RabbitMqMessageSenderModel rabbitMqConfig)
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
					Password = rabbitMqConfig.Password
				};

				connection = factory.CreateConnection();
			}
			catch (Exception ex)
			{

			}
		}
	}
}
