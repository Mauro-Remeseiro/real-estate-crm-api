using Microsoft.Extensions.DependencyInjection;
using RealEstateCrmApi.Application.Auth;
using RealEstateCrmApi.Application.Clients;
using RealEstateCrmApi.Application.Properties;
using RealEstateCrmApi.Application.Visits;

namespace RealEstateCrmApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPropertyService, PropertyService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IVisitService, VisitService>();

        return services;
    }
}
