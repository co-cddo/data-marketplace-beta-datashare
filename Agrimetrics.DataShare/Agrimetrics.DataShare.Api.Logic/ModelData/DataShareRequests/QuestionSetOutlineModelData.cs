namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;

public class QuestionSetOutlineModelData
{
    public Guid QuestionSetOutline_Id { get; set; }

    public Guid QuestionSetOutline_EsdaId { get; set; }

    public int QuestionSetOutline_SupplierDomain { get; set; }

    public int QuestionSetOutline_SupplierOrganisation { get; set; }

    public List<QuestionSetSectionOutlineModelData> Sections { get; set; } = [];
    
}