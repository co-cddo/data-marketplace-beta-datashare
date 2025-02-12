using Agrimetrics.DataShare.Api.Db.DbAccess;
using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.ModelData.Reporting;
using Microsoft.Extensions.Logging;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.Reporting;

internal class ReportingRepository(
    ILogger<ReportingRepository> logger,
    IDatabaseChannelCreation databaseChannelCreation,
    IDatabaseCommandRunner databaseCommandRunner,
    IReportingSqlQueries reportingSqlQueries) : IReportingRepository
{
    async Task<IEnumerable<ReportingDataShareRequestInformationModelData>> IReportingRepository.GetAllReportingDataShareRequestInformationAsync()
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var allReportingDataShareRequestInformationFlattened =
                await databaseCommandRunner.DbQueryAsync<
                    ReportingDataShareRequestInformationModelData,
                    ReportingDataShareRequestStatusModelData,
                    ReportingDataShareRequestInformationModelData>(
                    databaseChannel.Connection,
                    databaseChannel.Transaction!,
                    reportingSqlQueries.GetAllReportingDataShareRequestInformation,
                    (dsrInformation, status) =>
                    {
                        dsrInformation.DataShareRequest_Statuses ??= [];
                        dsrInformation.DataShareRequest_Statuses.Add(status);

                        return dsrInformation;
                    },
                    nameof(ReportingDataShareRequestStatusModelData.Status_Id))
                .ConfigureAwait(false);

            return BuildReportingDataShareRequestStatusChangeSetModelDatas(allReportingDataShareRequestInformationFlattened).ToList();
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to GetAllReportingDataShareRequestInformation from database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }

        IEnumerable<ReportingDataShareRequestInformationModelData> BuildReportingDataShareRequestStatusChangeSetModelDatas(
            IEnumerable<ReportingDataShareRequestInformationModelData> reportingDataShareRequestInformation)
        {
            var reportingDataShareRequestInformationGroupedByDataShareRequestId =
                reportingDataShareRequestInformation.GroupBy(x => x.DataShareRequest_Id).ToList();

            foreach (var group in reportingDataShareRequestInformationGroupedByDataShareRequestId)
            {
                var indexedStatusChanges = group
                    .SelectMany(x => x.DataShareRequest_Statuses)
                    .OrderBy(x => x.Status_EnteredAtUtc)
                    .Select((value, index) => new { Value = value, Index = index })
                    .ToList();

                // Include an index field on to each status for simple ordering
                foreach (var indexedStatusChange in indexedStatusChanges)
                {
                    indexedStatusChange.Value.Status_Order = indexedStatusChange.Index + 1;
                }

                // Include the time at which each status was left by copying the time at which the subsequent status was entered, apart from the most recent status
                foreach (var indexedStatusChange in indexedStatusChanges.GetRange(0, indexedStatusChanges.Count - 1))
                {
                    var thisStatusChange = indexedStatusChange.Value;
                    var nextStatusChange = indexedStatusChanges[indexedStatusChange.Index + 1].Value;

                    thisStatusChange.Status_LeftAtUtc = nextStatusChange.Status_EnteredAtUtc;
                }

                var firstRecordInGroup = group.First();

                firstRecordInGroup.DataShareRequest_Statuses = indexedStatusChanges.Select(x => x.Value).ToList();

                yield return firstRecordInGroup;
            }
        }
    }
}