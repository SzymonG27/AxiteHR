using AxiteHR.Services.InvoiceAPI.Extensions;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddGlobalization();
builder.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddControllers()
	.AddDataAnnotationsLocalization()
	.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

//Logging
builder.AddSerilog();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
