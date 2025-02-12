namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionSets;

public class QuestionSetQuestionModelData
{
    public Guid QuestionSet_QuestionId { get; set; }

    public List<QuestionSetQuestionPartModelData> QuestionSet_QuestionParts { get; set; } = [];
}