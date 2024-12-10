using AxiteHR.Services.SignalRApi.Models;
using AxiteHR.Services.SignalRApi.Services.Notification;
using Microsoft.AspNetCore.Mvc;

namespace AxiteHR.Services.SignalRApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NotificationController(INotificationService notificationService) : ControllerBase
	{
		[HttpGet("{userId}")]
		public async Task<IActionResult> GetNotifications(string userId)
		{
			var notifications = await notificationService.GetNotificationListAsync(userId);
			return Ok(notifications);
		}

		[HttpPost("{userId}")]
		public async Task<IActionResult> AddNotification(string userId, [FromBody] NotificationDto notification)
		{
			await notificationService.AddNotificationAsync(userId, notification);
			return Ok();
		}

		[HttpDelete("{userId}/{notificationId}")]
		public async Task<IActionResult> DeleteNotification(string userId, string notificationId)
		{
			await notificationService.RemoveNotificationAsync(userId, notificationId);
			return Ok();
		}
	}
}
