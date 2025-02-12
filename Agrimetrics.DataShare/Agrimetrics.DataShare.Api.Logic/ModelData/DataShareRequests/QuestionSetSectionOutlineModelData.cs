namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;

public class QuestionSetSectionOutlineModelData
{
    public Guid QuestionSetSectionOutline_Id { get; set; }

    public int QuestionSetSectionOutline_OrderWithinQuestionSetOutline { get; set; }

    public string QuestionSetSectionOutline_SectionHeader { get; set; } = string.Empty;

    public List<QuestionSetQuestionOutlineModelData> Questions { get; set; } = [];
}