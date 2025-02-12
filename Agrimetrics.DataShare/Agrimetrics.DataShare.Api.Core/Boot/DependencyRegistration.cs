using Agrimetrics.DataShare.Api.Core.Configuration;
using Agrimetrics.DataShare.Api.Core.SystemProxies;
using Microsoft.Extensions.DependencyInjection;

namespace Agrimetrics.DataShare.Api.Core.Boot;

public static class DependencyRegistration
{
    public static IServiceCollection RegisterCoreDependencies(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<IServiceConfigurationPresenter, ServiceConfigurationPresenter>();
        services.AddScoped<IClock, Clock>();
        
        return services;
    }
}