using AxiteHR.Services.SignalRApi.Models;
using AxiteHR.Services.SignalRApi.Services.Notification;
using Microsoft.AspNetCore.Mvc;

namespace AxiteHR.Services.SignalRApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NotificationController(INotificationService notificationService) : ControllerBase
	{
		[HttpGet("[action]/{userId}")]
		public async Task<IActionResult> GetNotificationListAsync(string userId)
		{
			var notifications = await notificationService.GetNotificationListAsync(userId);
			return Ok(notifications);
		}

		[HttpPost("[action]/{userId}")]
		public async Task<IActionResult> AddNotificationAsync(string userId, [FromBody] NotificationDto notification)
		{
			await notificationService.AddNotificationAsync(userId, notification);
			return Ok();
		}

		[HttpPost("[action]/{userId}")]
		public async Task<IActionResult> AddNotificationWithHubSendAsync(string userId, [FromBody] NotificationDto notification)
		{
			await notificationService.AddNotificationWithHubSendAsync(userId, notification);
			return Ok();
		}

		[HttpDelete("[action]/{userId}/{notificationId}")]
		public async Task<IActionResult> DeleteNotificationAsync(string userId, string notificationId)
		{
			await notificationService.RemoveNotificationAsync(userId, notificationId);
			return Ok();
		}
	}
}
