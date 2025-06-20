using System.Text;
using Inventory.Application.Abstractions.Authentication;
using Inventory.Infrastructure.Authentication;
using Inventory.Infrastructure.DomainEvents;
using Inventory.Infrastructure.Time;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Inventory.SharedKernel;

namespace Inventory.Infrastructure;

public static class DependencyInjection
{
    private const string HospitalConnectionStringKey = "appSettings:Finance_Connection";

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddServices()
            //.AddDatabase(configuration)
            .AddHealthChecks(configuration)
            .AddAuthenticationInternal(configuration)
            .AddAuthorizationInternal();
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IFormatProvider>(System.Globalization.CultureInfo.InvariantCulture);

        services.AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>();

        return services;
    }

    // private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    // {
    //     //string connectionString = configuration.GetSection(HospitalConnectionStringKey)?.Value ?? string.Empty;
    //
    //     // services.AddDbContext<ApplicationDbContext>(
    //     //     options => options
    //     //         .UseSqlServer(connectionString));
    //     //
    //     // services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
    //
    //     return services;
    // }

    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetSection(HospitalConnectionStringKey)?.Value ?? string.Empty;

        services
            .AddHealthChecks()
            .AddSqlServer(connectionString);

        return services;
    }

    private static IServiceCollection AddAuthenticationInternal(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                string? issuer = configuration.GetSection("appSettings:Jwt:Issuer").Get<string>();
                string? audience = configuration.GetSection("appSettings:Jwt:Audience").Get<string>();
                string secret = configuration.GetSection("appSettings:Jwt:Secret").Get<string>()!;

                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,

                    ValidateAudience = true,
                    ValidAudience = audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret.PadRight(64, '\0'))),

                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                };
            });

        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();

        return services;
    }

    private static IServiceCollection AddAuthorizationInternal(this IServiceCollection services)
    {
        services.AddAuthorization();

        return services;
    }
}
