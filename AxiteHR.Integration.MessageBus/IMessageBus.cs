namespace AxiteHR.Integration.MessageBus
{
	public interface IMessageBus
	{
		Task PublishMessage(object message, string connectionString, string topicOrQueueName);
	}
}
