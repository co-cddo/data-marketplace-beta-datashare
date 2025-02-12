namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswers;

public class QuestionPartAnswerItemSelectionOptionItemModelData
{
    public Guid QuestionPartAnswerItem_OptionSelectionItemId { get; set; }

    public Guid? QuestionPartAnswerItem_SupplementaryQuestionPartAnswerId { get; set; }

    public QuestionPartAnswerModelData? QuestionPartAnswerItem_SupplementaryQuestionPartAnswer { get; set; }
}