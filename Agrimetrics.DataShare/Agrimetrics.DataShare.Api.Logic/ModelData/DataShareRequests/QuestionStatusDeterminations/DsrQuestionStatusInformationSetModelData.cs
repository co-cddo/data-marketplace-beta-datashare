namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations
{
    public class DataShareRequestQuestionStatusInformationSetModelData
    {
        public Guid DataShareRequestId { get; set; }

        public List<DataShareRequestQuestionStatusInformationModelData> DataShareRequestQuestionStatuses { get; set; } = [];
    }
}
