using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.OptionSelectionItems;
using System.Diagnostics.CodeAnalysis;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

public abstract class QuestionPartResponseFormatOptionSelectModelData : QuestionPartResponseFormatModelData
{
    public override QuestionPartResponseInputType InputType { get; set; } = QuestionPartResponseInputType.OptionSelection;

    [ExcludeFromCodeCoverage] // Abstract, tested on derived classes
    public abstract List<QuestionPartOptionSelectionItemModelData> GetResponseFormatOptionSelectOptionSelectionItems();
}