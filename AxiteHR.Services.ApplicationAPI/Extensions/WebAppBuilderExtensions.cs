using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Services.ApplicationAPI.Helpers;
using AxiteHR.Services.ApplicationAPI.Services.Application;
using AxiteHR.Services.ApplicationAPI.Services.Application.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Globalization;
using System.Text;

namespace AxiteHR.Services.ApplicationAPI.Extensions
{
	public static class WebAppBuilderExtensions
	{
		public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder builder)
		{
			var settingsSection = builder.Configuration.GetSection("ApiSettings:JwtOptions");
			string secret = settingsSection.GetValue<string>("Secret")!;
			string issuer = settingsSection.GetValue<string>("Issuer")!;
			string audience = settingsSection.GetValue<string>("Audience")!;

			var key = Encoding.ASCII.GetBytes(secret);

			builder.Services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(x =>
			{
				x.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidIssuer = issuer,
					ValidateIssuer = true,
					ValidAudience = audience,
					ValidateAudience = true
				};
			});

			return builder;
		}

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
				.Enrich.WithProperty("Service", "applicationapi")
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
			builder.Services.AddScoped<IApplicationService, ApplicationService>();

			builder.Services.AddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();
			builder.Services.AddSingleton<IStringLocalizer<SharedResources>, StringLocalizer<SharedResources>>();
			builder.Services.AddSingleton<IStringLocalizer<ApplicationResources>, StringLocalizer<ApplicationResources>>();
			return builder;
		}
	}
}
