using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswerResponses;

public class DataShareRequestQuestionAnswerPartResponseFreeFormWriteModelData
    : DataShareRequestQuestionAnswerPartResponseWriteModelData
{
    public override QuestionPartResponseInputType InputType { get; } = QuestionPartResponseInputType.FreeForm;

    public required string EnteredValue { get; init; }

    public required bool ValueEntryDeclined { get; init; }
}