using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.Reporting;

public class ReportingDataShareRequestInformationModelData
{
    public Guid DataShareRequest_Id { get; set; }

    public string DataShareRequest_RequestId { get; set; } = string.Empty;

    public DataShareRequestStatusType DataShareRequest_CurrentStatus { get; set; }

    public int DataShareRequest_PublisherOrganisationId { get; set; }

    public int DataShareRequest_PublisherDomainId { get; set; }

    public List<ReportingDataShareRequestStatusModelData> DataShareRequest_Statuses { get; set; } = [];
}