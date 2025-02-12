namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;

public class QuestionSetQuestionOutlineModelData
{
    public Guid QuestionSetQuestionOutline_Id { get; set; }

    public int QuestionSetQuestionOutline_OrderWithinSection { get; set; }

    public string QuestionSetQuestionOutline_QuestionText { get; set; } = string.Empty;
}