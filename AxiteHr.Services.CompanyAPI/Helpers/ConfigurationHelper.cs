namespace AxiteHr.Services.CompanyAPI.Helpers
{
	public static class ConfigurationHelper
	{
		public const string DefaultConnectionString = "DefaultConnection";
		public const string AuthApiUrl = "ServiceUrls:AuthAPI";
		public const string IsDbFromDocker = "IsDbFromDocker";
		public const string LogStashUrl = "LogstashConfig:LogStashUrl";
		public const string LogStashQueueLimitBytes = "LogstashConfig:QueueLimitBytes";
	}
}
