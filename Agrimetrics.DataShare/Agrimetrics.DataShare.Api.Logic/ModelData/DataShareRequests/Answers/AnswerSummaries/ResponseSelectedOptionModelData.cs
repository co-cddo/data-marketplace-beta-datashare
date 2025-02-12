namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;

public class DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData
{
    public Guid DataShareRequestAnswersSummaryQuestionPartAnswerItemOptionSelectionItem_ItemId { get; set; }

    public int DataShareRequestAnswersSummaryQuestionPartAnswerItemOptionSelectionItem_OrderWithinAnswerPartResponse { get; set; }

    public string DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption_SelectionOptionText { get; set; } = string.Empty;

    public string? DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption_SupplementaryAnswerText { get; set; }
}