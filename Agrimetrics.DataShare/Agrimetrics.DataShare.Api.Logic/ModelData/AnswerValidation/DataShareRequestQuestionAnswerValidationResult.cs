using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;

internal class DataShareRequestQuestionAnswerValidationResult : IDataShareRequestQuestionAnswerValidationResult
{
    public bool AnswerIsValid => ValidationErrors?.Any() != true;

    public required IEnumerable<SetDataShareRequestQuestionAnswerPartResponseValidationError> ValidationErrors { get; init; }
}