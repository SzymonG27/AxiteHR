using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Integration.MessageBus;
using AxiteHR.Services.AuthAPI.Data;
using AxiteHR.Services.AuthAPI.Extensions;
using AxiteHR.Services.AuthAPI.Models.Auth;
using AxiteHR.Services.AuthAPI.Services.Auth;
using AxiteHR.Services.AuthAPI.Services.Auth.Impl;
using AxiteHR.Services.AuthAPI.Services.Data;
using AxiteHR.Services.AuthAPI.Services.Data.Impl;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.AddGlobalization();

builder.AddAuthentication();
builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services
	.AddIdentity<AppUser, IdentityRole>()
	.AddEntityFrameworkStores<AppDbContext>()
	.AddDefaultTokenProviders();

builder.Services.AddControllers()
	.AddDataAnnotationsLocalization()
	.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

//Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

//Scopes, singletons
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IMessageBus, MessageBus>();
builder.Services.AddScoped<IDataService, DataService>();

builder.Services.AddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();
builder.Services.AddSingleton<IStringLocalizer<SharedResources>, StringLocalizer<SharedResources>>();
builder.Services.AddSingleton<IStringLocalizer<AuthResources>, StringLocalizer<AuthResources>>();

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Cors
builder.Services.AddCors(opt => opt.AddPolicy("NgOrigins",
	policy => policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader())
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

app.UseAuthorization();
app.UseAuthentication();

var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(locOptions);

app.MapControllers();

if (builder.Configuration["IsDbFromDocker"] == "true")
{
	using var scope = app.Services.CreateScope();
	var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	try
	{
		db.Database.Migrate();
	}
	catch (Exception ex)
	{
		var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
		logger.LogError(ex, "An error occurred while migrating the database.");
	}
}

app.Run();