namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts;

public class QuestionPartMultipleAnswerItemControlModelData
{
    public Guid QuestionPartMultipleAnswerItemControl_QuestionPartId { get; set; }

    public bool QuestionPartMultipleAnswerItemControl_MultipleAnswerItemsAreAllowed { get; set; }

    public string? QuestionPartMultipleAnswerItemControl_ItemDescription { get; set; }

    public string? QuestionPartMultipleAnswerItemControl_CollectionDescription { get; set; }
}