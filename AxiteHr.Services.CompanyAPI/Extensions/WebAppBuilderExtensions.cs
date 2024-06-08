using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;

namespace AxiteHR.GatewaySol.Extensions
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
			builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
			builder.Services.Configure<RequestLocalizationOptions>(options =>
			{
				var supportedCultures = GetSupportedCultures();

				options.DefaultRequestCulture = new RequestCulture("en");
				options.SupportedCultures = supportedCultures;
				options.SupportedUICultures = supportedCultures;

				options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
				{
					var lang = context.Request.Headers["Accept-Language"].ToString();
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
				return new List<CultureInfo>
				{
					new CultureInfo("en"),
					new CultureInfo("pl")
				};
			}
		}
	}
}
