using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswers;

public class QuestionPartAnswerResponseItemFreeFormModelData : QuestionPartAnswerResponseItemModelData
{
    public override QuestionPartResponseInputType QuestionPartAnswerItem_InputType { get; set; } = QuestionPartResponseInputType.FreeForm;

    public string QuestionPartAnswerItemFreeForm_EnteredValue { get; set; } = string.Empty;

    public bool QuestionPartAnswerItemFreeForm_ValueEntryDeclined { get; set; }
}