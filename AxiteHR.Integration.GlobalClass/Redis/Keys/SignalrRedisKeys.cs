namespace AxiteHR.Integration.GlobalClass.Redis.Keys
{
	public static class SignalrRedisKeys
	{
		//Single object

		//List of objects
		public static string NotificationHubCache(string userId) => $"notificationhub:{userId}";
	}
}
