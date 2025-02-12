using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Reporting;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.Reporting;
using Agrimetrics.DataShare.Api.Logic.Repositories.Reporting;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Agrimetrics.DataShare.Api.Core.SystemProxies;

namespace Agrimetrics.DataShare.Api.Logic.Services.Reporting;

internal class ReportingService(
    ILogger<ReportingService> logger,
    IReportingRepository reportingRepository,
    IClock clock,
    IServiceOperationResultFactory serviceOperationResultFactory) : IReportingService
{
    async Task<IServiceOperationDataResult<IQueryDataShareRequestCountsResult>> IReportingService.QueryDataShareRequestCountsAsync(
        IEnumerable<IDataShareRequestCountQuery> dataShareRequestCountQueries)
    {
        ArgumentNullException.ThrowIfNull(dataShareRequestCountQueries);

        try
        {
            var dataShareRequestInformation =
                (await reportingRepository.GetAllReportingDataShareRequestInformationAsync()).ToList();

            var getDataShareRequestCounts = dataShareRequestCountQueries.Select(dataShareRequestCountQuery =>
                GetDataShareRequestCountAsync(dataShareRequestCountQuery, dataShareRequestInformation));

            return serviceOperationResultFactory.CreateSuccessfulDataResult<IQueryDataShareRequestCountsResult>(new QueryDataShareRequestCountsResult
            {
                DataShareRequestCounts = getDataShareRequestCounts.ToList()
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to GetDataShareRequestCounts");

            var response = serviceOperationResultFactory.CreateFailedDataResult<IQueryDataShareRequestCountsResult>(ex.Message);

            return await Task.FromResult(response);
        }

        IDataShareRequestCount GetDataShareRequestCountAsync(
            IDataShareRequestCountQuery dataShareRequestCountQuery,
            IEnumerable<ReportingDataShareRequestInformationModelData> dataShareRequests)
        {
            var dataShareRequestsMatchingQuery = dataShareRequests
                .Where(x => DataShareRequestMatchesQuery(dataShareRequestCountQuery, x))
                .ToList();

            return new DataShareRequestCount
            {
                DataShareRequestCountQuery = (DataShareRequestCountQuery) dataShareRequestCountQuery,
                NumberOfDataShareRequests = dataShareRequestsMatchingQuery.Count
            };
        }
    }

    private bool DataShareRequestMatchesQuery(
        IDataShareRequestCountQuery dataShareRequestCountQuery,
        ReportingDataShareRequestInformationModelData dataShareRequest)
    {
        // Firstly check the things that related to the overall data share request
        if (!CurrentDataShareRequestStatusMatchesQuery(dataShareRequestCountQuery, dataShareRequest)) return false;
        if (!DataShareRequestPublisherMatchesQuery(dataShareRequestCountQuery, dataShareRequest)) return false;

        // Now check things that relate to periods spent in individual statuses
        if (!IntermediateDataShareRequestStatusesMatchQuery(dataShareRequestCountQuery, dataShareRequest)) return false;

        return true;
    }

    private static bool CurrentDataShareRequestStatusMatchesQuery(
        IDataShareRequestCountQuery dataShareRequestCountQuery,
        ReportingDataShareRequestInformationModelData dataShareRequest)
    {
        var queryCurrentStatuses = dataShareRequestCountQuery.CurrentStatuses.Select(ConvertDataShareRequestStatusToDataShareRequestStatusType).ToList();
        if (!queryCurrentStatuses.Any()) return true;

        return queryCurrentStatuses.Contains(dataShareRequest.DataShareRequest_CurrentStatus);
    }

    private static bool DataShareRequestPublisherMatchesQuery(
        IDataShareRequestCountQuery dataShareRequestCountQuery,
        ReportingDataShareRequestInformationModelData dataShareRequest)
    {
        if (dataShareRequestCountQuery.PublisherOrganisationId.HasValue &&
            dataShareRequestCountQuery.PublisherOrganisationId.Value != dataShareRequest.DataShareRequest_PublisherOrganisationId) return false;

        if (dataShareRequestCountQuery.PublisherDomainId.HasValue &&
            dataShareRequestCountQuery.PublisherDomainId.Value != dataShareRequest.DataShareRequest_PublisherDomainId) return false;

        return true;
    }

    private bool IntermediateDataShareRequestStatusesMatchQuery(
        IDataShareRequestCountQuery dataShareRequestCountQuery,
        ReportingDataShareRequestInformationModelData dataShareRequest)
    {
        var queryStatusList = DoGetQueryIntermediateStatuses(dataShareRequestCountQuery).ToList();

        var queriedDataShareRequestStatusList = DoGetDataShareRequestIntermediateStatuses(
            dataShareRequestCountQuery, dataShareRequest, queryStatusList).ToList();

        // If there are no matching statuses then this data share request was never in any of the queried statuses
        if (!queriedDataShareRequestStatusList.Any()) return false;

        if (!DurationOfIntermediateDataShareRequestStatusesMatchQuery(
                dataShareRequestCountQuery, queriedDataShareRequestStatusList)) return false;
        
        if (!TimingOfIntermediateDataShareRequestStatusesMatchQuery(
                dataShareRequestCountQuery, queriedDataShareRequestStatusList)) return false;

        return true;
    }

    private static IEnumerable<DataShareRequestStatusType> DoGetQueryIntermediateStatuses(
        IDataShareRequestCountQuery dataShareRequestCountQuery)
    {
        var queryIntermediateStatusList = dataShareRequestCountQuery.IntermediateStatuses.Select(ConvertDataShareRequestStatusToDataShareRequestStatusType).ToList();
        if (queryIntermediateStatusList.Any()) return queryIntermediateStatusList;

        // If there are no statuses provided, then we treat this is a query against any status, and the simplest
        // way to achieve this downstream is to include all statuses here

        queryIntermediateStatusList.AddRange(Enum.GetValues<DataShareRequestStatusType>());
        return queryIntermediateStatusList;
    }

    private static IEnumerable<ReportingDataShareRequestStatusModelData> DoGetDataShareRequestIntermediateStatuses(
        IDataShareRequestCountQuery dataShareRequestCountQuery,
        ReportingDataShareRequestInformationModelData dataShareRequest,
        IEnumerable<DataShareRequestStatusType> queryStatuses)
    {
        var queriedStatuses = DoFindQueriedStatuses(
            dataShareRequestCountQuery, dataShareRequest, queryStatuses);

        return queriedStatuses.OrderBy(x => x.Status_Order).ToList();
    }

    private static IEnumerable<ReportingDataShareRequestStatusModelData> DoFindQueriedStatuses(
        IDataShareRequestCountQuery dataShareRequestCountQuery,
        ReportingDataShareRequestInformationModelData dataShareRequest,
        IEnumerable<DataShareRequestStatusType> queryStatuses)
    {
        foreach (var queryStatus in queryStatuses)
        {
            var matchingDataShareRequestStatuses =
                dataShareRequest.DataShareRequest_Statuses.Where(x => x.Status_Status == queryStatus).ToList();

            if (!matchingDataShareRequestStatuses.Any()) continue;

            if (!dataShareRequestCountQuery.UseOnlyTheMostRecentPeriodSpentInIntermediateStatuses)
            {
                foreach (var matchingDataShareRequestStatus in matchingDataShareRequestStatuses)
                {
                    yield return matchingDataShareRequestStatus;
                }
            }
            else
            {
                yield return matchingDataShareRequestStatuses.MaxBy(x => x.Status_Order)!;
            }
        }
    }

    private bool DurationOfIntermediateDataShareRequestStatusesMatchQuery(
        IDataShareRequestCountQuery dataShareRequestCountQuery,
        IEnumerable<ReportingDataShareRequestStatusModelData> dataShareRequestStatuses)
    {
        var totalDurationSpentInDataShareRequestStatuses = dataShareRequestStatuses.Aggregate(
            TimeSpan.Zero,
            (x, y) => x + GetDurationOfStatus(y));

        if (dataShareRequestCountQuery.MinimumDuration.HasValue &&
            dataShareRequestCountQuery.MinimumDuration > totalDurationSpentInDataShareRequestStatuses) return false;

        if (dataShareRequestCountQuery.MaximumDuration.HasValue &&
            dataShareRequestCountQuery.MaximumDuration < totalDurationSpentInDataShareRequestStatuses) return false;

        return true;

        TimeSpan GetDurationOfStatus(ReportingDataShareRequestStatusModelData dataShareRequestStatus)
        {
            var whenEnteredStatus = dataShareRequestStatus.Status_EnteredAtUtc;
            var whenLeftStatus = dataShareRequestStatus.Status_LeftAtUtc ?? clock.UtcNow;

            return whenLeftStatus - whenEnteredStatus;
        }
    }

    private bool TimingOfIntermediateDataShareRequestStatusesMatchQuery(
        IDataShareRequestCountQuery dataShareRequestCountQuery,
        IList<ReportingDataShareRequestStatusModelData> dataShareRequestStatuses)
    {
        if (!TimeEnteredEarliestStatusMatchesQuery()) return false;
        if (!TimeLeftLatestStatusMatchesQuery()) return false;

        return true;

        bool TimeEnteredEarliestStatusMatchesQuery()
        {
            if (!dataShareRequestCountQuery.From.HasValue) return true;

            var whenEnteredEarliestStatus = dataShareRequestStatuses.MinBy(x => x.Status_Order)!.Status_EnteredAtUtc;

            return dataShareRequestCountQuery.From.Value <= whenEnteredEarliestStatus;
        }

        bool TimeLeftLatestStatusMatchesQuery()
        {
            if (!dataShareRequestCountQuery.To.HasValue) return true;

            var whenLeftLatestStatus = dataShareRequestStatuses.MaxBy(x => x.Status_Order)!.Status_LeftAtUtc ?? clock.UtcNow;

            return dataShareRequestCountQuery.To.Value >= whenLeftLatestStatus;
        }
    }

    [ExcludeFromCodeCoverage] // This is just a mapper, not intended to be unit tested
    private static DataShareRequestStatusType ConvertDataShareRequestStatusToDataShareRequestStatusType(
        DataShareRequestStatus dataShareRequestStatus)
    {
        return dataShareRequestStatus switch
        {
            DataShareRequestStatus.Draft => DataShareRequestStatusType.Draft,
            DataShareRequestStatus.Submitted => DataShareRequestStatusType.Submitted,
            DataShareRequestStatus.Rejected => DataShareRequestStatusType.Rejected,
            DataShareRequestStatus.Accepted => DataShareRequestStatusType.Accepted,
            DataShareRequestStatus.Cancelled => DataShareRequestStatusType.Cancelled,
            DataShareRequestStatus.Returned => DataShareRequestStatusType.Returned,
            DataShareRequestStatus.InReview => DataShareRequestStatusType.InReview,
            DataShareRequestStatus.Deleted => DataShareRequestStatusType.Deleted,

            _ => throw new InvalidEnumArgumentException(
                nameof(dataShareRequestStatus), (int)dataShareRequestStatus, typeof(DataShareRequestStatus))
        };
    }
}