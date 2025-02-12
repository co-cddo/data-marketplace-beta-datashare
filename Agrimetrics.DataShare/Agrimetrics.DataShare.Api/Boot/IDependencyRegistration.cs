namespace Agrimetrics.DataShare.Api.Boot;

public interface IDependencyRegistration
{
    IServiceCollection RegisterServiceDependencies(IServiceCollection services);
}