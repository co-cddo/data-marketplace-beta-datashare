namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;

public class DataShareRequestAnswersSummaryQuestionGroupModelData
{
    public Guid DataShareRequestAnswersSummaryQuestionGroup_MainQuestionId { get; set; }

    public int DataShareRequestAnswersSummaryQuestionGroup_OrderWithinSection { get; set; }

    public List<Guid> DataShareRequestAnswersSummaryQuestionGroup_BackingQuestionIds { get; set; } = [];

    public DataShareRequestAnswersSummaryQuestionModelData DataShareRequestAnswersSummaryQuestionGroup_SummaryMainQuestion { get; set; }

    public List<DataShareRequestAnswersSummaryQuestionModelData> DataShareRequestAnswersSummaryQuestionGroup_SummaryBackingQuestions { get; set; } = [];
}