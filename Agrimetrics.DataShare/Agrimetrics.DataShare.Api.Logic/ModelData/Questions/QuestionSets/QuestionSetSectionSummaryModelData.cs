namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionSets;

public class QuestionSetSectionSummaryModelData
{
    public Guid QuestionSetSection_Id { get; set; }

    public int QuestionSetSection_Number { get; set; }

    public string QuestionSetSection_Header { get; set; } = string.Empty;

    public bool QuestionSetSection_IsComplete { get; set; }

    public List<QuestionSummaryModelData> QuestionSetSection_QuestionSummaries { get; set; } = [];
}