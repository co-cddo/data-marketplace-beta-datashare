using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerHighlights;

namespace Agrimetrics.DataShare.Api.Logic.Services.AnswerHighlights
{
    public interface IAnswerHighlightsService
    {
        Task<IEnumerable<string>> GetDataShareRequestsAnswerHighlightsAsync(
            Guid dataShareRequestId);
    }
}
