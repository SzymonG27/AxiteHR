using System.Text.Json;
using System.Text;
using AxiteHR.Integration.BrokerMessageSender.Models;
using Azure.Messaging.ServiceBus;

namespace AxiteHR.Integration.BrokerMessageSender.Senders
{
	public class ServiceBusMessageSender : IBrokerMessageSender<ServiceBusMessageSenderConfig>
	{
		public async Task PublishMessageAsync(MessageSenderModel<ServiceBusMessageSenderConfig> serviceBusSenderModel)
		{
			await using var client = new ServiceBusClient(serviceBusSenderModel.Config.ConnectionString);

			var sender = client.CreateSender(serviceBusSenderModel.Config.TopicOrQueueName);

			var jsonMessage = JsonSerializer.Serialize(serviceBusSenderModel.Message);
			var finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
			{
				CorrelationId = serviceBusSenderModel.Config.TopicOrQueueName
			};

			await sender.SendMessageAsync(finalMessage);
		}
	}
}
