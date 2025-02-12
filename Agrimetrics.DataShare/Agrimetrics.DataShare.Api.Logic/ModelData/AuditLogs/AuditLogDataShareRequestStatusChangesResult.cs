using Agrimetrics.DataShare.Api.Dto.Models.AuditLogs;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;

internal class AuditLogDataShareRequestStatusChangesResult : IAuditLogDataShareRequestStatusChangesResult
{
    public required DataShareRequestAuditLog DataShareRequestAuditLog { get; init; }
}