using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;

public class QuestionPartResponseDataModel
{
    public QuestionPartResponseInputType QuestionPartResponse_ResponseInputType { get; set; }

    public Guid? QuestionPartResponse_AnswerPartResponseId { get; set; }
}