using Azure.Messaging.ServiceBus;
using System.Text;
using System.Text.Json;

namespace AxiteHR.Integration.MessageBus
{
	public class MessageBus : IMessageBus
	{
		public async Task PublishMessage(object message, string connectionString, string topicOrQueueName)
		{
			await using var client = new ServiceBusClient(connectionString);

			var sender = client.CreateSender(topicOrQueueName);

			var jsonMessage = JsonSerializer.Serialize(message);
			var finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
			{
				CorrelationId = topicOrQueueName
			};

			await sender.SendMessageAsync(finalMessage);
		}
	}
}
