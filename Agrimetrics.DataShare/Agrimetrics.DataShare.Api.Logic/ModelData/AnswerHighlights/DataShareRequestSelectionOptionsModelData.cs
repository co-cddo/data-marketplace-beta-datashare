namespace Agrimetrics.DataShare.Api.Logic.ModelData.AnswerHighlights
{
    public class DataShareRequestSelectionOptionsModelData
    {
        public Guid DataShareRequestSelectionOptions_DataShareRequestId { get; set; }

        public List<DataShareRequestSelectedOptionModelData> DataShareRequestSelectionOptions_SelectedOptions { get; set; } = [];
    }
}
