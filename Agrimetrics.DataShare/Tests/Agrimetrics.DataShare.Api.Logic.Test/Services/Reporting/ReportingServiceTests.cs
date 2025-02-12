using System.Net;
using Agrimetrics.DataShare.Api.Core.SystemProxies;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Services.Reporting;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;
using AutoFixture.AutoMoq;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Agrimetrics.DataShare.Api.Logic.Repositories.Reporting;
using Agrimetrics.DataShare.Api.Logic.ModelData.Reporting;
using Agrimetrics.DataShare.Api.Dto.Models.Reporting;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Reporting;

[TestFixture]
public class ReportingServiceTests
{
    #region QueryDataShareRequestCountsAsync() Tests
    [Test]
    public void GivenANullSetOfDataShareRequestCountQueries_WhenIQueryDataShareRequestCountsAsync_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.ReportingService.QueryDataShareRequestCountsAsync(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dataShareRequestCountQueries"));
    }

    [Test]
    public async Task GivenGettingReportingDataShareRequestInformationFromTheRepositoryWillFail_WhenIQueryDataShareRequestCountsAsync_ThenAFailedDataResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testException = new Exception("oh noes!");

        testItems.MockReportingRepository.Setup(x => x.GetAllReportingDataShareRequestInformationAsync())
            .ThrowsAsync(testException);

        var testFailedResult = testItems.Fixture.Create<IServiceOperationDataResult<IQueryDataShareRequestCountsResult>>();

        testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<IQueryDataShareRequestCountsResult>(
                testException.Message, It.IsAny<HttpStatusCode?>()))
            .Returns(() => testFailedResult);

        var result = await testItems.ReportingService.QueryDataShareRequestCountsAsync(
            Enumerable.Empty<IDataShareRequestCountQuery>());

        Assert.That(result, Is.EqualTo(testFailedResult));
    }

    [Test]
    public async Task GivenGettingReportingDataShareRequestInformationFromTheRepositoryWillSucceed_WhenIQueryDataShareRequestCountsAsync_ThenASuccessfulDataResultIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockReportingRepository.Setup(x => x.GetAllReportingDataShareRequestInformationAsync())
            .ReturnsAsync(Enumerable.Empty<ReportingDataShareRequestInformationModelData>);

        var testSuccessfulResult = testItems.Fixture.Create<IServiceOperationDataResult<IQueryDataShareRequestCountsResult>>();

        testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(
                It.IsAny<IQueryDataShareRequestCountsResult>(), It.IsAny<HttpStatusCode?>()))
            .Returns(() => testSuccessfulResult);

        var result = await testItems.ReportingService.QueryDataShareRequestCountsAsync(
            Enumerable.Empty<IDataShareRequestCountQuery>());

        Assert.That(result, Is.EqualTo(testSuccessfulResult));
    }

    [Test]
    public async Task GivenThereIsNoDataShareRequestInformation_WhenIQueryDataShareRequestCountsAsync_ThenAZeroResultIsReturnedForEachQuery()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestCountQueries = new List<DataShareRequestCountQuery>
        {
            CreateTestDataShareRequestCountQuery(id: 1, currentStatuses: Enum.GetValues<DataShareRequestStatus>()),
            CreateTestDataShareRequestCountQuery(id: 2, currentStatuses: Enum.GetValues<DataShareRequestStatus>()),
        };

        var result = (await testItems.ReportingService.QueryDataShareRequestCountsAsync(testDataShareRequestCountQueries))
            .Data!.DataShareRequestCounts.ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Exactly(testDataShareRequestCountQueries.Count).Items);

            foreach (var testDataShareRequestCountQuery in testDataShareRequestCountQueries)
            {
                Assert.That(result.Any(x =>
                    x.DataShareRequestCountQuery.Id == testDataShareRequestCountQuery.Id &&
                    x.NumberOfDataShareRequests == 0));
            }
        });
    }

    [Test]
    public async Task GivenAQueryForCurrentStatuses_WhenIQueryDataShareRequestCountsAsync_ThenTheNumberOfDataShareRequestsCurrentlyWithThoseStatusesAreReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockReportingRepository.Setup(x => x.GetAllReportingDataShareRequestInformationAsync())
            .ReturnsAsync(() =>
            [
                CreateTestReportingDataShareRequestInformationModelData(currentStatus: DataShareRequestStatusType.Accepted),
                CreateTestReportingDataShareRequestInformationModelData(currentStatus: DataShareRequestStatusType.Draft),
                CreateTestReportingDataShareRequestInformationModelData(currentStatus: DataShareRequestStatusType.Cancelled),
                CreateTestReportingDataShareRequestInformationModelData(currentStatus: DataShareRequestStatusType.Accepted),
                CreateTestReportingDataShareRequestInformationModelData(currentStatus: DataShareRequestStatusType.Returned),
                CreateTestReportingDataShareRequestInformationModelData(currentStatus: DataShareRequestStatusType.Cancelled)
            ]);

        var testDataShareRequestCountQueries = new List<DataShareRequestCountQuery>
        {
            CreateTestDataShareRequestCountQuery(id: 1, currentStatuses: [ DataShareRequestStatus.Accepted, DataShareRequestStatus.Draft ]),
            CreateTestDataShareRequestCountQuery(id: 2, currentStatuses: [ DataShareRequestStatus.InReview ]),
            CreateTestDataShareRequestCountQuery(id: 3, currentStatuses: [ DataShareRequestStatus.Cancelled, DataShareRequestStatus.Accepted, DataShareRequestStatus.Submitted ]),
            CreateTestDataShareRequestCountQuery(id: 4, currentStatuses: [ DataShareRequestStatus.Returned ]),
        };

        var result = (await testItems.ReportingService.QueryDataShareRequestCountsAsync(testDataShareRequestCountQueries))
            .Data!.DataShareRequestCounts.ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Exactly(testDataShareRequestCountQueries.Count).Items);

            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 1 && x.NumberOfDataShareRequests == 3));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 2 && x.NumberOfDataShareRequests == 0));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 3 && x.NumberOfDataShareRequests == 4));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 4 && x.NumberOfDataShareRequests == 1));
        });
    }

    [Test]
    public async Task GivenAQueryForPublisherDetails_WhenIQueryDataShareRequestCountsAsync_ThenTheNumberOfDataShareRequestsCurrentlyWithThosePublisherDetailsAreReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockReportingRepository.Setup(x => x.GetAllReportingDataShareRequestInformationAsync())
            .ReturnsAsync(() =>
            [
                CreateTestReportingDataShareRequestInformationModelData(publisherOrganisationId: 100, publisherDomainId: 200),
                CreateTestReportingDataShareRequestInformationModelData(publisherOrganisationId: 100, publisherDomainId: 210),
                CreateTestReportingDataShareRequestInformationModelData(publisherOrganisationId: 110, publisherDomainId: 200),
                CreateTestReportingDataShareRequestInformationModelData(publisherOrganisationId: 120, publisherDomainId: 220),
                CreateTestReportingDataShareRequestInformationModelData(publisherOrganisationId: 100, publisherDomainId: 200),
                CreateTestReportingDataShareRequestInformationModelData(publisherOrganisationId: 110, publisherDomainId: 200)
            ]);

        var testDataShareRequestCountQueries = new List<DataShareRequestCountQuery>
        {
            CreateTestDataShareRequestCountQuery(id: 1, publisherOrganisationId: 100),
            CreateTestDataShareRequestCountQuery(id: 2, publisherOrganisationId: 100, publisherDomainId: 200),
            CreateTestDataShareRequestCountQuery(id: 3, publisherOrganisationId: 100, publisherDomainId: 210),
            
            CreateTestDataShareRequestCountQuery(id: 4, publisherOrganisationId: 110),
            CreateTestDataShareRequestCountQuery(id: 5, publisherOrganisationId: 110, publisherDomainId: 200),

            CreateTestDataShareRequestCountQuery(id: 6, publisherOrganisationId: 120),
            CreateTestDataShareRequestCountQuery(id: 7, publisherOrganisationId: 120, publisherDomainId: 999),

            CreateTestDataShareRequestCountQuery(id: 8, publisherOrganisationId: 999)
        };

        var result = (await testItems.ReportingService.QueryDataShareRequestCountsAsync(testDataShareRequestCountQueries))
            .Data!.DataShareRequestCounts.ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Exactly(testDataShareRequestCountQueries.Count).Items);

            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 1 && x.NumberOfDataShareRequests == 3));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 2 && x.NumberOfDataShareRequests == 2));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 3 && x.NumberOfDataShareRequests == 1));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 4 && x.NumberOfDataShareRequests == 2));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 5 && x.NumberOfDataShareRequests == 2));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 6 && x.NumberOfDataShareRequests == 1));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 7 && x.NumberOfDataShareRequests == 0));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 8 && x.NumberOfDataShareRequests == 0));
        });
    }

    [Test]
    public async Task GivenAQueryForIntermediateStatuses_WhenIQueryDataShareRequestCountsAsync_ThenTheNumberOfDataShareRequestsThatHaveBeenInThoseStatusesAreReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockReportingRepository.Setup(x => x.GetAllReportingDataShareRequestInformationAsync())
            .ReturnsAsync(() =>
            [
                CreateTestReportingDataShareRequestInformationModelData(statuses:
                    [
                        CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Draft),
                        CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Submitted),
                    ]),

                CreateTestReportingDataShareRequestInformationModelData(statuses:
                [
                    CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Draft),
                    CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Submitted),
                    CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.InReview),
                    CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Returned),
                    CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Submitted),
                ]),

                CreateTestReportingDataShareRequestInformationModelData(statuses:
                [
                    CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Draft),
                    CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Submitted),
                    CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Cancelled)
                ]),
            ]);

        var testDataShareRequestCountQueries = new List<DataShareRequestCountQuery>
        {
            CreateTestDataShareRequestCountQuery(id: 1, intermediateStatuses:[ DataShareRequestStatus.Rejected ]),
            CreateTestDataShareRequestCountQuery(id: 2, intermediateStatuses:[ DataShareRequestStatus.Accepted, DataShareRequestStatus.Cancelled ]),
            CreateTestDataShareRequestCountQuery(id: 3, intermediateStatuses:[ DataShareRequestStatus.Draft ]),
            CreateTestDataShareRequestCountQuery(id: 4, intermediateStatuses:[ DataShareRequestStatus.Draft, DataShareRequestStatus.Submitted ])
        };

        var result = (await testItems.ReportingService.QueryDataShareRequestCountsAsync(testDataShareRequestCountQueries))
            .Data!.DataShareRequestCounts.ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Exactly(testDataShareRequestCountQueries.Count).Items);

            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 1 && x.NumberOfDataShareRequests == 0));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 2 && x.NumberOfDataShareRequests == 1));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 3 && x.NumberOfDataShareRequests == 3));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 4 && x.NumberOfDataShareRequests == 3));
        });
    }

    [Test]
    public async Task GivenAQueryForIntermediateStatusesLongerThanAMinimumDuration_WhenIQueryDataShareRequestCountsAsync_ThenTheNumberOfDataShareRequestsThatHaveBeenInNamedStatusesForAtLeastThatDurationAreReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockReportingRepository.Setup(x => x.GetAllReportingDataShareRequestInformationAsync())
            .ReturnsAsync(() =>
            [
                CreateTestReportingDataShareRequestInformationModelData(statuses:
                    [
                        CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Draft,
                            enteredAtUtc: CreateDateTime(10, 0, 0), leftAtUtc: CreateDateTime(10, 5, 0)), // 5 minutes

                        CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Submitted,
                            enteredAtUtc: CreateDateTime(11, 30, 0), leftAtUtc: CreateDateTime(11, 40, 0)), // 10 minutes

                        CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Draft,
                            enteredAtUtc: CreateDateTime(12, 0, 0), leftAtUtc: CreateDateTime(12, 2, 0)), // 2 minutes

                        CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Submitted,
                            enteredAtUtc: CreateDateTime(12, 30, 0), leftAtUtc: CreateDateTime(12, 45, 0)), // 15 minutes
                    ]),
            ]);

        var testDataShareRequestCountQueries = new List<DataShareRequestCountQuery>
        {
            CreateTestDataShareRequestCountQuery(id: 1, useOnlyTheMostRecentPeriodSpentInIntermediateStatuses: false, intermediateStatuses:[ DataShareRequestStatus.Draft ], minimumDuration: TimeSpan.FromMinutes(6)),
            CreateTestDataShareRequestCountQuery(id: 2, useOnlyTheMostRecentPeriodSpentInIntermediateStatuses: true, intermediateStatuses:[ DataShareRequestStatus.Draft ], minimumDuration: TimeSpan.FromMinutes(6)),

            CreateTestDataShareRequestCountQuery(id: 3, useOnlyTheMostRecentPeriodSpentInIntermediateStatuses: false, intermediateStatuses:[ DataShareRequestStatus.Submitted ], minimumDuration: TimeSpan.FromMinutes(15)),
            CreateTestDataShareRequestCountQuery(id: 4, useOnlyTheMostRecentPeriodSpentInIntermediateStatuses: true, intermediateStatuses:[ DataShareRequestStatus.Submitted ], minimumDuration: TimeSpan.FromMinutes(15)),

            CreateTestDataShareRequestCountQuery(id: 5, useOnlyTheMostRecentPeriodSpentInIntermediateStatuses: false, intermediateStatuses:[ DataShareRequestStatus.Draft, DataShareRequestStatus.Submitted ], minimumDuration: TimeSpan.FromMinutes(18)),
            CreateTestDataShareRequestCountQuery(id: 6, useOnlyTheMostRecentPeriodSpentInIntermediateStatuses: true, intermediateStatuses:[DataShareRequestStatus.Draft, DataShareRequestStatus.Submitted ], minimumDuration: TimeSpan.FromMinutes(18)),
        };

        var result = (await testItems.ReportingService.QueryDataShareRequestCountsAsync(testDataShareRequestCountQueries))
            .Data!.DataShareRequestCounts.ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Exactly(testDataShareRequestCountQueries.Count).Items);

            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 1 && x.NumberOfDataShareRequests == 1));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 2 && x.NumberOfDataShareRequests == 0));

            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 3 && x.NumberOfDataShareRequests == 1));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 4 && x.NumberOfDataShareRequests == 1));

            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 5 && x.NumberOfDataShareRequests == 1));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 6 && x.NumberOfDataShareRequests == 0));
        });

        DateTime CreateDateTime(int hour, int minute, int second)
        {
            return new DateTime(2024, 12, 25, hour, minute, second);
        }
    }

    [Test]
    public async Task GivenAQueryForIntermediateStatusesShorterThanAMaximumDuration_WhenIQueryDataShareRequestCountsAsync_ThenTheNumberOfDataShareRequestsThatHaveBeenInNamedStatusesForAtMostThatDurationAreReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockReportingRepository.Setup(x => x.GetAllReportingDataShareRequestInformationAsync())
            .ReturnsAsync(() =>
            [
                CreateTestReportingDataShareRequestInformationModelData(statuses:
                    [
                        CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Draft,
                            enteredAtUtc: CreateDateTime(10, 0, 0), leftAtUtc: CreateDateTime(10, 5, 0)), // 5 minutes

                        CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Submitted,
                            enteredAtUtc: CreateDateTime(11, 30, 0), leftAtUtc: CreateDateTime(11, 40, 0)), // 10 minutes

                        CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Draft,
                            enteredAtUtc: CreateDateTime(12, 0, 0), leftAtUtc: CreateDateTime(12, 2, 0)), // 2 minutes

                        CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Submitted,
                            enteredAtUtc: CreateDateTime(12, 30, 0), leftAtUtc: CreateDateTime(12, 45, 0)), // 15 minutes
                    ]),
            ]);

        var testDataShareRequestCountQueries = new List<DataShareRequestCountQuery>
        {
            CreateTestDataShareRequestCountQuery(id: 1, useOnlyTheMostRecentPeriodSpentInIntermediateStatuses: false, intermediateStatuses:[ DataShareRequestStatus.Draft ], maximumDuration: TimeSpan.FromMinutes(3)),
            CreateTestDataShareRequestCountQuery(id: 2, useOnlyTheMostRecentPeriodSpentInIntermediateStatuses: true, intermediateStatuses:[ DataShareRequestStatus.Draft ], maximumDuration: TimeSpan.FromMinutes(3)),

            CreateTestDataShareRequestCountQuery(id: 3, useOnlyTheMostRecentPeriodSpentInIntermediateStatuses: false, intermediateStatuses:[ DataShareRequestStatus.Submitted ], maximumDuration: TimeSpan.FromMinutes(20)),
            CreateTestDataShareRequestCountQuery(id: 4, useOnlyTheMostRecentPeriodSpentInIntermediateStatuses: true, intermediateStatuses:[ DataShareRequestStatus.Submitted ], maximumDuration: TimeSpan.FromMinutes(20)),

            CreateTestDataShareRequestCountQuery(id: 5, useOnlyTheMostRecentPeriodSpentInIntermediateStatuses: false, intermediateStatuses:[ DataShareRequestStatus.Draft, DataShareRequestStatus.Submitted ], maximumDuration: TimeSpan.FromMinutes(17)),
            CreateTestDataShareRequestCountQuery(id: 6, useOnlyTheMostRecentPeriodSpentInIntermediateStatuses: true, intermediateStatuses:[DataShareRequestStatus.Draft, DataShareRequestStatus.Submitted ], maximumDuration: TimeSpan.FromMinutes(17)),
        };

        var result = (await testItems.ReportingService.QueryDataShareRequestCountsAsync(testDataShareRequestCountQueries))
            .Data!.DataShareRequestCounts.ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Exactly(testDataShareRequestCountQueries.Count).Items);

            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 1 && x.NumberOfDataShareRequests == 0));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 2 && x.NumberOfDataShareRequests == 1));

            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 3 && x.NumberOfDataShareRequests == 0));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 4 && x.NumberOfDataShareRequests == 1));

            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 5 && x.NumberOfDataShareRequests == 0));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 6 && x.NumberOfDataShareRequests == 1));
        });

        DateTime CreateDateTime(int hour, int minute, int second)
        {
            return new DateTime(2024, 12, 25, hour, minute, second);
        }
    }

    [Test]
    public async Task GivenAQueryForIntermediateStatusesEnteredFromAPointInTime_WhenIQueryDataShareRequestCountsAsync_ThenTheNumberOfDataShareRequestsThatEnteredThoseStatusesFromAPointInTimeAreReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockReportingRepository.Setup(x => x.GetAllReportingDataShareRequestInformationAsync())
            .ReturnsAsync(() =>
            [
                CreateTestReportingDataShareRequestInformationModelData(statuses:
                    [
                        CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Draft,
                            enteredAtUtc: CreateDateTime(10, 0, 0)),

                        CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Submitted,
                            enteredAtUtc: CreateDateTime(11, 40, 0)),

                        CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Draft,
                            enteredAtUtc: CreateDateTime(12, 0, 0)),

                        CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Submitted,
                            enteredAtUtc: CreateDateTime(12, 30, 0))
                    ]),
            ]);

        var testDataShareRequestCountQueries = new List<DataShareRequestCountQuery>
        {
            CreateTestDataShareRequestCountQuery(id: 1, intermediateStatuses:[ DataShareRequestStatus.Draft ], from: CreateDateTime(9, 59, 59)),
            CreateTestDataShareRequestCountQuery(id: 2, intermediateStatuses:[ DataShareRequestStatus.Draft ], from: CreateDateTime(10, 0, 0)),
            CreateTestDataShareRequestCountQuery(id: 3, intermediateStatuses:[ DataShareRequestStatus.Draft ], from: CreateDateTime(10, 0, 1)),

            CreateTestDataShareRequestCountQuery(id: 4, intermediateStatuses:[ DataShareRequestStatus.Submitted ], from: CreateDateTime(11, 39, 59)),
            CreateTestDataShareRequestCountQuery(id: 5, intermediateStatuses:[ DataShareRequestStatus.Submitted ], from: CreateDateTime(11, 40, 0)),
            CreateTestDataShareRequestCountQuery(id: 6, intermediateStatuses:[ DataShareRequestStatus.Submitted ], from: CreateDateTime(11, 40, 1)),

            CreateTestDataShareRequestCountQuery(id: 7, intermediateStatuses:[ DataShareRequestStatus.Draft, DataShareRequestStatus.Submitted ], from: CreateDateTime(9, 59, 59)),
            CreateTestDataShareRequestCountQuery(id: 8, intermediateStatuses:[ DataShareRequestStatus.Draft, DataShareRequestStatus.Submitted ], from: CreateDateTime(10, 0, 0)),
            CreateTestDataShareRequestCountQuery(id: 9, intermediateStatuses:[ DataShareRequestStatus.Draft, DataShareRequestStatus.Submitted ], from: CreateDateTime(10, 0, 1)),
        };

        var result = (await testItems.ReportingService.QueryDataShareRequestCountsAsync(testDataShareRequestCountQueries))
            .Data!.DataShareRequestCounts.ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Exactly(testDataShareRequestCountQueries.Count).Items);

            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 1 && x.NumberOfDataShareRequests == 1));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 2 && x.NumberOfDataShareRequests == 1));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 3 && x.NumberOfDataShareRequests == 0));

            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 4 && x.NumberOfDataShareRequests == 1));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 5 && x.NumberOfDataShareRequests == 1));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 6 && x.NumberOfDataShareRequests == 0));

            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 7 && x.NumberOfDataShareRequests == 1));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 8 && x.NumberOfDataShareRequests == 1));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 9 && x.NumberOfDataShareRequests == 0));
        });

        DateTime CreateDateTime(int hour, int minute, int second)
        {
            return new DateTime(2024, 12, 25, hour, minute, second);
        }
    }

    [Test]
    public async Task GivenAQueryForIntermediateStatusesLeftBeforeAPointInTime_WhenIQueryDataShareRequestCountsAsync_ThenTheNumberOfDataShareRequestsThatLeftThoseStatusesBeforeAPointInTimeAreReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockReportingRepository.Setup(x => x.GetAllReportingDataShareRequestInformationAsync())
            .ReturnsAsync(() =>
            [
                CreateTestReportingDataShareRequestInformationModelData(statuses:
                    [
                        CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Draft,
                            leftAtUtc: CreateDateTime(10, 0, 0)),

                        CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Submitted,
                            leftAtUtc: CreateDateTime(11, 40, 0)),

                        CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Draft,
                            leftAtUtc: CreateDateTime(12, 0, 0)),

                        CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Submitted,
                            leftAtUtc: CreateDateTime(12, 30, 0))
                    ]),
            ]);

        var testDataShareRequestCountQueries = new List<DataShareRequestCountQuery>
        {
            CreateTestDataShareRequestCountQuery(id: 1, intermediateStatuses:[ DataShareRequestStatus.Draft ], to: CreateDateTime(11, 59, 59)),
            CreateTestDataShareRequestCountQuery(id: 2, intermediateStatuses:[ DataShareRequestStatus.Draft ], to: CreateDateTime(12, 0, 0)),
            CreateTestDataShareRequestCountQuery(id: 3, intermediateStatuses:[ DataShareRequestStatus.Draft ], to: CreateDateTime(12, 0, 1)),

            CreateTestDataShareRequestCountQuery(id: 4, intermediateStatuses:[ DataShareRequestStatus.Submitted ], to: CreateDateTime(12, 29, 59)),
            CreateTestDataShareRequestCountQuery(id: 5, intermediateStatuses:[ DataShareRequestStatus.Submitted ], to: CreateDateTime(12, 30, 0)),
            CreateTestDataShareRequestCountQuery(id: 6, intermediateStatuses:[ DataShareRequestStatus.Submitted ], to: CreateDateTime(12, 30, 1)),

            CreateTestDataShareRequestCountQuery(id: 7, intermediateStatuses:[ DataShareRequestStatus.Draft, DataShareRequestStatus.Submitted ], to: CreateDateTime(12, 29, 59)),
            CreateTestDataShareRequestCountQuery(id: 8, intermediateStatuses:[ DataShareRequestStatus.Draft, DataShareRequestStatus.Submitted ], to: CreateDateTime(12, 30, 0)),
            CreateTestDataShareRequestCountQuery(id: 9, intermediateStatuses:[ DataShareRequestStatus.Draft, DataShareRequestStatus.Submitted ], to: CreateDateTime(12, 30, 1)),
        };

        var result = (await testItems.ReportingService.QueryDataShareRequestCountsAsync(testDataShareRequestCountQueries))
            .Data!.DataShareRequestCounts.ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Exactly(testDataShareRequestCountQueries.Count).Items);

            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 1 && x.NumberOfDataShareRequests == 0));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 2 && x.NumberOfDataShareRequests == 1));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 3 && x.NumberOfDataShareRequests == 1));

            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 4 && x.NumberOfDataShareRequests == 0));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 5 && x.NumberOfDataShareRequests == 1));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 6 && x.NumberOfDataShareRequests == 1));

            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 7 && x.NumberOfDataShareRequests == 0));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 8 && x.NumberOfDataShareRequests == 1));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 9 && x.NumberOfDataShareRequests == 1));
        });

        DateTime CreateDateTime(int hour, int minute, int second)
        {
            return new DateTime(2024, 12, 25, hour, minute, second);
        }
    }

    [Test]
    public async Task GivenAQueryForIntermediateStatusesLeftBeforeAPointInTimeAndAStatusHasNotBeenLeft_WhenIQueryDataShareRequestCountsAsync_ThenTheLeavingTimeIsTakenAsTheCurrentTime()
    {
        var testItems = CreateTestItems();

        var testTimeNow = CreateDateTime(11, 30, 0);

        testItems.MockClock.SetupGet(x => x.UtcNow).Returns(testTimeNow);

        testItems.MockReportingRepository.Setup(x => x.GetAllReportingDataShareRequestInformationAsync())
            .ReturnsAsync(() =>
            [
                CreateTestReportingDataShareRequestInformationModelData(statuses:
                    [
                        CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Draft,
                            leftAtUtc: CreateDateTime(10, 0, 0)),

                        CreateTestReportingDataShareRequestStatusModelData(status: DataShareRequestStatusType.Submitted),
                    ]),
            ]);

        var testDataShareRequestCountQueries = new List<DataShareRequestCountQuery>
        {
            CreateTestDataShareRequestCountQuery(id: 1, intermediateStatuses:[ DataShareRequestStatus.Submitted ], to: testTimeNow.AddMinutes(-1)),
            CreateTestDataShareRequestCountQuery(id: 2, intermediateStatuses:[ DataShareRequestStatus.Submitted ], to: testTimeNow),
            CreateTestDataShareRequestCountQuery(id: 3, intermediateStatuses:[ DataShareRequestStatus.Submitted ], to: testTimeNow.AddMinutes(1))
        };

        var result = (await testItems.ReportingService.QueryDataShareRequestCountsAsync(testDataShareRequestCountQueries))
            .Data!.DataShareRequestCounts.ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Exactly(testDataShareRequestCountQueries.Count).Items);

            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 1 && x.NumberOfDataShareRequests == 0));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 2 && x.NumberOfDataShareRequests == 1));
            Assert.That(result.Any(x => x.DataShareRequestCountQuery.Id == 3 && x.NumberOfDataShareRequests == 1));
        });

        DateTime CreateDateTime(int hour, int minute, int second)
        {
            return new DateTime(2024, 12, 25, hour, minute, second);
        }
    }
    #endregion

    #region Test Data Creation
    private static ReportingDataShareRequestInformationModelData CreateTestReportingDataShareRequestInformationModelData(DataShareRequestStatusType? currentStatus = null,
        int? publisherOrganisationId = null,
        int? publisherDomainId = null,
        IEnumerable<ReportingDataShareRequestStatusModelData>? statuses = null)
    {
        var statusesList = (statuses ?? [CreateTestReportingDataShareRequestStatusModelData()]).ToList();
        foreach (var x in statusesList.Select((value, index) => new { Value = value, Index = index}))
        {
            x.Value.Status_Order = x.Index + 1;
        }

        return new ReportingDataShareRequestInformationModelData
        {
            DataShareRequest_CurrentStatus = currentStatus ?? DataShareRequestStatusType.None,
            DataShareRequest_PublisherOrganisationId = publisherOrganisationId ?? 0,
            DataShareRequest_PublisherDomainId = publisherDomainId ?? 0,
            DataShareRequest_Statuses = statusesList
        };
    }

    private static ReportingDataShareRequestStatusModelData CreateTestReportingDataShareRequestStatusModelData(
        Guid? id = null,
        int? order = null,
        DataShareRequestStatusType? status = null,
        DateTime? enteredAtUtc = null,
        DateTime? leftAtUtc = null)
    {
        return new ReportingDataShareRequestStatusModelData
        {
            Status_Id = id ?? Guid.Empty,
            Status_Order = order ?? 0,
            Status_Status = status ?? DataShareRequestStatusType.None,
            Status_EnteredAtUtc = enteredAtUtc ?? DateTime.MinValue,
            Status_LeftAtUtc = leftAtUtc
        };
    }

    private static DataShareRequestCountQuery CreateTestDataShareRequestCountQuery(
        int? id = null,
        IEnumerable<DataShareRequestStatus>? currentStatuses = null,
        IEnumerable<DataShareRequestStatus>? intermediateStatuses = null,
        bool? useOnlyTheMostRecentPeriodSpentInIntermediateStatuses = null,
        TimeSpan? minimumDuration = null,
        TimeSpan? maximumDuration = null,
        DateTime? from = null,
        DateTime? to = null,
        int? publisherOrganisationId = null,
        int? publisherDomainId = null)
    {
        return new DataShareRequestCountQuery
        {
            Id = id ?? 0,
            CurrentStatuses = currentStatuses ?? [],
            IntermediateStatuses = intermediateStatuses ?? [],
            UseOnlyTheMostRecentPeriodSpentInIntermediateStatuses = useOnlyTheMostRecentPeriodSpentInIntermediateStatuses ?? false,
            MinimumDuration = minimumDuration,
            MaximumDuration = maximumDuration,
            From = from,
            To = to,
            PublisherOrganisationId = publisherOrganisationId,
            PublisherDomainId = publisherDomainId
        };
    }
    
    private static IServiceOperationDataResult<IQueryDataShareRequestCountsResult> CreateTestSuccessfulDataResult(
        QueryDataShareRequestCountsResult queryDataShareRequestCountsResult)
    {
        var mockServiceOperationDataResult = new Mock<IServiceOperationDataResult<IQueryDataShareRequestCountsResult>>();

        mockServiceOperationDataResult.SetupGet(x => x.Success).Returns(true);
        mockServiceOperationDataResult.SetupGet(x => x.Error).Returns((string?)null);
        mockServiceOperationDataResult.SetupGet(x => x.Data).Returns(queryDataShareRequestCountsResult);

        return mockServiceOperationDataResult.Object;
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockLogger = Mock.Get(fixture.Freeze<ILogger<ReportingService>>());
        var mockReportingRepository = Mock.Get(fixture.Create<IReportingRepository>());
        var mockClock = Mock.Get(fixture.Create<IClock>());
        var mockServiceOperationResultFactory = Mock.Get(fixture.Create<IServiceOperationResultFactory>());

        ConfigureHappyPathTesting();

        var auditLogService = new ReportingService(
            mockLogger.Object,
            mockReportingRepository.Object,
            mockClock.Object,
            mockServiceOperationResultFactory.Object);

        return new TestItems(
            fixture,
            auditLogService,
            mockReportingRepository,
            mockClock,
            mockServiceOperationResultFactory);

        void ConfigureHappyPathTesting()
        {
            mockServiceOperationResultFactory
                .Setup(x => x.CreateSuccessfulDataResult<IQueryDataShareRequestCountsResult>(
                    It.IsAny<QueryDataShareRequestCountsResult>(), It.IsAny<HttpStatusCode?>()))
                .Returns((QueryDataShareRequestCountsResult queryDataShareRequestCountsResult, HttpStatusCode? _) =>
                    CreateTestSuccessfulDataResult(queryDataShareRequestCountsResult));
        }
    }

    private class TestItems(
        IFixture fixture,
        IReportingService reportingService,
        Mock<IReportingRepository> mockReportingRepository,
        Mock<IClock> mockClock,
        Mock<IServiceOperationResultFactory> mockServiceOperationResultFactory)
    {
        public IFixture Fixture { get; } = fixture;
        public IReportingService ReportingService { get; } = reportingService;
        public Mock<IReportingRepository> MockReportingRepository { get; } = mockReportingRepository;
        public Mock<IClock> MockClock { get; } = mockClock;
        public Mock<IServiceOperationResultFactory> MockServiceOperationResultFactory { get; } = mockServiceOperationResultFactory;
    }
    #endregion
}
