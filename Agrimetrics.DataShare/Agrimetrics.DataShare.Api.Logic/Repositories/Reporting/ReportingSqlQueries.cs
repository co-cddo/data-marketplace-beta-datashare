using System.Diagnostics.CodeAnalysis;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.Reporting;

[ExcludeFromCodeCoverage] // It makes no sense to write unit tests against SQL statements as they have actual function
internal class ReportingSqlQueries : IReportingSqlQueries
{
    string IReportingSqlQueries.GetAllReportingDataShareRequestInformation =>
        @"SELECT
	        [dsr].[Id] AS [DataShareRequest_Id],
	        [dsr].[RequestId] AS [DataShareRequest_RequestId],
	        [dsr].[RequestStatus] AS [DataShareRequest_CurrentStatus],
	        [dsr].[SupplierOrganisation] AS [DataShareRequest_PublisherOrganisationId],
	        [dsr].[SupplierDomain] AS [DataShareRequest_PublisherDomainId],

	        [sc].[Id] AS [Status_Id],
	        [sc].[ToStatus] AS [Status_Status],
	        [sc].[ChangedAtUtc] AS [Status_EnteredAtUtc]
        FROM [dbo].[AuditLogDataShareRequestStatusChange] [sc]
	        JOIN [dbo].[DataShareRequest] [dsr] ON [dsr].[Id] = [sc].[DataShareRequest]
        ORDER BY
	        [dsr].[RequestId],
	        [sc].[ChangedAtUtc]";
}