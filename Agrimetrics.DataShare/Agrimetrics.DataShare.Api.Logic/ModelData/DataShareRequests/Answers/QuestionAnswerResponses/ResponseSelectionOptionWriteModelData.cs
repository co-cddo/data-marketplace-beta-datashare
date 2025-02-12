namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswerResponses;

public class DataShareRequestQuestionAnswerPartResponseSelectionOptionWriteModelData
{
    public required Guid OptionSelectionId { get; init; }

    public required DataShareRequestQuestionAnswerPartWriteModelData? SupplementaryQuestionAnswerPart { get; init; }
}