namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;

public class DataShareRequestAnswersSummaryModelData
{
    public Guid DataShareRequestAnswersSummary_DataShareRequestId { get; set; }

    public string DataShareRequestAnswersSummary_EsdaName { get; set; } = string.Empty;

    public string DataShareRequestAnswersSummary_RequestId { get; set; } = string.Empty;

    public DataShareRequestStatusType DataShareRequestAnswersSummary_RequestStatus { get; set; }

    public bool DataShareRequestAnswersSummary_QuestionsRemainThatRequireAResponse { get; set; }

    public List<DataShareRequestAnswersSummarySectionModelData> DataShareRequestAnswersSummary_SummarySections { get; set; } = [];

    public string? DataShareRequestAnswersSummary_SubmissionResponseFromSupplier { get; set; }

    public string? DataShareRequestAnswersSummary_CancellationReasonsFromAcquirer { get; set; }
}