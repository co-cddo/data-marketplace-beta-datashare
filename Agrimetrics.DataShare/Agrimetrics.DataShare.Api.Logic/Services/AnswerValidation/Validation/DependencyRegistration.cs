using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules.SystemValidationRules;
using Microsoft.Extensions.DependencyInjection;

namespace Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation;

internal static class DependencyRegistration
{
    public static IServiceCollection RegisterValidationRules(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<IValidationRule, DateCannotBeInTheFutureValidationRule>();
        services.AddScoped<IValidationRule, DateCannotBeInThePastValidationRule>();
        services.AddScoped<IValidationRule, DateTimeCannotBeInTheFutureValidationRule>();
        services.AddScoped<IValidationRule, DateTimeCannotBeInThePastValidationRule>();
        services.AddScoped<IValidationRule, DateTimeValueHasNotBeenSuppliedValidationRule>();
        services.AddScoped<IValidationRule, DateTimeValueIsNotAValidDateTimeValidationRule>();
        services.AddScoped<IValidationRule, DateValueHasNotBeenSuppliedValidationRule>();
        services.AddScoped<IValidationRule, DateValueIsNotAValidDateValidationRule>();
        services.AddScoped<IValidationRule, NumberValueHasNotBeenSuppliedValidationRule>();
        services.AddScoped<IValidationRule, NumberValueIsNotAValidNumberValidationRule>();
        services.AddScoped<IValidationRule, SelectMultiNoOptionHasBeenSelectedValidationRule>();
        services.AddScoped<IValidationRule, SelectSingleNoOptionHasBeenSelectedValidationRule>();
        services.AddScoped<IValidationRule, TextValueHasNotBeenSuppliedValidationRule>();
        services.AddScoped<IValidationRule, TimeValueHasNotBeenSuppliedValidationRule>();
        services.AddScoped<IValidationRule, TimeValueIsNotAValidTimeValidationRule>();
        services.AddScoped<IValidationRule, CountryValueHasNotBeenSuppliedValidationRule>();

        services.AddScoped<ISystemValidationRule, FreeFormResponseExceedsMaximumLengthValidationRule>();
        services.AddScoped<ISystemValidationRule, FreeFormMultipleResponseHasNonEmptyDuplicatesValidationRule>();
        services.AddScoped<ISystemValidationRule, OptionSelectionSupplementaryResponseExceedsMaximumLengthValidationRule>();

        services.AddScoped<IResponseFormatter, ResponseFormatter>();

        return services;
    }
}