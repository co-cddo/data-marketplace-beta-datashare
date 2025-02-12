using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.OptionSelectionItems;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

public class QuestionPartResponseFormatOptionSelectMultiValueModelData : QuestionPartResponseFormatOptionSelectModelData
{
    public override QuestionPartResponseFormatType FormatType { get; set; } = QuestionPartResponseFormatType.SelectMulti;

    public List<QuestionPartOptionSelectionItemForMultiSelectionModelData> ResponseFormatOptionSelectMultiValue_MultiSelectionOptions { get; set; } = [];

    public override List<QuestionPartOptionSelectionItemModelData> GetResponseFormatOptionSelectOptionSelectionItems()
    {
        return ResponseFormatOptionSelectMultiValue_MultiSelectionOptions
            .Cast<QuestionPartOptionSelectionItemModelData>()
            .ToList();
    }
}