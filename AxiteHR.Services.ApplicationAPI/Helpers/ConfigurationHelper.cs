namespace AxiteHR.Services.ApplicationAPI.Helpers
{
	public record ConfigurationHelper
	{
		public const string DefaultConnectionString = "DefaultConnection";
		public const string IsDbFromDocker = "IsDbFromDocker";
		public const string LogStashUrl = "LogstashConfig:LogStashUrl";
		public const string LogStashQueueLimitBytes = "LogstashConfig:QueueLimitBytes";
		public const string CompanyApiUrl = "ServiceUrls:CompanyAPI";
		public const string RedisSection = "Redis";
	}
}
