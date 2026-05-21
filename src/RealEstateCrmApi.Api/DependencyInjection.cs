using RealEstateCrmApi.Api.Services;
using RealEstateCrmApi.Application.Common.Interfaces;

namespace RealEstateCrmApi.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
