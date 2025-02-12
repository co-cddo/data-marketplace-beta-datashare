using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.AnswerHighlights;

public class QuestionSetSelectionOptionQuestionHighlightModelData
{
    public Guid QuestionSetSelectionOptionQuestionHighlight_Id { get; set; }

    public Guid QuestionSetSelectionOptionQuestionHighlight_QuestionSetId { get; set; }

    public Guid QuestionSetSelectionOptionQuestionHighlight_SelectionOptionId { get; set; }

    public QuestionSetSelectionOptionQuestionHighlightConditionType QuestionSetSelectionOptionQuestionHighlight_HighlightCondition { get; set; }

    public string QuestionSetSelectionOptionQuestionHighlight_ReasonHighlighted { get; set; } = string.Empty;
}