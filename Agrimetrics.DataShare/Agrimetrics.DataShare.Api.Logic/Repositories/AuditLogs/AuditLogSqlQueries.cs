using System.Diagnostics.CodeAnalysis;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.AuditLogs;

[ExcludeFromCodeCoverage] // It makes no sense to write unit tests against SQL statements as they have actual function
internal class AuditLogSqlQueries : IAuditLogSqlQueries
{
    string IAuditLogSqlQueries.RecordDataShareRequestStatusChange =>
      @"INSERT INTO [dbo].[AuditLogDataShareRequestStatusChange]
            (DataShareRequest, FromStatus, ToStatus, ChangedByUser, ChangedByUserDomain, ChangedByUserOrganisation, ChangedAtUtc)
        OUTPUT [inserted].[Id]
        VALUES (
            @DataShareRequestId,
            @FromStatus,
            @ToStatus,
            @ChangedByUserId,
            @ChangedByUserDomainId,
            @ChangedByUserOrganisationId,
            @ChangedAtUtc)";

    string IAuditLogSqlQueries.RecordDataShareRequestStatusChangeComment =>
      @"INSERT INTO [dbo].[AuditLogDataShareRequestStatusChangeComment]
	        (StatusChange, Comment, CommentOrder)
        VALUES (
	        @StatusChangeId,
	        @Comment,
        	@CommentOrder)";

    string IAuditLogSqlQueries.GetDataShareRequestStatusChanges =>
      @"SELECT
	        [sc].[Id] AS AuditLogDataShareRequestStatusChange_Id,
	        [sc].[DataShareRequest] AS AuditLogDataShareRequestStatusChange_DataShareRequestId,
	        [sc].[FromStatus] AS AuditLogDataShareRequestStatusChange_FromStatus,
	        [sc].[ToStatus] AS AuditLogDataShareRequestStatusChange_ToStatus,
	        [sc].[ChangedByUser] AS AuditLogDataShareRequestStatusChange_ChangedByUserId,
	        [sc].[ChangedByUserDomain] AS AuditLogDataShareRequestStatusChange_ChangedByUserDomainId,
	        [sc].[ChangedByUserOrganisation] AS AuditLogDataShareRequestStatusChange_ChangedByUserOrganisationId,
	        [sc].[ChangedAtUtc] AS AuditLogDataShareRequestStatusChange_ChangedAtUtc,

	        [scc].[Id] AS AuditLogDataShareRequestStatusChangeComment_Id,
	        [scc].[StatusChange] AS AuditLogDataShareRequestStatusChangeComment_StatusChangeId,
	        [scc].[Comment] AS AuditLogDataShareRequestStatusChangeComment_Comment,
	        [scc].[CommentOrder] AS AuditLogDataShareRequestStatusChangeComment_CommentOrder
	        
        FROM [dbo].[AuditLogDataShareRequestStatusChange] [sc]
	        LEFT JOIN [dbo].[AuditLogDataShareRequestStatusChangeComment] [scc] ON [scc].[StatusChange] = [sc].[Id]
        WHERE
            ([sc].[DataShareRequest] = @DataShareRequestId) AND
            (@HasFromStatuses = 0 OR [sc].[FromStatus] IN @FromStatuses) AND
            (@HasToStatuses = 0 OR [sc].[ToStatus] IN @ToStatuses)
        ORDER BY
	        [sc].[ChangedAtUtc],
	        [scc].[CommentOrder]";
}