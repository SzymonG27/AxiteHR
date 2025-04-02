using AxiteHR.Services.SignalRApi.Helpers;
using AxiteHR.Services.SignalRApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace AxiteHR.Services.SignalRApi.Hubs
{
	public class NotificationHub : Hub
	{

		private static readonly ConcurrentDictionary<string, string> _connectionsWithUserCompanyId = new();

		public override async Task OnConnectedAsync()
		{
			var httpContext = Context.GetHttpContext();
			var userCompanyId = httpContext?.Request.Query["userCompanyId"].ToString();

			if (!string.IsNullOrEmpty(userCompanyId))
			{
				_connectionsWithUserCompanyId[Context.ConnectionId] = userCompanyId;
				Console.WriteLine($"UserCompanyId={userCompanyId} connected (ConnectionId={Context.ConnectionId})");
			}

			await base.OnConnectedAsync();
		}

		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			if (_connectionsWithUserCompanyId.TryRemove(Context.ConnectionId, out var userCompanyId))
			{
				Console.WriteLine($"UserCompanyId={userCompanyId} disconnected (ConnectionId={Context.ConnectionId})");
			}

			await base.OnDisconnectedAsync(exception);
		}

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task SendMessageAsync(string userCompanyId, string header, string message)
		{
			NotificationDto notification = new()
			{
				Header = header,
				Message = message,
				CreatedAt = DateTime.UtcNow
			};

			var connectionIds = _connectionsWithUserCompanyId
				.Where(x => x.Value == userCompanyId)
				.Select(x => x.Key)
				.ToList();

			foreach (var connectionId in connectionIds)
			{
				await Clients.Client(connectionId).SendAsync(SignalrMethodHelper.NotificationReceive, notification);
			}
		}
	}
}
