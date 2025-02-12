namespace Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionSets;

public class QuestionSetSectionOutline
{
    public int OrderWithinQuestionSetOutline { get; set; }

    public string Header { get; set; } = string.Empty;

    public List<QuestionSetQuestionOutline> Questions { get; set; } = [];
}