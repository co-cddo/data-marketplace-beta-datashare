using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;

namespace Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation;

public interface IQuestionPartAnswerValidation
{
    IEnumerable<SetDataShareRequestQuestionAnswerPartResponseValidationError> ValidateQuestionPartAnswer(
        DataShareRequestQuestionAnswerPart questionPartAnswer,
        QuestionPartAnswerValidationRuleSetModelData validationRuleSet);
}