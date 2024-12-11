using AxiteHR.GatewaySol.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.AddAuthentication();

builder.Configuration.AddJsonFile("ocelot.json", false, true);
builder.Services.AddOcelot(builder.Configuration);

//Cors
builder.Services.AddCors(opt => opt.AddPolicy("NgOrigins",
	policy => policy.WithOrigins("http://localhost:4200")
		.AllowAnyMethod()
		.AllowAnyHeader()
		.AllowCredentials())
);

var app = builder.Build();

app.UseWebSockets();

app.UseCors("NgOrigins");

app.MapGet("/", () => "Hello World!");

app.UseOcelot().Wait();

await app.RunAsync();