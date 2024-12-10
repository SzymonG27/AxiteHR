using AxiteHR.Services.SignalRApi.Helpers;
using Microsoft.AspNetCore.SignalR;

namespace AxiteHR.Services.SignalRApi.Hubs
{
	public class NotificationHub : Hub
	{
		public async Task SendMessage(string userId, string message)
		{
			await Clients.User(userId).SendAsync(SignalrMethodHelper.NotificationReceive, message);
		}
	}
}
