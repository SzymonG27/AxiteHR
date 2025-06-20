namespace AxiteHR.Services.DocumentAPI.Helpers
{
	public static class ConfigurationHelper
	{
		public const string DefaultConnectionString = "DefaultConnection";
		public const string InvoiceGeneratorQueue = "TopicsAndQueueNames:InvoiceGeneratorQueue";
		public const string IsDbFromDocker = "IsDbFromDocker";
		public const string LogStashUrl = "LogstashConfig:LogStashUrl";
		public const string LogStashQueueLimitBytes = "LogstashConfig:QueueLimitBytes";
		public const string RabbitMqBrokerMessageSenderConfig = "BrokerMessageSenderConfigs:RabbitMq";
	}
}
