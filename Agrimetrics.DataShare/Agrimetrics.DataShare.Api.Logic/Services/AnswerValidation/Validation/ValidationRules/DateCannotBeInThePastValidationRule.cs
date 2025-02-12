using Agrimetrics.DataShare.Api.Core.SystemProxies;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;

namespace Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules;

public class DateCannotBeInThePastValidationRule(
    IResponseFormatter responseFormatter,
    IClock clock) : IValidationRule
{
    bool IValidationRule.Accepts(QuestionPartAnswerValidationRuleModelData validationRule)
    {
        ArgumentNullException.ThrowIfNull(validationRule);

        return validationRule.QuestionPartAnswerValidationRule_Rule is QuestionPartAnswerValidationRuleId.FreeForm_Date_DateCannotBeInThePast;
    }

    bool IValidationRule.ResponseFailsValidation(
        IQuestionAnswerPartResponseForValidation questionAnswerPartResponseForValidation)
    {
        ArgumentNullException.ThrowIfNull(questionAnswerPartResponseForValidation);

        if (!(questionAnswerPartResponseForValidation.QuestionAnswerPartResponse is DataShareRequestQuestionAnswerPartResponseFreeForm responseFreeForm))
        {
            throw new InvalidOperationException($"QuestionAnswerPartResponse is not of expected type for validation rule {GetType().Name}");
        }

        if (!responseFormatter.TryFormatDateResponse(responseFreeForm.EnteredValue, out var responseDate))
        {
            // The value is not a valid date, but that's not what this rule is checking, so it didn't fail this rule
            return false;
        }

        return responseDate < clock.LocalNow.Date;
    }
}