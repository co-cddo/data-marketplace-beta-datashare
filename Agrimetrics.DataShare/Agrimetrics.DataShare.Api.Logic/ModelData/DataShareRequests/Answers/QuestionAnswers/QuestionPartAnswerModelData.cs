namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswers;

public class QuestionPartAnswerModelData
{
    public Guid QuestionPartAnswer_Id { get; set; }

    public Guid QuestionPartAnswer_QuestionPartId { get; set; }

    public List<QuestionPartAnswerResponseInformationModelData> QuestionPartAnswer_AnswerPartResponseInformations { get; set; } = [];

    public List<QuestionPartAnswerResponseModelData> QuestionPartAnswer_AnswerPartResponses { get; set; } = [];
}