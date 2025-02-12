using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswers;

public class QuestionPartAnswerResponseInformationModelData
{
    public Guid QuestionPartAnswerResponse_Id { get; set; }

    public int QuestionPartAnswerItem_OrderWithinAnswerPart { get; set; }

    public QuestionPartResponseInputType QuestionPartAnswerItem_InputType { get; set; }
}