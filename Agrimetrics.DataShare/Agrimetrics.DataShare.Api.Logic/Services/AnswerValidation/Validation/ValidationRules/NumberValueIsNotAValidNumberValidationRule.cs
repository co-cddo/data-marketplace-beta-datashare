using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;

namespace Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules;

public class NumberValueIsNotAValidNumberValidationRule(
    IResponseFormatter responseFormatter) : IValidationRule
{
    bool IValidationRule.Accepts(QuestionPartAnswerValidationRuleModelData validationRule)
    {
        ArgumentNullException.ThrowIfNull(validationRule);

        return validationRule.QuestionPartAnswerValidationRule_Rule is QuestionPartAnswerValidationRuleId.FreeForm_Number_NotAValidNumber;
    }

    bool IValidationRule.ResponseFailsValidation(
        IQuestionAnswerPartResponseForValidation questionAnswerPartResponseForValidation)
    {
        ArgumentNullException.ThrowIfNull(questionAnswerPartResponseForValidation);

        if (!(questionAnswerPartResponseForValidation.QuestionAnswerPartResponse is DataShareRequestQuestionAnswerPartResponseFreeForm responseFreeForm))
        {
            throw new InvalidOperationException($"QuestionAnswerPartResponse is not of expected type for validation rule {GetType().Name}");
        }

        var enteredValue = responseFreeForm.EnteredValue;
        if (string.IsNullOrWhiteSpace(enteredValue))
        {
            // No value was supplied, but that's not what this rule is checking, so it didn't fail this rule
            return false;
        }

        return !responseFormatter.TryFormatNumericResponse(enteredValue, out _);
    }
}