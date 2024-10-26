using AutoMapper;
using AxiteHr.Services.CompanyAPI;
using AxiteHr.Services.CompanyAPI.Data;
using AxiteHR.Services.CompanyAPI.Extensions;
using AxiteHr.Services.CompanyAPI.Helpers;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddGlobalization();
builder.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(opt =>
	opt.UseSqlServer(
		builder.Configuration.GetConnectionString(ConfigurationHelper.DefaultConnectionString)
	)
);

IMapper mapper = MapperConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHttpClient(
	HttpClientNameHelper.Auth,
	configureClient => configureClient.BaseAddress = new Uri(builder.Configuration[ConfigurationHelper.AuthApiUrl]!)
);

builder.Services.AddControllers()
	.AddDataAnnotationsLocalization()
	.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

//Logging
builder.AddSerilog();

//Scopes, singletons
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

app.UseAuthorization();
app.UseAuthentication();

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