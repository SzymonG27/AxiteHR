namespace AxiteHR.Services.AuthAPI.Helpers
{
	public static class ConfigurationHelper
	{
		public const string JwtOptions = "ApiSettings:JwtOptions";
		public const string DefaultConnectionString = "DefaultConnection";
		public const string MessageBusConnectionString = "MessageBusSettings:ConnectionString";
		public const string EmailTempPasswordQueue = "TopicsAndQueueNames:EmailTempPasswordQueue";
		public const string IsDbFromDocker = "IsDbFromDocker";
		public const string LogStashUrl = "LogstashConfig:LogStashUrl";
		public const string LogStashQueueLimitBytes = "LogstashConfig:QueueLimitBytes";
		public const string TempPasswordEncryptionKey = "TempPasswordEncryptionKey";
		public const string RabbitMqBrokerMessageSenderConfig = "BrokerMessageSenderConfigs:RabbitMq";
	}
}
