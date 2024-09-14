using AxiteHR.Integration.BrokerMessageSender.Models;
using AxiteHR.Services.AuthAPI.Data;
using AxiteHR.Services.AuthAPI.Extensions;
using AxiteHR.Services.AuthAPI.Helpers;
using AxiteHR.Services.AuthAPI.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddGlobalization();

builder.AddAuthentication();
builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(ConfigurationHelper.JwtOptions));
builder.Services.Configure<RabbitMqMessageSenderConfig>(builder.Configuration.GetSection(ConfigurationHelper.RabbitMqBrokerMessageSenderConfig));

builder.Services.AddDbContext<AppDbContext>(opt =>
	opt.UseSqlServer(
		builder.Configuration.GetConnectionString(ConfigurationHelper.DefaultConnectionString)
	)
);

builder.Services
	.AddIdentity<AppUser, IdentityRole>()
	.AddEntityFrameworkStores<AppDbContext>()
	.AddDefaultTokenProviders();

builder.Services.AddControllers()
	.AddDataAnnotationsLocalization()
	.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

//Logging
builder.AddSerilog();

//Service registration
builder.RegisterServices();

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

app.UseAuthentication();
app.UseAuthorization();

var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(locOptions);

app.MapControllers();

if (builder.Configuration.GetValue<bool>(ConfigurationHelper.IsDbFromDocker))
{
	using var scope = app.Services.CreateScope();
	var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	try
	{
		await db.Database.MigrateAsync();
	}
	catch (Exception ex)
	{
		var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
		logger.LogError(ex, "An error occurred while migrating the database.");
	}
}

try
{
	Log.Information("Starting web host");
	await app.RunAsync();
}
catch (Exception ex)
{
	Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
	await Log.CloseAndFlushAsync();
}