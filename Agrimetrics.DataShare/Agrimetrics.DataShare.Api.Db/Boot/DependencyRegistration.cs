using Agrimetrics.DataShare.Api.Db.Configuration;
using Agrimetrics.DataShare.Api.Db.DbAccess;
using Microsoft.Extensions.DependencyInjection;

namespace Agrimetrics.DataShare.Api.Db.Boot;

public static class DependencyRegistration
{
    public static IServiceCollection RegisterDbAccessDependencies(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<IDatabaseConnectionsConfigurationPresenter, DatabaseConnectionsConfigurationPresenter>(); 
        services.AddScoped<IDatabaseChannelCreation, DatabaseChannelCreation>();
        services.AddScoped<IDatabaseCommandRunner, DatabaseCommandRunner>();

        return services;
    }
}