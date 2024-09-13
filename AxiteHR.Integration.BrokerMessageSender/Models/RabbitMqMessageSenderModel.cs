namespace AxiteHR.Integration.BrokerMessageSender.Models
{
	public record RabbitMqMessageSenderModel
	{
		public string HostName { get; set; } = string.Empty;
		public string UserName { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public object Message { get; set; } = new();
		public string QueueName { get; set; } = string.Empty;
	}
}
