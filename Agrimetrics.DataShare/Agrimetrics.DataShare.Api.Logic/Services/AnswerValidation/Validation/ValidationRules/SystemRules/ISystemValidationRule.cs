using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;

namespace Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules.SystemValidationRules;

public interface ISystemValidationRule
{
    bool Accepts(
        DataShareRequestQuestionAnswerPart questionAnswerPart);

    bool ResponseFailsValidation(
        DataShareRequestQuestionAnswerPart questionAnswerPart,
        out IQuestionAnswerPartValidationErrorSet validationErrorSet);
}