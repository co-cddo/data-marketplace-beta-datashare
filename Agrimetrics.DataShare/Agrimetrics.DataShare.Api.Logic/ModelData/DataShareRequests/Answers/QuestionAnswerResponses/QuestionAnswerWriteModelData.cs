namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswerResponses
{
    public class DataShareRequestQuestionAnswerWriteModelData
    {
        public required Guid DataShareRequestId { get; init; }

        public required Guid QuestionId { get; init; }

        public required List<DataShareRequestQuestionAnswerPartWriteModelData> AnswerParts { get; init; }
    }
}
