using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;

public class DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeFormModelData : DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemModelData
{
    public override QuestionPartResponseInputType DataShareRequestAnswersSummaryQuestionPartAnswerResponseItem_ResponseInputType => QuestionPartResponseInputType.FreeForm;

    public string DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm_AnswerValue { get; set; } = string.Empty;

    public bool DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm_ValueEntryDeclined { get; set; }
}