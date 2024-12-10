using AxiteHR.Integration.Cache.Redis;
using AxiteHR.Services.SignalRApi.Helpers;
using AxiteHR.Services.SignalRApi.Services.Notification;
using AxiteHR.Services.SignalRApi.Services.Notification.Impl;
using StackExchange.Redis;

namespace AxiteHR.Services.SignalRApi.Extensions
{
	public static class WebAppBuilderExtensions
	{
		public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
		{
			var redisConnectionString = builder.Configuration.GetConnectionString(ConfigurationHelper.RedisConnectionString)!;
			builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));

			builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();
			builder.Services.AddScoped<INotificationService, NotificationService>();

			return builder;
		}
	}
}
