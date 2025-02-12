using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using Agrimetrics.DataShare.Api.Logic.Configuration;

namespace Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules.SystemValidationRules;

public class OptionSelectionSupplementaryResponseExceedsMaximumLengthValidationRule(
    IInputConstraintConfigurationPresenter inputConstraintConfigurationPresenter) : IOptionSelectionSupplementaryResponseExceedsMaximumLengthValidationRule
{
    bool ISystemValidationRule.Accepts(
        DataShareRequestQuestionAnswerPart questionAnswerPart)
    {
        ArgumentNullException.ThrowIfNull(questionAnswerPart);

        // Answer parts always have the same input type, so make the decision based on the first response
        return questionAnswerPart.AnswerPartResponses.FirstOrDefault()?.InputType == QuestionPartResponseInputType.OptionSelection;
    }

    bool ISystemValidationRule.ResponseFailsValidation(
        DataShareRequestQuestionAnswerPart questionAnswerPart,
        out IQuestionAnswerPartValidationErrorSet validationErrorSet)
    {
        ArgumentNullException.ThrowIfNull(questionAnswerPart);

        var maximumLengthOfSupplementaryTextResponse = inputConstraintConfigurationPresenter.GetMaximumLengthOfSupplementaryTextResponse();

        validationErrorSet = new QuestionAnswerPartValidationErrorSet
        {
            ResponseValidationErrors = ValidateResponses().ToList()
        };
        return validationErrorSet.ResponseValidationErrors.Any();

        IEnumerable<IQuestionAnswerPartResponseValidationErrorSet> ValidateResponses()
        {
            var responseValidationErrorSets = questionAnswerPart.AnswerPartResponses.Select(response =>
                new QuestionAnswerPartResponseValidationErrorSet
                {
                    ResponseOrderWithinAnswerPart = response.OrderWithinAnswerPart,
                    ValidationErrors = ValidateResponse(response)
                });

            return responseValidationErrorSets.Where(x => x.ValidationErrors.Any());

            IEnumerable<string> ValidateResponse(DataShareRequestQuestionAnswerPartResponseBase questionAnswerPartResponse)
            {
                if (questionAnswerPartResponse is not DataShareRequestQuestionAnswerPartResponseSelectionOption responseSelectionOption)
                    throw new InvalidOperationException("Unable to validate whether OptionSelection Supplementary Response Exceeds Maximum Length for response of incorrect type");

                var nonNullSupplementaryQuestionAnswerParts = responseSelectionOption.SelectedOptionItems
                    .Select(selectionOptionItem => selectionOptionItem.SupplementaryQuestionAnswerPart)
                    .OfType<DataShareRequestQuestionAnswerPart>();

                var supplementaryFreeFormAnswerPartResponses = nonNullSupplementaryQuestionAnswerParts
                    .SelectMany(answerPart => answerPart.AnswerPartResponses)
                    .OfType<DataShareRequestQuestionAnswerPartResponseFreeForm>();

                if (supplementaryFreeFormAnswerPartResponses.Any(response =>
                        response.EnteredValue.Length > maximumLengthOfSupplementaryTextResponse))
                {
                    yield return $"Supplementary Answer exceeds maximum length ({maximumLengthOfSupplementaryTextResponse} characters)";
                }
            }
        }
    }
}