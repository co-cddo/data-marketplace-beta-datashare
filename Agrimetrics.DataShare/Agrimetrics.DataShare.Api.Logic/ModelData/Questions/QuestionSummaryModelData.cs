namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions;

public class QuestionSummaryModelData
{
    public Guid Question_Id { get; set; }

    public int Question_OrderWithinQuestionSetSection { get; set; }

    public string Question_Header { get; set; } = string.Empty;

    public QuestionStatusType Question_QuestionStatus { get; set; }

    public bool Question_QuestionCanBeAnswered { get; set; }
}