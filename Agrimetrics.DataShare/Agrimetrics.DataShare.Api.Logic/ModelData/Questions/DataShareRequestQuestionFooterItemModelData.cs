namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions;

public class DataShareRequestQuestionFooterItemModelData
{
    public Guid DataShareRequestQuestionFooterItem_Id { get; set; }

    public Guid DataShareRequestQuestionFooterItem_FooterId { get; set; }

    public string DataShareRequestQuestionFooterItem_Text { get; set; } = string.Empty;

    public int DataShareRequestQuestionFooterItem_OrderWithinFooter { get; set; }
}