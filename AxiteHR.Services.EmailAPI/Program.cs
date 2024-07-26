using AxiteHr.Services.EmailAPI.Data;
using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Services.EmailAPI.Extensions;
using AxiteHR.Services.EmailAPI.Messaging;
using AxiteHR.Services.EmailAPI.Models.SenderOptions;
using AxiteHR.Services.EmailAPI.Services.EmailSender;
using AxiteHR.Services.EmailAPI.Services.EmployeeTempPassword;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.AddGlobalization();

builder.Services.Configure<MailSenderOptions>(builder.Configuration.GetSection("EmailSenderSettings"));

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

//Scopes, singletons
builder.Services.AddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();
builder.Services.AddSingleton<IStringLocalizer<SharedResources>, StringLocalizer<SharedResources>>();
builder.Services.AddSingleton<IStringLocalizer<EmailResources>, StringLocalizer<EmailResources>>();

builder.Services.AddSingleton<IEmployeeTempPasswordService, EmployeeTempPasswordService>();
builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();

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

app.Run();