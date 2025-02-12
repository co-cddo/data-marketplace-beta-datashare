using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswers;

public class QuestionPartAnswerResponseItemOptionSelectionModelData : QuestionPartAnswerResponseItemModelData
{
    public override QuestionPartResponseInputType QuestionPartAnswerItem_InputType { get; set; } = QuestionPartResponseInputType.OptionSelection;

    public List<QuestionPartAnswerItemSelectionOptionItemModelData> QuestionPartAnswerItem_SelectedOptionItems { get; set; } = [];
}