using Agrimetrics.DataShare.Api.Core.Utilities;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;
using Agrimetrics.DataShare.Api.Logic.Repositories.AnswerValidation;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation;

namespace Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation;

internal class DataShareRequestQuestionAnswerValidationService(
    IAnswerValidationRepository answerValidationRepository,
    IQuestionPartAnswerValidation questionPartAnswerValidation) : IDataShareRequestQuestionAnswerValidationService
{
    async Task<IDataShareRequestQuestionAnswerValidationResult> IDataShareRequestQuestionAnswerValidationService.ValidateDataShareRequestQuestionAnswerAsync(
        DataShareRequestQuestionAnswer dataShareRequestQuestionAnswer)
    {
        ArgumentNullException.ThrowIfNull(dataShareRequestQuestionAnswer);

        var validationErrors = (await dataShareRequestQuestionAnswer.AnswerParts
            .SelectManyAsync(answerPart => GetValidationErrorsForAnswerPartAsync(dataShareRequestQuestionAnswer.DataShareRequestId, answerPart)))
            .ToList();

        return new DataShareRequestQuestionAnswerValidationResult
        {
            ValidationErrors = validationErrors
        };
    }

    private async Task<IEnumerable<SetDataShareRequestQuestionAnswerPartResponseValidationError>> GetValidationErrorsForAnswerPartAsync(
        Guid dataShareRequestId,
        DataShareRequestQuestionAnswerPart answerPart)
    {
        var questionPartAnswerValidationRuleSet = await answerValidationRepository.GetQuestionPartAnswerValidationRulesAsync(
            dataShareRequestId,
            answerPart.QuestionPartId);

        return questionPartAnswerValidation.ValidateQuestionPartAnswer(
            answerPart,
            questionPartAnswerValidationRuleSet);
    }
}