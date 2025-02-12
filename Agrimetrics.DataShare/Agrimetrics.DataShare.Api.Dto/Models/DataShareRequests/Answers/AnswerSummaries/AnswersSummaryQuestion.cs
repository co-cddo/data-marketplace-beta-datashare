namespace Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;

public class DataShareRequestAnswersSummaryQuestion
{
    public Guid QuestionId { get; set; }

    public string QuestionHeader { get; set; } = string.Empty;

    public bool QuestionIsApplicable { get; set; }

    public List<DataShareRequestAnswersSummaryQuestionPart> SummaryQuestionParts { get; set; } = [];
}