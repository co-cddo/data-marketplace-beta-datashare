using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;

namespace Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation;

public interface IDataShareRequestQuestionAnswerValidationService
{
    Task<IDataShareRequestQuestionAnswerValidationResult> ValidateDataShareRequestQuestionAnswerAsync(
        DataShareRequestQuestionAnswer dataShareRequestQuestionAnswer);
}