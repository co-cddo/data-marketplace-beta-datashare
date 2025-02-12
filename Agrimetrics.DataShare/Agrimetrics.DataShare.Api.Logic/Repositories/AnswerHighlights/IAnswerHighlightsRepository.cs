using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerHighlights;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.AnswerHighlights
{
    public interface IAnswerHighlightsRepository
    {
        Task<IEnumerable<QuestionSetSelectionOptionQuestionHighlightModelData>> GetQuestionSetSelectionOptionQuestionHighlightModelDataAsync(
            Guid dataShareRequestId);

        Task<DataShareRequestSelectionOptionsModelData> GetDataShareRequestSelectionOptionsModelDataAsync(
            Guid dataShareRequestId);
    }
}
