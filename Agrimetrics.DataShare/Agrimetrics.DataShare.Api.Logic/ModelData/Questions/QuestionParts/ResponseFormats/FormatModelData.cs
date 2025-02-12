namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

public abstract class QuestionPartResponseFormatModelData
{
    public virtual QuestionPartResponseInputType InputType { get; set; }

    public virtual QuestionPartResponseFormatType FormatType { get; set; }
}