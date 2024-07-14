using AxiteHR.Services.EmailAPI.Messaging;

namespace AxiteHR.Services.EmailAPI.Extensions
{
	public static class ApplicationBuilderExtension
	{
		private static IAzureServiceBusConsumer? ServiceBusConsumer;

		public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
		{
			var logger = app.ApplicationServices.GetService<ILogger<IAzureServiceBusConsumer>>();

			ServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
			if (ServiceBusConsumer == null)
			{
				logger?.LogError("AzureServiceBusConsumer service is not registered in the application services.");
				throw new ArgumentException("AzureServiceBusConsumer service is not registered in the application services.");
			}

			var hostApplicationLifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();
			if (hostApplicationLifetime == null)
			{
				logger?.LogError("HostApplicationLifetime service is not registered in the application services.");
				throw new ArgumentException("HostApplicationLifetime service is not registered in the application services.");
			}

			hostApplicationLifetime.ApplicationStarted.Register(OnStart);
			hostApplicationLifetime.ApplicationStopping.Register(OnStop);

			return app;
		}

		#region Private Methods
		private static void OnStart()
		{
			ServiceBusConsumer!.Start();
		}

		private static void OnStop()
		{
			ServiceBusConsumer!.Stop();
		}
		#endregion
	}
}
