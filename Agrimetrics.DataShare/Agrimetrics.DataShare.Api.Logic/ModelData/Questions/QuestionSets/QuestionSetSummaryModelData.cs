namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionSets;

public class QuestionSetSummaryModelData
{
    public Guid QuestionSet_Id { get; set; }

    public bool QuestionSet_AnswersSectionComplete { get; set; }

    public List<QuestionSetSectionSummaryModelData> QuestionSet_SectionSummaries { get; set; } = [];
}
