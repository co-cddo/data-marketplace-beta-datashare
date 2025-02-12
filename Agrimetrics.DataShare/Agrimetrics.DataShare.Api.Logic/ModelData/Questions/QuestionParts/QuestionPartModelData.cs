using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts;

public class QuestionPartModelData
{
    public Guid QuestionPart_Id { get; set; }

    public int QuestionPart_QuestionPartOrderWithinQuestion { get; set; }

    public QuestionPartType QuestionPart_QuestionPartType { get; set; }

    public QuestionPartPromptsModelData QuestionPart_Prompts { get; set; }

    public QuestionPartMultipleAnswerItemControlModelData QuestionPart_MultipleAnswerItemControl { get; set; }

    public QuestionPartResponseTypeInformationModelData QuestionPart_ResponseTypeInformation { get; set; }

    public QuestionPartResponseFormatModelData QuestionPart_ResponseFormat { get; set; }
}