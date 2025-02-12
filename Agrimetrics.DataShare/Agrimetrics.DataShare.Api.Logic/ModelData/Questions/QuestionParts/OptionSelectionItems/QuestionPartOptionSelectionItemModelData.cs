namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.OptionSelectionItems;

public abstract class QuestionPartOptionSelectionItemModelData
{
    public Guid OptionSelectionItem_Id { get; set; }

    public string OptionSelectionItem_ValueText { get; set; } = string.Empty;

    public string? OptionSelectionItem_HintText { get; set; }

    public int OptionSelectionItem_OptionOrderWithinSelection { get; set; }

    public Guid? OptionSelectionItem_SupplementaryQuestionPartId { get; set; }

    public QuestionPartModelData? OptionSelectionItem_SupplementaryQuestionPart { get; set; }
}