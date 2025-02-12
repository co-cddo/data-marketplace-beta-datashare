using Agrimetrics.DataShare.Api.Dto.Requests.AuditLogs;
using Agrimetrics.DataShare.Api.Dto.Responses.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;

namespace Agrimetrics.DataShare.Api.Controllers.AuditLogs;

public interface IAuditLogResponseFactory
{
    GetDataShareRequestAuditLogResponse CreateGetDataShareRequestAuditLogResponse(
        GetDataShareRequestAuditLogRequest getDataShareRequestAuditLogRequest,
        IAuditLogDataShareRequestStatusChangesResult auditLogDataShareRequestStatusChangesResult);
}