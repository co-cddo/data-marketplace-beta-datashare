using Agrimetrics.DataShare.Api.Dto.Requests.AuditLogs;
using Agrimetrics.DataShare.Api.Dto.Responses.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;

namespace Agrimetrics.DataShare.Api.Controllers.AuditLogs;

internal class AuditLogResponseFactory : IAuditLogResponseFactory
{
    GetDataShareRequestAuditLogResponse IAuditLogResponseFactory.CreateGetDataShareRequestAuditLogResponse(
        GetDataShareRequestAuditLogRequest getDataShareRequestAuditLogRequest,
        IAuditLogDataShareRequestStatusChangesResult auditLogDataShareRequestStatusChangesResult)
    {
        ArgumentNullException.ThrowIfNull(getDataShareRequestAuditLogRequest);
        ArgumentNullException.ThrowIfNull(auditLogDataShareRequestStatusChangesResult);

        return new GetDataShareRequestAuditLogResponse
        {
            DataShareRequestId = getDataShareRequestAuditLogRequest.DataShareRequestId,
            ToStatuses = getDataShareRequestAuditLogRequest.ToStatuses,
            DataShareRequestAuditLog = auditLogDataShareRequestStatusChangesResult.DataShareRequestAuditLog
        };

    }
}