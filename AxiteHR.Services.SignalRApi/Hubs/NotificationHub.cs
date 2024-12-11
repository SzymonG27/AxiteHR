using AxiteHR.Services.SignalRApi.Helpers;
using AxiteHR.Services.SignalRApi.Models;
using Microsoft.AspNetCore.SignalR;

namespace AxiteHR.Services.SignalRApi.Hubs
{
	public class NotificationHub : Hub
	{
		public async Task SendMessage(string userId, string header, string message)
		{
			NotificationDto notification = new()
			{
				Header = header,
				Message = message,
				CreatedAt = DateTime.UtcNow
			};

			await Clients.User(userId).SendAsync(SignalrMethodHelper.NotificationReceive, notification);
		}
	}
}
