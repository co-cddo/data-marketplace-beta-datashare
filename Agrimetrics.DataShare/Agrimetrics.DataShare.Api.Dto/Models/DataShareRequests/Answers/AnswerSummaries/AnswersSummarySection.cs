namespace Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;

public class DataShareRequestAnswersSummarySection
{
    public int OrderWithinSummary { get; set; }

    public string SectionHeader { get; set; } = string.Empty;

    public List<DataShareRequestAnswersSummaryQuestionGroup> SummaryQuestionGroups { get; set; } = [];
}