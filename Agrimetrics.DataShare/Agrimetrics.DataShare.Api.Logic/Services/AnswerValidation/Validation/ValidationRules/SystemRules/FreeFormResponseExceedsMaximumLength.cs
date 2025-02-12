using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using Agrimetrics.DataShare.Api.Logic.Configuration;

namespace Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules.SystemValidationRules;

public class FreeFormResponseExceedsMaximumLengthValidationRule(
    IInputConstraintConfigurationPresenter inputConstraintConfigurationPresenter) : IFreeFormResponseExceedsMaximumLengthValidationRule
{
    bool ISystemValidationRule.Accepts(
        DataShareRequestQuestionAnswerPart questionAnswerPart)
    {
        ArgumentNullException.ThrowIfNull(questionAnswerPart);

        // Answer parts always have the same input type, so make the decision based on the first response
        return questionAnswerPart.AnswerPartResponses.FirstOrDefault()?.InputType == QuestionPartResponseInputType.FreeForm;
    }

    bool ISystemValidationRule.ResponseFailsValidation(
        DataShareRequestQuestionAnswerPart questionAnswerPart,
        out IQuestionAnswerPartValidationErrorSet validationErrorSet)
    {
        ArgumentNullException.ThrowIfNull(questionAnswerPart);

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

            IEnumerable<string> ValidateResponse(
                DataShareRequestQuestionAnswerPartResponseBase questionAnswerPartResponse)
            {
                if (questionAnswerPartResponse is not DataShareRequestQuestionAnswerPartResponseFreeForm responseFreeForm) 
                    throw new InvalidOperationException("Unable to validate whether FreeForm Response Exceeds Maximum Length for response of incorrect type");

                var maximumLengthOfFreeFormTextResponse = questionAnswerPartResponse.MultipleResponsesAreAllowed
                    ? inputConstraintConfigurationPresenter.GetMaximumLengthOfFreeFormMultiResponseTextResponse()
                    : inputConstraintConfigurationPresenter.GetMaximumLengthOfFreeFormTextResponse();

                if (responseFreeForm.EnteredValue.Length > maximumLengthOfFreeFormTextResponse)
                {
                    yield return $"Value exceeds maximum length ({maximumLengthOfFreeFormTextResponse} characters)";
                }
            }
        }
    }
}