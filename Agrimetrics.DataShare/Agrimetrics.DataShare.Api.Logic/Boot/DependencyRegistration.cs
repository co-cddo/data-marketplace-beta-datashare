using Agrimetrics.DataShare.Api.Logic.Configuration;
using Agrimetrics.DataShare.Api.Logic.ModelData;
using Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.ModelData.QuestionConfiguration;
using Agrimetrics.DataShare.Api.Logic.Repositories.AcquirerDataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Repositories.AnswerHighlights;
using Agrimetrics.DataShare.Api.Logic.Repositories.AnswerValidation;
using Agrimetrics.DataShare.Api.Logic.Repositories.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.Repositories.KeyQuestionPartAnswers;
using Agrimetrics.DataShare.Api.Logic.Repositories.QuestionConfiguration;
using Agrimetrics.DataShare.Api.Logic.Repositories.Reporting;
using Agrimetrics.DataShare.Api.Logic.Repositories.SupplierDataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest;
using Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestNotificationRecipientDeterminations;
using Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestQuestionStatusesDeterminations;
using Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.NextQuestionDeterminations;
using Agrimetrics.DataShare.Api.Logic.Services.Admin;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerHighlights;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation;
using Agrimetrics.DataShare.Api.Logic.Services.AuditLog;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas.Configuration;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas.DataSetPresentation;
using Agrimetrics.DataShare.Api.Logic.Services.KeyQuestionPartAnswers;
using Agrimetrics.DataShare.Api.Logic.Services.Notification;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Building;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Client;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core;
using Agrimetrics.DataShare.Api.Logic.Services.QuestionConfiguration;
using Agrimetrics.DataShare.Api.Logic.Services.QuestionSetPlaceHolderReplacement;
using Agrimetrics.DataShare.Api.Logic.Services.Reporting;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;
using Agrimetrics.DataShare.Api.Logic.Services.SupplierDataShareRequest;
using Agrimetrics.DataShare.Api.Logic.Services.SupplierDataShareRequest.SubmissionContentFileBuilding;
using Agrimetrics.DataShare.Api.Logic.Services.Users;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Configuration;
using Agrimetrics.DataShare.Api.Logic.Services.Users.UserIdPresentation;
using Microsoft.Extensions.DependencyInjection;

namespace Agrimetrics.DataShare.Api.Logic.Boot;

public static class DependencyRegistration
{
    public static IServiceCollection RegisterApiLogicDependencies(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // -- Services
        services.AddScoped<IAcquirerDataShareRequestService, AcquirerDataShareRequestService>();
        services.AddScoped<ISupplierDataShareRequestService, SupplierDataShareRequestService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IAuditLogService, AuditLogService>();
        services.AddScoped<IReportingService, ReportingService>();
        services.AddScoped<IDataShareRequestQuestionAnswerValidationService, DataShareRequestQuestionAnswerValidationService>();
        services.AddScoped<IQuestionSetPlaceholderReplacementService, QuestionSetPlaceholderReplacementService>();
        services.AddScoped<IQuestionConfigurationService, QuestionConfigurationService>();
        services.AddScoped<IKeyQuestionPartAnswerProviderService, KeyQuestionPartAnswerProviderService>();
        services.AddScoped<IAnswerHighlightsService, AnswerHighlightsService>();
        services.AddScoped<IServiceOperationResultFactory, ServiceOperationResultFactory>();

        // -- Service Utilities
        services.AddScoped<IAcquirerDataShareRequestModelDataFactory, AcquirerDataShareRequestModelDataFactory>();
        services.AddScoped<ISupplierDataShareRequestModelDataFactory, SupplierDataShareRequestModelDataFactory>();
        services.AddScoped<IDataShareRequestQuestionStatusesDetermination, DataShareRequestQuestionStatusesDetermination>();
        services.AddScoped<IDataShareRequestQuestionSetCompletenessDetermination, DataShareRequestQuestionSetCompletenessDetermination>();
        services.AddScoped<INextQuestionDetermination, NextQuestionDetermination>();
        services.AddScoped<IQuestionPartAnswerValidation, QuestionPartAnswerValidation>();
        services.AddScoped<IQuestionConfigurationModelDataFactory, QuestionConfigurationModelDataFactory>();
        services.AddScoped<IAuditLogModelDataFactory, AuditLogModelDataFactory>();

        // -- Repo Access
        services.AddScoped<IAcquirerDataShareRequestRepository, AcquirerDataShareRequestRepository>();
        services.AddScoped<IAcquirerDataShareRequestSqlQueries, AcquirerDataShareRequestSqlQueries>();
        services.AddScoped<ISupplierDataShareRequestRepository, SupplierDataShareRequestRepository>();
        services.AddScoped<ISupplierDataShareRequestSqlQueries, SupplierDataShareRequestSqlQueries>();
        services.AddScoped<IAnswerValidationRepository, AnswerValidationRepository>();
        services.AddScoped<IAnswerValidationSqlQueries, AnswerValidationSqlQueries>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<IAuditLogSqlQueries, AuditLogSqlQueries>();
        services.AddScoped<IReportingRepository, ReportingRepository>();
        services.AddScoped<IReportingSqlQueries, ReportingSqlQueries>();
        services.AddScoped<IQuestionConfigurationRepository, QuestionConfigurationRepository>();
        services.AddScoped<IQuestionConfigurationSqlQueries, QuestionConfigurationSqlQueries>();
        services.AddScoped<IKeyQuestionPartAnswersRepository, KeyQuestionPartAnswersRepository>();
        services.AddScoped<IKeyQuestionPartAnswersSqlQueries, KeyQuestionPartAnswersSqlQueries>();
        services.AddScoped<IAnswerHighlightsRepository, AnswerHighlightsRepository>();
        services.AddScoped<IAnswerHighlightsSqlQueries, AnswerHighlightsSqlQueries>();

        // -- User Service Utilities
        services.AddScoped<IUserProfilePresenter, UserProfilePresenter>();
        services.AddScoped<IUserIdPresenter, UserIdPresenter>();
        services.AddScoped<IUsersServiceConfigurationPresenter, UsersServiceConfigurationPresenter>();

        // -- Esda Information Utilities
        services.AddScoped<IEsdaInformationPresenter, EsdaInformationPresenter>();
        services.AddScoped<IDataAssetPresenter, DataAssetPresenter>();
        services.AddScoped<IDataAssetInformationServiceConfigurationPresenter, DataAssetInformationServiceConfigurationPresenter>();

        // -- Notification Sending
        services.AddScoped<IDataShareRequestNotificationRecipientDetermination, DataShareRequestNotificationRecipientDetermination>();
        services.AddScoped<INotificationsConfigurationPresenter, NotificationsConfigurationPresenter>();
        services.AddScoped<INotificationConfiguration, NotificationConfiguration>();
        services.AddScoped<INotificationTemplateIdSet, NotificationTemplateIdSet>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<INotificationClientProxyFactory, NotificationClientProxyFactory>();
        services.AddScoped<INotificationBuilder, NotificationBuilder>();

        // -- Submission Content File Building
        services.AddScoped<ISubmissionContentPdfFileBuilder, SubmissionContentPdfFileBuilder>();

        // -- General Config access
        services.AddScoped<IPageLinksConfigurationPresenter, PageLinksConfigurationPresenter>();
        services.AddScoped<IInputConstraintConfigurationPresenter, InputConstraintConfigurationPresenter>();

        services.RegisterValidationRules();

        return services;
    }
}