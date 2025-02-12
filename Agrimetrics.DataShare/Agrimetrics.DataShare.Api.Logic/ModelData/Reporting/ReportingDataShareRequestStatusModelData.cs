using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.Reporting;

public class ReportingDataShareRequestStatusModelData
{
    public Guid Status_Id { get; set; }

    public int Status_Order { get; set; }

    public DataShareRequestStatusType Status_Status { get; set; }

    public DateTime Status_EnteredAtUtc { get; set; }

    public DateTime? Status_LeftAtUtc { get; set; }
}