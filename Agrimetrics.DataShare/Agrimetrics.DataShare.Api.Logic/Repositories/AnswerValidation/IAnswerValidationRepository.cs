using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.AnswerValidation
{
    public interface IAnswerValidationRepository
    {
        Task<QuestionPartAnswerValidationRuleSetModelData> GetQuestionPartAnswerValidationRulesAsync(
            Guid dataShareRequestId,
            Guid questionPartId);
    }
}
