namespace Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules.SystemValidationRules;

internal class QuestionAnswerPartResponseValidationErrorSet : IQuestionAnswerPartResponseValidationErrorSet
{
    public required int ResponseOrderWithinAnswerPart { get; init; }

    public required IEnumerable<string> ValidationErrors { get; init; }
}