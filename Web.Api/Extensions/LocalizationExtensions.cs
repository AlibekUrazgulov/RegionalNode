using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Api.Extensions;

public static class LocalizationExtensions
{
    public static IServiceCollection AddLocalizationWithCustomProvider(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.Configure<RequestLocalizationOptions>(options =>
        {
            var defaultCulture = new CultureInfo("ru");
            CultureInfo[] supportedCultures =
            [
                defaultCulture,
                new("kk")
            ];

            options.DefaultRequestCulture = new RequestCulture(culture: defaultCulture.Name);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;

            CustomRequestCultureProvider customRequestCultureProvider = CreateCustomRequestCultureProvider();
            options.AddInitialRequestCultureProvider(customRequestCultureProvider);
        });

        return services;
    }

    private static CustomRequestCultureProvider CreateCustomRequestCultureProvider()
    {
        return new CustomRequestCultureProvider(async context =>
        {
            string? lang = context.Request.Headers.AcceptLanguage.FirstOrDefault()?.Split(";")[0];
            if (string.IsNullOrEmpty(lang))
            {
                lang = context.Request.Cookies["lang"];
            }
            return await Task.FromResult(new ProviderCultureResult(lang));
        });
    }
}
