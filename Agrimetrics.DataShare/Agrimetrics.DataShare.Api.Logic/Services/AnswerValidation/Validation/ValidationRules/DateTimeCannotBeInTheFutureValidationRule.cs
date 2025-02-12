using Agrimetrics.DataShare.Api.Core.SystemProxies;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;

namespace Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules;

public class DateTimeCannotBeInTheFutureValidationRule(
    IResponseFormatter responseFormatter,
    IClock clock) : IValidationRule
{
    bool IValidationRule.Accepts(QuestionPartAnswerValidationRuleModelData validationRule)
    {
        ArgumentNullException.ThrowIfNull(validationRule);

        return validationRule.QuestionPartAnswerValidationRule_Rule is QuestionPartAnswerValidationRuleId.FreeForm_DateTime_DateTimeCannotBeInTheFuture;
    }

    bool IValidationRule.ResponseFailsValidation(
        IQuestionAnswerPartResponseForValidation questionAnswerPartResponseForValidation)
    {
        ArgumentNullException.ThrowIfNull(questionAnswerPartResponseForValidation);

        if (!(questionAnswerPartResponseForValidation.QuestionAnswerPartResponse is DataShareRequestQuestionAnswerPartResponseFreeForm responseFreeForm))
        {
            throw new InvalidOperationException($"QuestionAnswerPartResponse is not of expected type for validation rule {GetType().Name}");
        }

        if (!responseFormatter.TryFormatDateTimeResponse(responseFreeForm.EnteredValue, out var responseDateTime))
        {
            // The value is not a valid datetime, but that's not what this rule is checking, so it didn't fail this rule
            return false;
        }

        return responseDateTime > clock.LocalNow;
    }
}