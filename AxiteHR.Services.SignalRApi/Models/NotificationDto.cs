using AxiteHR.Integration.GlobalClass.Redis;

namespace AxiteHR.Services.SignalRApi.Models
{
	public class NotificationDto : RedisObjectList
	{
		public string Message { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}
}
