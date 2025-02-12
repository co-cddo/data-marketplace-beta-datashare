namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;

public class DataShareRequestAnswersSummaryQuestionPartResponseModelData
{
    public Guid DataShareRequestAnswersSummaryQuestionPartResponse_ResponseId { get; set; }

    public int DataShareRequestAnswersSummaryQuestionPartResponse_OrderWithinQuestionPart { get; set; }

    public DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemModelData? DataShareRequestAnswersSummaryQuestionPart_ResponseItem { get; set; }
}