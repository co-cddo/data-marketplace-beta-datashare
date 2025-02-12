namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;

public class QuestionSetQuestionApplicabilityOverride
{
    public Guid QuestionSetQuestionApplicabilityOverride_ControlledQuestionId { get; set; }

    public Guid QuestionSetQuestionApplicabilityOverride_ControllingSelectionOptionId { get; set; }

    public QuestionSetQuestionApplicabilityConditionType QuestionSetQuestionApplicabilityOverride_ControlledQuestionApplicabilityCondition { get; set; }

    public bool QuestionSetQuestionApplicabilityOverride_ControllingSelectionOptionIsSelected { get; set; }
}