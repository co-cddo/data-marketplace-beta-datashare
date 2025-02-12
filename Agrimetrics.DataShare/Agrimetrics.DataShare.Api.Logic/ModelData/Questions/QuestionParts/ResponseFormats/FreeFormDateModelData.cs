namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

public class QuestionPartResponseFormatFreeFormDateModelData : QuestionPartResponseFormatFreeFormModelData
{
    public override QuestionPartResponseFormatType FormatType { get; set; } = QuestionPartResponseFormatType.Date;
}