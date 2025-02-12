namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

public abstract class QuestionPartResponseFormatNoInputModelData : QuestionPartResponseFormatModelData
{
    public override QuestionPartResponseInputType InputType { get; set; } = QuestionPartResponseInputType.None;
}