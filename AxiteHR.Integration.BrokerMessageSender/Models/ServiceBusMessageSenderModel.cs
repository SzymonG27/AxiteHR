namespace AxiteHR.Integration.BrokerMessageSender.Models
{
	public record ServiceBusMessageSenderModel
	{
		public object Message { get; set; } = string.Empty;
		public string ConnectionString { get; set; } = string.Empty;
		public string TopicOrQueueName { get; set; } = string.Empty;
	}
}
