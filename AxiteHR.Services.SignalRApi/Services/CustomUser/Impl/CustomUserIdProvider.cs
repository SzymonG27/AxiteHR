using Microsoft.AspNetCore.SignalR;

namespace AxiteHR.Services.SignalRApi.Services.CustomUser.Impl
{
	public class CustomUserIdProvider : IUserIdProvider
	{
		public string GetUserId(HubConnectionContext connection)
		{
			var userId = connection.GetHttpContext()?.Request.Query["userId"].ToString();

			return !string.IsNullOrEmpty(userId) ? userId : connection.ConnectionId;
		}
	}
}
