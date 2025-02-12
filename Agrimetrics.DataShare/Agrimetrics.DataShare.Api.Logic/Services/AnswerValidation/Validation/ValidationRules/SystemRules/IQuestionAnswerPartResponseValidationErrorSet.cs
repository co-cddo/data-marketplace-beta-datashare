namespace Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules.SystemValidationRules;

public interface IQuestionAnswerPartResponseValidationErrorSet
{
    int ResponseOrderWithinAnswerPart { get; }

    IEnumerable<string> ValidationErrors { get; }
}