namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswerResponses;

public class DataShareRequestQuestionAnswerPartWriteModelData
{
    public required Guid QuestionPartId { get; init; }

    public required List<DataShareRequestQuestionAnswerPartResponseWriteModelData> AnswerPartResponses { get; init; }
}