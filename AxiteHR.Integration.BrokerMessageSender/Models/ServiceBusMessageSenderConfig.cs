namespace AxiteHR.Integration.BrokerMessageSender.Models
{
	public record ServiceBusMessageSenderConfig : IBrokerConfig
	{
		public string ConnectionString { get; set; } = string.Empty;
		public string TopicOrQueueName { get; set; } = string.Empty;
	}
}
