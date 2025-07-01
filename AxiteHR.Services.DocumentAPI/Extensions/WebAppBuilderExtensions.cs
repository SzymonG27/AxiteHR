using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Integration.Cache.Redis;
using AxiteHR.Integration.Storage.Abstractions;
using AxiteHR.Integration.Storage.Factory;
using AxiteHR.Integration.Storage.Providers.Minio;
using AxiteHR.Services.DocumentAPI.Helpers;
using AxiteHR.Services.DocumentAPI.Messaging;
using AxiteHR.Services.DocumentAPI.Services.Invoice;
using AxiteHR.Services.DocumentAPI.Services.Invoice.Impl;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Serilog;
using StackExchange.Redis;
using System.Globalization;

namespace AxiteHR.Services.DocumentAPI.Extensions
{
	public static class WebAppBuilderExtensions
	{
		public static WebApplicationBuilder AddGlobalization(this WebApplicationBuilder builder)
		{
			builder.Services.AddLocalization();
			builder.Services.Configure<RequestLocalizationOptions>(options =>
			{
				var supportedCultures = GetSupportedCultures();

				options.DefaultRequestCulture = new RequestCulture("en");
				options.SupportedCultures = supportedCultures;
				options.SupportedUICultures = supportedCultures;

				options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
				{
					var lang = context.Request.Headers.AcceptLanguage.ToString();
					if (string.IsNullOrEmpty(lang))
					{
						lang = "en";
					}
					return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(lang, lang));
				}));
			});

			return builder;

			static List<CultureInfo> GetSupportedCultures()
			{
				return
				[
					new CultureInfo("en"),
					new CultureInfo("pl")
				];
			}
		}

		public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Information()
				.Enrich.WithProperty("Service", "emailapi")
				.Enrich.FromLogContext()
				.WriteTo.Http(
					builder.Configuration[ConfigurationHelper.LogStashUrl]!,
					builder.Configuration.GetValue<long>(ConfigurationHelper.LogStashQueueLimitBytes)
				)
				.WriteTo.Console()
				.CreateLogger();

			builder.Host.UseSerilog();

			return builder;
		}

		public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
		{
			builder.Services.AddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();
			builder.Services.AddSingleton<IStringLocalizer<SharedResources>, StringLocalizer<SharedResources>>();
			builder.Services.AddSingleton<IStringLocalizer<DocumentResources>, StringLocalizer<DocumentResources>>();

			var redisConnectionString = builder.Configuration.GetConnectionString("Redis")!;
			builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));
			builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();

			builder.Services.AddSingleton<MinioService>();
			builder.Services.AddSingleton<IStorageFactory, StorageFactory>();

			builder.Services.AddSingleton<IInvoiceGeneratorService, InvoiceGeneratorService>();

			builder.Services.AddHostedService<RabbitMqInvoiceGeneratorConsumer>();

			return builder;
		}
	}
}
