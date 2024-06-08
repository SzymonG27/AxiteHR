using AxiteHR.GatewaySol.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.AddAuthentication();
builder.Services.AddOcelot();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.UseOcelot();

app.Run();
