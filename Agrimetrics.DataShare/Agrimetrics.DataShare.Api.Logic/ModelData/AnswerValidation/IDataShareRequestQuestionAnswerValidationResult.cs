using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;

public interface IDataShareRequestQuestionAnswerValidationResult
{
    bool AnswerIsValid { get; }

    IEnumerable<SetDataShareRequestQuestionAnswerPartResponseValidationError> ValidationErrors { get; }
}