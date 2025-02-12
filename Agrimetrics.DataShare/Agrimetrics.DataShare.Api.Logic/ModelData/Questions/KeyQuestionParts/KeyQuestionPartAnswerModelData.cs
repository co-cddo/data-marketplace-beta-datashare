using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions.KeyQuestionParts;

public class KeyQuestionPartAnswerModelData
{
    public Guid KeyQuestionPartAnswer_QuestionSetId { get; set; }

    public Guid KeyQuestionPartAnswer_QuestionPartId { get; set; }

    public bool KeyQuestionPartAnswer_AllowMultipleResponses { get; set; }

    public QuestionPartResponseInputType KeyQuestionPartAnswer_ResponseFormatType { get; set; }

    public Guid KeyQuestionPartAnswer_AnswerPartId { get; set; }

    public List<KeyQuestionPartAnswerResponseModelData> KeyQuestionPartAnswer_AnswerPartResponses { get; set; } = [];
}