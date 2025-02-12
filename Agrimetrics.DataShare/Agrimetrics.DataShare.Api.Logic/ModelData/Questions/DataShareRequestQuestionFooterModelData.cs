namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions;

public class DataShareRequestQuestionFooterModelData
{
    public Guid DataShareRequestQuestionFooter_Id { get; set; }

    public string? DataShareRequestQuestionFooter_Header { get; set; }

    public List<DataShareRequestQuestionFooterItemModelData> DataShareRequestQuestionFooter_Items { get; set; } = [];
}