namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;

public class DataShareRequestAnswersSummaryQuestionModelData
{
    public Guid DataShareRequestAnswersSummaryQuestion_QuestionId { get; set; }

    public string DataShareRequestAnswersSummaryQuestion_QuestionHeader { get; set; } = string.Empty;

    public bool DataShareRequestAnswersSummaryQuestion_QuestionIsApplicable { get; set; }

    public List<DataShareRequestAnswersSummaryQuestionQuestionPartIdModelData> DataShareRequestAnswersSummaryQuestion_QuestionPartIds { get; set; } = [];
    
    public List<DataShareRequestAnswersSummaryQuestionPartModelData> DataShareRequestAnswersSummaryQuestion_QuestionParts { get; set; } = [];
}