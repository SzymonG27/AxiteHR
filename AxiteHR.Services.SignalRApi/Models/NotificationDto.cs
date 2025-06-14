using AxiteHR.Integration.GlobalClass.Redis;

namespace AxiteHR.Services.SignalRApi.Models
{
	public class NotificationDto : RedisObjectList
	{
		public string Header { get; set; } = string.Empty;
		public string Message { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}
}
