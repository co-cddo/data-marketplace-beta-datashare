using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionSets;

namespace Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests
{
    public class DataShareRequestQuestionsSummary
    {
        public Guid DataShareRequestId { get; set; }

        public string DataShareRequestRequestId { get; set; } = string.Empty;

        public string EsdaName { get; set; } = string.Empty;

        public QuestionSetSummary QuestionSetSummary { get; set; }
    }
}
