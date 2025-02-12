namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

public class QuestionPartResponseFormatFreeFormTimeModelData : QuestionPartResponseFormatFreeFormModelData
{
    public override QuestionPartResponseFormatType FormatType { get; set; } = QuestionPartResponseFormatType.Time;
}