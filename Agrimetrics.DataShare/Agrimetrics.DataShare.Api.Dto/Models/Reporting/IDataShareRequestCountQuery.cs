using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Dto.Models.Reporting;

public interface IDataShareRequestCountQuery
{
    int Id { get; }
    IEnumerable<DataShareRequestStatus> CurrentStatuses { get; }
    IEnumerable<DataShareRequestStatus> IntermediateStatuses { get; }
    bool UseOnlyTheMostRecentPeriodSpentInIntermediateStatuses { get; }
    TimeSpan? MinimumDuration { get; }
    TimeSpan? MaximumDuration { get; }
    DateTime? From { get; }
    DateTime? To { get; }
    int? PublisherOrganisationId { get; }
    int? PublisherDomainId { get; }
}