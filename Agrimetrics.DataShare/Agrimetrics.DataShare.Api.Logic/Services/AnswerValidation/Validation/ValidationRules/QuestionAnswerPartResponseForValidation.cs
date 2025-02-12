using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;

namespace Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules;

internal class QuestionAnswerPartResponseForValidation : IQuestionAnswerPartResponseForValidation
{
    public required DataShareRequestQuestionAnswerPartResponseBase QuestionAnswerPartResponse { get; init; }

    public required bool QuestionAnswerPartIsOptional { get; init; }
}