namespace Agrimetrics.DataShare.Api.Logic.Repositories.AuditLogs;

public interface IAuditLogSqlQueries
{
    string RecordDataShareRequestStatusChange { get; }

    string RecordDataShareRequestStatusChangeComment { get; }

    string GetDataShareRequestStatusChanges { get; }
}