namespace Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules.SystemValidationRules;

internal class QuestionAnswerPartValidationErrorSet : IQuestionAnswerPartValidationErrorSet
{
    public required IEnumerable<IQuestionAnswerPartResponseValidationErrorSet> ResponseValidationErrors { get; init; }
}