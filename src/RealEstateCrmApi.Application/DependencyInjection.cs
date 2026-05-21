using Microsoft.Extensions.DependencyInjection;
using RealEstateCrmApi.Application.Properties;

namespace RealEstateCrmApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPropertyService, PropertyService>();

        return services;
    }
}
