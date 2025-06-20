using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Web.Api.Extensions;

public static class SerilogExtensions
{
    public static IHostBuilder UseSerilogWithConfiguration(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, loggerConfig) =>
        {
            loggerConfig.ReadFrom.Configuration(context.Configuration);

            var seqConfig = SeqLoggerConfiguration.FromConfiguration(context.Configuration);
            System.Globalization.CultureInfo formatProvider = System.Globalization.CultureInfo.InvariantCulture;

            if (context.HostingEnvironment.IsDevelopment())
            {
                loggerConfig
                    .WriteTo.Console(formatProvider: formatProvider);
            }
            else
            {
                loggerConfig
                    .WriteTo.Seq(seqConfig.Url, apiKey: seqConfig.ApiKey,
                        formatProvider: formatProvider)
                    .Enrich.WithProperty("Area", seqConfig.Area)
                    .Enrich.WithProperty("Project", seqConfig.Project);
            }
        });

        return hostBuilder;
    }

    internal sealed class SeqLoggerConfiguration
    {
        private SeqLoggerConfiguration(string area, string project, string url, string apiKey)
        {
            Area = area;
            Project = project;
            Url = url;
            ApiKey = apiKey;
        }

        public static SeqLoggerConfiguration FromConfiguration(IConfiguration configuration)
        {
            string area = configuration["appSettings:seq:Area"] ?? "Regional.Area";
            string project = configuration["appSettings:seq:Project"] ?? "Regional.Api";
            string url = configuration["appSettings:seq:Url"] ?? string.Empty;
            string apiKey = configuration["appSettings:seq:ApiKey"] ?? string.Empty;

            return new SeqLoggerConfiguration(area, project, url, apiKey);
        }

        public string Area { get; }
        public string Project { get; }
        public string Url { get; }
        public string ApiKey { get; }
    }
}
