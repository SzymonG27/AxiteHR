using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
	}
}
