namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;

public class DataShareRequestAnswersSummarySectionModelData
{
    public Guid DataShareRequestAnswersSummarySection_SectionId { get; set; }

    public int DataShareRequestAnswersSummarySection_OrderWithinSummary { get; set; }

    public string DataShareRequestAnswersSummarySection_SectionHeader { get; set; } = string.Empty;

    public List<DataShareRequestAnswersSummaryQuestionGroupModelData> DataShareRequestAnswersSummarySection_QuestionGroups { get; set; } = [];
}