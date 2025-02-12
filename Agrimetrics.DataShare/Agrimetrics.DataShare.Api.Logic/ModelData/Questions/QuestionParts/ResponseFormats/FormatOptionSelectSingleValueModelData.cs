using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.OptionSelectionItems;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

public class QuestionPartResponseFormatOptionSelectSingleValueModelData : QuestionPartResponseFormatOptionSelectModelData
{
    public override QuestionPartResponseFormatType FormatType { get; set; } = QuestionPartResponseFormatType.SelectSingle;

    public List<QuestionPartOptionSelectionItemForSingleSelectionModelData> ResponseFormatOptionSelectSingleValue_SingleSelectionOptions { get; set; } = [];

    public override List<QuestionPartOptionSelectionItemModelData> GetResponseFormatOptionSelectOptionSelectionItems()
    {
        return ResponseFormatOptionSelectSingleValue_SingleSelectionOptions
            .Cast<QuestionPartOptionSelectionItemModelData>()
            .ToList();
    }
}