using System.Text.Json;
using System.Text;
using AxiteHR.Integration.BrokerMessageSender.Models;
using Azure.Messaging.ServiceBus;

namespace AxiteHR.Integration.BrokerMessageSender
{
	public class ServiceBusMessageSender : IBrokerMessageSender
	{
		public async Task PublishMessage(ServiceBusMessageSenderModel serviceBusSenderModel)
		{
			await using var client = new ServiceBusClient(serviceBusSenderModel.ConnectionString);

			var sender = client.CreateSender(serviceBusSenderModel.TopicOrQueueName);

			var jsonMessage = JsonSerializer.Serialize(serviceBusSenderModel.Message);
			var finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
			{
				CorrelationId = serviceBusSenderModel.TopicOrQueueName
			};

			await sender.SendMessageAsync(finalMessage);
		}
	}
}
