using Agrimetrics.DataShare.Api.Controllers.Acquirer.DataShareRequests;
using Agrimetrics.DataShare.Api.Controllers.Admin;
using Agrimetrics.DataShare.Api.Controllers.AuditLogs;
using Agrimetrics.DataShare.Api.Controllers.QuestionConfiguration;
using Agrimetrics.DataShare.Api.Controllers.Reporting;
using Agrimetrics.DataShare.Api.Controllers.Supplier.DataShareRequests;
using Agrimetrics.DataShare.Api.Core.Boot;
using Agrimetrics.DataShare.Api.Db.Boot;
using Agrimetrics.DataShare.Api.Logic.Boot;

namespace Agrimetrics.DataShare.Api.Boot;

internal class DependencyRegistration : IDependencyRegistration
{
    IServiceCollection IDependencyRegistration.RegisterServiceDependencies(IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.RegisterCoreDependencies();

        services.AddScoped<IAcquirerDataShareRequestResponseFactory, AcquirerDataShareRequestResponseFactory>();
        services.AddScoped<ISupplierDataShareRequestResponseFactory, SupplierDataShareRequestResponseFactory>();
        services.AddScoped<IAuditLogResponseFactory, AuditLogResponseFactory>();
        services.AddScoped<IReportingResponseFactory, ReportingResponseFactory>();
        services.AddScoped<IAdminResponseFactory, AdminResponseFactory>();
        services.AddScoped<IQuestionConfigurationResponseFactory, QuestionConfigurationResponseFactory>();

        services.RegisterDbAccessDependencies();
        services.RegisterApiLogicDependencies();

        return services;
    }
}