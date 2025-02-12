using Agrimetrics.DataShare.Api.Dto.Models.AuditLogs;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;

public interface IAuditLogDataShareRequestStatusChangesResult
{
    DataShareRequestAuditLog DataShareRequestAuditLog { get; }
}