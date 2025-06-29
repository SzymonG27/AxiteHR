﻿using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Integration.Cache.Redis;
using AxiteHR.Services.CompanyAPI.Helpers;
using AxiteHR.Services.CompanyAPI.Infrastructure.AuthApi;
using AxiteHR.Services.CompanyAPI.Services.Company;
using AxiteHR.Services.CompanyAPI.Services.Company.Impl;
using AxiteHR.Services.CompanyAPI.Services.CompanyPermission;
using AxiteHR.Services.CompanyAPI.Services.CompanyPermission.Impl;
using AxiteHR.Services.CompanyAPI.Services.CompanyRole;
using AxiteHR.Services.CompanyAPI.Services.CompanyRole.Impl;
using AxiteHR.Services.CompanyAPI.Services.CompanyUser;
using AxiteHR.Services.CompanyAPI.Services.CompanyUser.Impl;
using AxiteHR.Services.CompanyAPI.Services.Employee;
using AxiteHR.Services.CompanyAPI.Services.Employee.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using StackExchange.Redis;
using System.Globalization;
using System.Text;

namespace AxiteHR.Services.CompanyAPI.Extensions
{
	public static class WebAppBuilderExtensions
	{
		public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder builder)
		{
			//ToDo IOptions
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
				.Enrich.WithProperty("Service", "companyapi")
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
			var redisConnectionString = builder.Configuration.GetConnectionString(ConfigurationHelper.RedisConnectionString)!;
			builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));
			builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();

			builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
			builder.Services.AddScoped<ICompanyService, CompanyService>();
			builder.Services.AddScoped<ICompanyManagerService, CompanyManagerService>();
			builder.Services.AddScoped<ICompanyUserService, CompanyUserService>();
			builder.Services.AddScoped<ICompanyRoleService, CompanyRoleService>();
			builder.Services.AddScoped<ICompanyPermissionService, CompanyPermissionService>();
			builder.Services.AddScoped<IEmployeeService, EmployeeService>();
			builder.Services.AddScoped<IAuthApiClient, AuthApiClient>();

			builder.Services.AddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();
			builder.Services.AddSingleton<IStringLocalizer<SharedResources>, StringLocalizer<SharedResources>>();
			builder.Services.AddSingleton<IStringLocalizer<CompanyResources>, StringLocalizer<CompanyResources>>();

			return builder;
		}
	}
}
