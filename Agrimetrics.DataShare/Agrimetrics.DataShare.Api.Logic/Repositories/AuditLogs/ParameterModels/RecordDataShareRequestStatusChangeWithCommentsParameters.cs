using System.Data;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.AuditLogs.ParameterModels;

internal class RecordDataShareRequestStatusChangeWithCommentsParameters : IRecordDataShareRequestStatusChangeWithCommentsParameters
{
    public required IDbConnection DbConnection { get; init; }

    public required IDbTransaction DbTransaction { get; init; }

    public required Guid DataShareRequestId { get; init; }

    public required DataShareRequestStatusType FromStatus { get; init; }

    public required DataShareRequestStatusType ToStatus { get; init; }

    public required IUserIdSet ChangedByUser { get; init; }

    public required DateTime ChangedAtLocalTime { get; init; }

    public required IEnumerable<string> Comments { get; init; }
}