using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;

namespace Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules.SystemValidationRules;

public class FreeFormMultipleResponseHasNonEmptyDuplicatesValidationRule : IFreeFormMultipleResponseHasNonEmptyDuplicatesValidationRule
{
    bool ISystemValidationRule.Accepts(DataShareRequestQuestionAnswerPart questionAnswerPart)
    {
        ArgumentNullException.ThrowIfNull(questionAnswerPart);

        // Answer parts always have the same input type, so make the decision based on the first response
        var answerPartFirstResponse = questionAnswerPart.AnswerPartResponses.FirstOrDefault();
        return answerPartFirstResponse is {InputType: QuestionPartResponseInputType.FreeForm, MultipleResponsesAreAllowed: true};
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
            var freeFormResponses = questionAnswerPart.AnswerPartResponses.Select(questionAnswerPartResponse =>
            {
                if (questionAnswerPartResponse is not DataShareRequestQuestionAnswerPartResponseFreeForm responseFreeForm)
                    throw new InvalidOperationException("Unable to validate whether FreeForm Multi Response Has Non Empty Duplicates for response of incorrect type");

                return responseFreeForm;
            });

            var nonEmptyFreeFormResponses = freeFormResponses.Where(x =>
                !string.IsNullOrWhiteSpace(x.EnteredValue));

            var responsesGroupedByEnteredValue = nonEmptyFreeFormResponses.GroupBy(response => response.EnteredValue.Trim().ToLower());

            var responsesWithDuplicatedEnteredValues = responsesGroupedByEnteredValue
                .Where(x => x.Count() > 1)
                .SelectMany(x => x.ToList());

            foreach (var responseWithDuplicatedEnteredValue in responsesWithDuplicatedEnteredValues)
            {
                yield return new QuestionAnswerPartResponseValidationErrorSet
                {
                    ResponseOrderWithinAnswerPart = responseWithDuplicatedEnteredValue.OrderWithinAnswerPart,
                    ValidationErrors = ["This value has been entered more than once"]
                };
            }
        }
    }
}