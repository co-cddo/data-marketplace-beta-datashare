using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswers;

public abstract class QuestionPartAnswerResponseItemModelData
{
    public virtual QuestionPartResponseInputType QuestionPartAnswerItem_InputType { get; set; }

    public Guid QuestionPartAnswerItem_Id { get; set; }
}