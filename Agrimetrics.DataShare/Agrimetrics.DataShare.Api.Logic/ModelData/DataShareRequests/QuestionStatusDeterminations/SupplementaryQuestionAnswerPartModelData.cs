using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;

public class SupplementaryQuestionAnswerPartModelData
{
    public Guid SupplementaryAnswerPart_AnswerPartId { get; set; }

    public QuestionPartResponseInputType SupplementaryAnswerPart_InputType { get; set; }
}