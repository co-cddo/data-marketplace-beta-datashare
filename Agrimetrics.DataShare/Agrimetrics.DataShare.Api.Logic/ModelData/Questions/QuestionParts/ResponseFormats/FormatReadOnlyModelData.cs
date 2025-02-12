namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

public class QuestionPartResponseFormatReadOnlyModelData : QuestionPartResponseFormatNoInputModelData
{
    public override QuestionPartResponseFormatType FormatType { get; set; } = QuestionPartResponseFormatType.ReadOnly;
}