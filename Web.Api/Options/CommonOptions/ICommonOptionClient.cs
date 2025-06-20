using Refit;

namespace Web.Api.Options.CommonOptions;

public interface ICommonOptionClient
{
    [Post("/api/configValue/list")]
    Task<IReadOnlyList<ConfigValueResponse>> GetConfigValuesAsync(ConfigValueRequest request);
}
