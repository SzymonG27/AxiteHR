using AxiteHR.Services.SignalRApi.Models;

namespace AxiteHR.Services.SignalRApi.Services.Notification
{
	public interface INotificationService
	{
		Task AddNotificationAsync(string userId, NotificationDto notification);

		Task<List<NotificationDto?>> GetNotificationListAsync(string userId);

		Task RemoveNotificationAsync(string userId, string notificationId);
	}
}
