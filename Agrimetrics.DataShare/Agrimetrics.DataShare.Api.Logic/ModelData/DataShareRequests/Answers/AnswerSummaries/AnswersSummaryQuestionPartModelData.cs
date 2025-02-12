using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;

public class DataShareRequestAnswersSummaryQuestionPartModelData
{
    public Guid DataShareRequestAnswersSummaryQuestionPart_QuestionPartId { get; set; }

    public Guid DataShareRequestAnswersSummaryQuestionPart_AnswerPartId { get; set; }

    public int DataShareRequestAnswersSummaryQuestionPart_OrderWithinQuestion { get; set; }

    public string DataShareRequestAnswersSummaryQuestionPart_QuestionPartText { get; set; } = string.Empty;

    public bool DataShareRequestAnswersSummaryQuestionPart_MultipleResponsesAllowed { get; set; }

    public string DataShareRequestAnswersSummaryQuestionPart_MultipleResponsesCollectionDescriptionIfMultipleResponsesAllowed { get; set; } = string.Empty;

    public QuestionPartResponseInputType DataShareRequestAnswersSummaryQuestionPart_ResponseInputType { get; set; }

    public QuestionPartResponseFormatType DataShareRequestAnswersSummaryQuestionPart_ResponseFormatType { get; set; }

    public List<DataShareRequestAnswersSummaryQuestionPartResponseModelData> DataShareRequestAnswersSummaryQuestionPart_Responses { get; set; } = [];
}