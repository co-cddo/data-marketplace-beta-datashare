namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;

public class DataShareRequestModelData
{
    public Guid DataShareRequest_Id { get; set; }

    public string DataShareRequest_RequestId { get; set; } = string.Empty;

    public int DataShareRequest_AcquirerUserId { get; set; }

    public int DataShareRequest_AcquirerDomainId { get; set; }

    public int DataShareRequest_AcquirerOrganisationId { get; set; }

    public int DataShareRequest_SupplierOrganisationId { get; set; }

    public Guid DataShareRequest_EsdaId { get; set; }

    public string DataShareRequest_EsdaName { get; set; } = string.Empty;

    public DataShareRequestStatusType DataShareRequest_RequestStatus { get; set; }

    public bool DataShareRequest_QuestionsRemainThatRequireAResponse { get; set; }

    public Guid DataShareRequest_QuestionSetId { get; set; }
}