﻿using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Security.Encryption;
using AxiteHR.Services.EmailAPI.Helpers;
using AxiteHR.Services.EmailAPI.Messaging;
using AxiteHR.Services.EmailAPI.Services.EmailSender;
using AxiteHR.Services.EmailAPI.Services.EmployeeTempPassword;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Serilog;
using System.Globalization;

namespace AxiteHR.Services.EmailAPI.Extensions
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
			builder.Services.AddSingleton<IStringLocalizer<EmailResources>, StringLocalizer<EmailResources>>();

			builder.Services.AddSingleton<IEmployeeTempPasswordService, EmployeeTempPasswordService>();
			builder.Services.AddSingleton<IEmailSender, EmailSender>();
			builder.Services.AddSingleton<IEncryptionService, EncryptionService>();

			//Messaging consumers
			builder.Services.AddHostedService<RabbitMqEmployeeTempPasswordConsumer>();

			return builder;
		}
	}
}
