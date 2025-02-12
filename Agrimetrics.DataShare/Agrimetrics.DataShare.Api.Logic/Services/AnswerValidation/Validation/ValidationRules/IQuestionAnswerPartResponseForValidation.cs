using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;

namespace Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules;

public interface IQuestionAnswerPartResponseForValidation
{
    DataShareRequestQuestionAnswerPartResponseBase QuestionAnswerPartResponse { get; }

    bool QuestionAnswerPartIsOptional { get; }
}