namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

public class QuestionPartResponseFormatFreeFormTextModelData : QuestionPartResponseFormatFreeFormModelData
{
    public override QuestionPartResponseFormatType FormatType { get; set; } = QuestionPartResponseFormatType.Text;

    public int QuestionPartResponseFormatFreeFormText_MaximumResponseLength { get; set; }
}