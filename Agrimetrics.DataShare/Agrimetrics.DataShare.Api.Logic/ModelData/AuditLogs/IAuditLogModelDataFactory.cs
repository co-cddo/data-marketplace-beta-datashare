using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs
{
    public interface IAuditLogModelDataFactory
    {
        IAuditLogDataShareRequestStatusChangesResult ConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult(
            Guid dataShareRequestId,
            IEnumerable<AuditLogDataShareRequestStatusChangeModelData> auditLogsForDataShareRequestStatusChangesModelDatas);

        DataShareRequestStatusType ConvertDataShareRequestStatusToDataShareRequestStatusType(
            DataShareRequestStatus dataShareRequestStatus);
    }
}
