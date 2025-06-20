using Microsoft.Extensions.Configuration;
using Refit;
using Web.Api.Options.CommonOptions;

namespace Web.Api.Extensions;

public static class CommonServiceExtension
{
    public static ConfigurationManager AddConfigurationValues(this ConfigurationManager configuration, string environmentName)
    {
        configuration.Sources.Clear();
        configuration.AddJsonFile("appsettings.json", optional: true);

        Dictionary<string, string?> configValues = ImportConfigValues(configuration, "shared");

        configuration.AddInMemoryCollection(configValues);

        configuration.AddJsonFile($"appsettings.{environmentName}.json", optional: false);

        configuration.AddEnvironmentVariables();

        return configuration;
    }

    private static Dictionary<string, string?> ImportConfigValues(ConfigurationManager configuration, string extensionName)
    {
        string? url = configuration["appSettings:CommonUrl"];
        string? regionCode = configuration["appSettings:RegionCode"];

        if (string.IsNullOrEmpty(url))
        {
            throw new ApplicationException("CommonServiceUrlIsNotSet");
        }

        if (string.IsNullOrEmpty(regionCode))
        {
            throw new ApplicationException("CommonServiceRegionCodeIsNotSet");
        }

        string basePath = regionCode + $"/{extensionName}";
        ICommonOptionClient commonClient = RestService.For<ICommonOptionClient>(url);
        IReadOnlyList<ConfigValueResponse>? configValues = commonClient.GetConfigValuesAsync(new ConfigValueRequest
        {
            Path = basePath
        }).Result;

        if (configValues is null || !configValues.Any())
        {
            throw new ApplicationException("CommonServiceConfigValuesAreNotSet");
        }

        return ParseAddConfigValues(configValues, basePath);
    }

    private static Dictionary<string, string?> ParseAddConfigValues(IReadOnlyList<ConfigValueResponse> configResponses, string basePath)
    {
        var result = new Dictionary<string, string?>();

        foreach (ConfigValueResponse value in configResponses)
        {
            string path = value.Path.Replace(basePath + "/", "appSettings:");
            result.Add(path.Replace("/", ":"), value.Value);
        }

        return result;
    }
}
