using System.Reflection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Serilog;
using Web.Api.DependencyInjection;
using Web.Api.DependencyInjection.Modules;
using Web.Api.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConfigurationValues(builder.Environment.EnvironmentName);

builder.Services.AddSwaggerGenWithAuth();

builder.Host.UseSerilogWithConfiguration();

builder.Services
    .AddPresentation()
    .AddInventory(builder.Configuration);

builder.Services.AddBackgroundHosted();

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
builder.Services.AddLocalizationWithCustomProvider();

WebApplication app = builder.Build();

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUi();

    //app.ApplyMigrations();
}

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseRequestLocalization();

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

await app.RunAsync();

namespace Web.Api
{
    public partial class Program;
}
