using AxiteHr.Services.EmailAPI.Data;
using AxiteHR.Services.EmailAPI.Extensions;
using AxiteHR.Services.EmailAPI.Helpers;
using AxiteHR.Services.EmailAPI.Models.SenderOptions;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddGlobalization();

builder.Services.Configure<MailSenderOptions>(builder.Configuration.GetSection(ConfigurationHelper.EmailSenderSettings));

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(opt =>
	opt.UseSqlServer(
		builder.Configuration.GetConnectionString(ConfigurationHelper.DefaultConnectionString)
	)
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Logging
builder.AddSerilog();

//Scopes, singletons
builder.RegisterServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseAzureServiceBusConsumer();

if (builder.Configuration.GetValue<bool>(ConfigurationHelper.IsDbFromDocker))
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

try
{
	Log.Information("Starting web host");
	app.Run();
}
catch (Exception ex)
{
	Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
	Log.CloseAndFlush();
}