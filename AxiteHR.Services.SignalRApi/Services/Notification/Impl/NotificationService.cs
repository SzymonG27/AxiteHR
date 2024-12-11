using AxiteHR.Integration.Cache.Redis;
using AxiteHR.Integration.GlobalClass.Redis.Keys;
using AxiteHR.Services.SignalRApi.Helpers;
using AxiteHR.Services.SignalRApi.Hubs;
using AxiteHR.Services.SignalRApi.Models;
using Microsoft.AspNetCore.SignalR;

namespace AxiteHR.Services.SignalRApi.Services.Notification.Impl
{
	public class NotificationService(
		IRedisCacheService redisCacheService,
		IHubContext<NotificationHub> notificationHubContext) : INotificationService
	{
		public async Task AddNotificationAsync(string userId, NotificationDto notification)
		{
			var key = SignalrRedisKeys.NotificationHubCache(userId);
			await redisCacheService.PushRightObjectAsync(key, notification);
		}

		public async Task AddNotificationWithHubSendAsync(string userId, NotificationDto notification)
		{
			var key = SignalrRedisKeys.NotificationHubCache(userId);
			await redisCacheService.PushRightObjectAsync(key, notification);

			await notificationHubContext.Clients.User(userId).SendAsync(SignalrMethodHelper.NotificationReceive, notification);
		}

		public async Task<List<NotificationDto?>> GetNotificationListAsync(string userId)
		{
			var key = SignalrRedisKeys.NotificationHubCache(userId);

			return await redisCacheService.GetObjectListAsync<NotificationDto>(key);
		}

		public async Task RemoveNotificationAsync(string userId, string notificationId)
		{
			var key = SignalrRedisKeys.NotificationHubCache(userId);
			await redisCacheService.RemoveFromObjectListAsync<NotificationDto>(key, notificationId);
		}
	}
}
