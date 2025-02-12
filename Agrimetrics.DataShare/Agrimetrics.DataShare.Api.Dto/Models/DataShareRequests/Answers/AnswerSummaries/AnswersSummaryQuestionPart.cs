using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;

namespace Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;

public class DataShareRequestAnswersSummaryQuestionPart
{
    public int OrderWithinQuestion { get; set; }

    public string QuestionPartText { get; set; } = string.Empty;

    public bool MultipleResponsesAllowed { get; set; }

    public string MultipleResponsesCollectionHeaderIfMultipleResponsesAllowed { get; set; } = string.Empty;

    public QuestionPartResponseInputType ResponseInputType { get; set; }

    public QuestionPartResponseFormatType ResponseFormatType { get; set; }

    public List<DataShareRequestAnswersSummaryQuestionPartAnswerResponse> Responses { get; set; }
}