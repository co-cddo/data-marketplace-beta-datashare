using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;

namespace Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules;

public class SelectMultiNoOptionHasBeenSelectedValidationRule : IValidationRule
{
    bool IValidationRule.Accepts(QuestionPartAnswerValidationRuleModelData validationRule)
    {
        ArgumentNullException.ThrowIfNull(validationRule);

        return validationRule.QuestionPartAnswerValidationRule_Rule is QuestionPartAnswerValidationRuleId.OptionSelection_SelectMulti_NoOptionIsSelected;
    }

    bool IValidationRule.ResponseFailsValidation(
        IQuestionAnswerPartResponseForValidation questionAnswerPartResponseForValidation)
    {
        ArgumentNullException.ThrowIfNull(questionAnswerPartResponseForValidation);

        if (!(questionAnswerPartResponseForValidation.QuestionAnswerPartResponse is DataShareRequestQuestionAnswerPartResponseSelectionOption responseSelectionOption))
        {
            throw new InvalidOperationException($"QuestionAnswerPartResponse is not of expected type for validation rule {GetType().Name}");
        }

        return !responseSelectionOption.SelectedOptionItems.Any();
    }
}