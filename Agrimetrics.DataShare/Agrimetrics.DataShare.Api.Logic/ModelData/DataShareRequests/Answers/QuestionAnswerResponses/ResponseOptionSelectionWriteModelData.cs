using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswerResponses;

public class DataShareRequestQuestionAnswerPartResponseOptionSelectionWriteModelData
    : DataShareRequestQuestionAnswerPartResponseWriteModelData
{
    public override QuestionPartResponseInputType InputType { get; } = QuestionPartResponseInputType.OptionSelection;

    public required List<DataShareRequestQuestionAnswerPartResponseSelectionOptionWriteModelData> SelectionOptions { get; init; }
}