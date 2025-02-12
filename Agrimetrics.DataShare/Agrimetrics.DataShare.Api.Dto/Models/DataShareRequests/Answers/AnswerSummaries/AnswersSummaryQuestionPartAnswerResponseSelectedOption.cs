namespace Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;

public class DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption
{
    public int OrderWithinAnswerPart { get; set; }

    public string SelectionOptionText { get; set; } = string.Empty;

    public string? SupplementaryAnswerText { get; set; }
}