using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswers;

public class QuestionPartAnswerResponseModelData
{
    public Guid QuestionPartAnswerResponse_Id { get; set; }

    public int QuestionPartAnswerResponse_OrderWithinAnswerPart { get; set; }

    public virtual QuestionPartResponseInputType QuestionPartAnswerResponse_InputType { get; set; }

    public QuestionPartAnswerResponseItemModelData? QuestionPartAnswerResponse_ResponseItem { get; set; }
}