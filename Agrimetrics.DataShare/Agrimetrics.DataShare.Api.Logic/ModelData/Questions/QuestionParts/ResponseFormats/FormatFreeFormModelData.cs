using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.FreeFormItems;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

public abstract class QuestionPartResponseFormatFreeFormModelData : QuestionPartResponseFormatModelData
{
    public override QuestionPartResponseInputType InputType { get; set; } = QuestionPartResponseInputType.FreeForm;

    public QuestionPartResponseFormatFreeFormOptionsModelData? FreeFormOptions { get; set; }
}