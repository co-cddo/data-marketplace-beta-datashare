namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions.KeyQuestionParts;

public class KeyQuestionPartAnswerResponseItemOptionSelectionModelData : KeyQuestionPartAnswerResponseItemModelData
{
    public Guid KeyQuestionPartAnswerResponseItemOptionSelection_ResponseItemId { get; set; }

    public List<KeyQuestionPartAnswerResponseItemSelectedOptionModelData> KeyQuestionPartAnswerResponseItemOptionSelection_SelectedOptions { get; set; } = [];
}