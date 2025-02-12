namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions;

public class DataShareRequestQuestionModelData
{
    public Guid DataShareRequestQuestion_DataShareRequestId { get; set; }

    public string DataShareRequestQuestion_DataShareRequestRequestId { get; set; } = string.Empty;

    public Guid DataShareRequestQuestion_QuestionId { get; set; }

    public bool DataShareRequestQuestion_IsOptional { get; set; }

    public List<DataShareRequestQuestionPartModelData> DataShareRequestQuestion_QuestionParts { get; set; } = [];

    public DataShareRequestQuestionFooterModelData? DataShareRequestQuestion_QuestionFooter { get; set; }
}