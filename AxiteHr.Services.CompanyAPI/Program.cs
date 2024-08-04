using AutoMapper;
using AxiteHr.Services.CompanyAPI;
using AxiteHr.Services.CompanyAPI.Data;
using AxiteHr.Services.CompanyAPI.Helpers;
using AxiteHr.Services.CompanyAPI.Services.Company;
using AxiteHr.Services.CompanyAPI.Services.Company.Impl;
using AxiteHr.Services.CompanyAPI.Services.Employee;
using AxiteHr.Services.CompanyAPI.Services.Employee.Impl;
using AxiteHR.GatewaySol.Extensions;
using AxiteHR.GlobalizationResources.Resources;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddGlobalization();
builder.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

IMapper mapper = MapperConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHttpClient(
	HttpClientNameHelper.Auth,
	configureClient => configureClient.BaseAddress = new Uri(builder.Configuration["ServiceUrls:AuthAPI"]!)
);

builder.Services.AddControllers()
	.AddDataAnnotationsLocalization()
	.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

//Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

//Scopes, singletons
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<ICompanyCreatorService, CompanyCreatorService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();
builder.Services.AddSingleton<IStringLocalizer<SharedResources>, StringLocalizer<SharedResources>>();
builder.Services.AddSingleton<IStringLocalizer<CompanyResources>, StringLocalizer<CompanyResources>>();

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