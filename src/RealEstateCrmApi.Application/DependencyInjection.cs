using Microsoft.Extensions.DependencyInjection;
using RealEstateCrmApi.Application.Auth;
using RealEstateCrmApi.Application.Properties;

namespace RealEstateCrmApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPropertyService, PropertyService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
