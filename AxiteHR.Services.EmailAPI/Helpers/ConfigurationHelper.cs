namespace AxiteHR.Services.EmailAPI.Helpers
{
	public static class ConfigurationHelper
	{
		public const string EmailSenderSettings = "EmailSenderSettings";
		public const string DefaultConnectionString = "DefaultConnection";
		public const string ServiceBusConnectionString = "ServiceBusConnectionString";
		public const string EmailTempPasswordQueue = "TopicsAndQueueNames:EmailTempPasswordQueue";
		public const string IsDbFromDocker = "IsDbFromDocker";
		public const string LogStashUrl = "LogstashConfig:LogStashUrl";
		public const string LogStashQueueLimitBytes = "LogstashConfig:QueueLimitBytes";
		public const string TempPasswordEncryptionKey = "TempPasswordEncryptionKey";
	}
}
