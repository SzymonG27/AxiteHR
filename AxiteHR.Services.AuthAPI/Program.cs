using AxiteHR.GlobalizationResources;
using AxiteHR.Services.AuthAPI.Data;
using AxiteHR.Services.AuthAPI.Models;
using AxiteHR.Services.AuthAPI.Services;
using AxiteHR.Services.AuthAPI.Services.Impl;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

//Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
	var supportedCultures = new List<CultureInfo>
			{
				new CultureInfo("en"),
				new CultureInfo("pl")
			};

	options.DefaultRequestCulture = new RequestCulture("en");
	options.SupportedCultures = supportedCultures;
	options.SupportedUICultures = supportedCultures;

	options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
	{
		var lang = await Task.Run(() => context.Request.Headers["Accept-Language"].ToString());
		if (string.IsNullOrEmpty(lang))
		{
			lang = "en";
		}
		return new ProviderCultureResult(lang, lang);
	}));
});

// Add services to the container.
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));
builder.Services.AddDbContext<AppDbContext>(opt =>
{
	opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services
	.AddIdentity<AppUser, IdentityRole>()
	.AddEntityFrameworkStores<AppDbContext>()
	.AddDefaultTokenProviders();

builder.Services.AddControllers()
	.AddDataAnnotationsLocalization()
	.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

//Scopes, singletons
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();
builder.Services.AddSingleton<IStringLocalizer, StringLocalizer<SharedResources>>();
builder.Services.AddSingleton<IStringLocalizer, StringLocalizer<AuthResources>>();

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Cors
builder.Services.AddCors(opt => opt.AddPolicy("NgOrigins", 
	policy =>
	{
		policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
	})
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors("NgOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(locOptions);

app.MapControllers();

app.Run();