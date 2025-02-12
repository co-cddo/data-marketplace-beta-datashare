using Agrimetrics.DataShare.Api.Dto.Models.AuditLogs;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Repositories.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.Services.AuditLog;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AuditLog
{
    [TestFixture]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration", Justification = "Multiple enumeration in mock verification")]
    public class AuditLogServiceTests
    {
        #region GetAuditLogsForDataShareRequestStatusChangeToStatusAsync() Tests
        [Test]
        public async Task GivenASetOfDataShareRequestAndStatuses_WhenIGetAuditLogsForDataShareRequestStatusChangeToStatusAsync_ThenAuditLogsForDataShareRequestStatusChangeToThoseStatusesAreReturned()
        {
            var testItems = CreateTestItems();

            var testDataShareRequestId = testItems.Fixture.Create<Guid>();

            var testStatuses = new List<DataShareRequestStatus> {DataShareRequestStatus.Accepted, DataShareRequestStatus.Rejected};

            testItems.MockAuditLogModelDataFactory
                .Setup(x => x.ConvertDataShareRequestStatusToDataShareRequestStatusType(DataShareRequestStatus.Accepted))
                .Returns(DataShareRequestStatusType.Accepted);

            testItems.MockAuditLogModelDataFactory
                .Setup(x => x.ConvertDataShareRequestStatusToDataShareRequestStatusType(DataShareRequestStatus.Rejected))
                .Returns(DataShareRequestStatusType.Rejected);

            var testAuditLogDataShareRequestStatusChangeModelDatas = testItems.Fixture
                .Build<AuditLogDataShareRequestStatusChangeModelData>()
                .CreateMany(3);

            testItems.MockAuditLogRepository
                .Setup(x => x.GetAuditLogsForDataShareRequestStatusChangesSetAsync(
                    testDataShareRequestId,
                    null,
                    It.Is<IEnumerable<DataShareRequestStatusType>>(toStatuses =>
                        toStatuses.Count() == 2 &&
                        toStatuses.Contains(DataShareRequestStatusType.Accepted) &&
                        toStatuses.Contains(DataShareRequestStatusType.Rejected))))
                .ReturnsAsync(testAuditLogDataShareRequestStatusChangeModelDatas);

            var testAuditLogEntries = testItems.Fixture
                .Build<DataShareRequestAuditLogEntry>()
                .CreateMany()
                .ToList();

            var testDataShareRequestAuditLog = testItems.Fixture
                .Build<DataShareRequestAuditLog>()
                .With(x => x.DataShareRequestId, testDataShareRequestId)
                .With(x => x.AuditLogEntries, testAuditLogEntries)
                .Create();

            var testAuditLogDataShareRequestStatusChangesResult = testItems.Fixture
                .Build<AuditLogDataShareRequestStatusChangesResult>()
                .With(x => x.DataShareRequestAuditLog, testDataShareRequestAuditLog)
                .Create();

            testItems.MockAuditLogModelDataFactory
                .Setup(x => x.ConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult(
                    testDataShareRequestId,
                    It.Is<IEnumerable<AuditLogDataShareRequestStatusChangeModelData>>(changes =>
                        changes.Count() == 3 &&
                        changes.Any(change => change == testAuditLogDataShareRequestStatusChangeModelDatas.ElementAt(0)) &&
                        changes.Any(change => change == testAuditLogDataShareRequestStatusChangeModelDatas.ElementAt(1)) &&
                        changes.Any(change => change == testAuditLogDataShareRequestStatusChangeModelDatas.ElementAt(2)))))
                .Returns(() => testAuditLogDataShareRequestStatusChangesResult);

            testItems.MockServiceOperationResultFactory
                .Setup(x => x.CreateSuccessfulDataResult(It.IsAny<IAuditLogDataShareRequestStatusChangesResult>(),
                    It.IsAny<HttpStatusCode?>()))
                .Returns((IAuditLogDataShareRequestStatusChangesResult result, HttpStatusCode? _) =>
                {
                    var mockServiceOperationDataResult = new Mock<IServiceOperationDataResult<IAuditLogDataShareRequestStatusChangesResult>>();

                    mockServiceOperationDataResult.SetupGet(x => x.Data).Returns(result);

                    return mockServiceOperationDataResult.Object;
                });

            var result = await testItems.AuditLogService.GetAuditLogsForDataShareRequestStatusChangeToStatusAsync(
                testDataShareRequestId, testStatuses);

            Assert.That(result.Data!.DataShareRequestAuditLog.DataShareRequestId, Is.EqualTo(testDataShareRequestId));
        }
        #endregion

        #region GetAuditLogsForDataShareRequestStatusChangeToStatusAsync() Tests
        [Test]
        public async Task GivenADataShareRequestAndStatus_WhenIGetAuditLogsForDataShareRequestStatusChangeToStatusAsync_ThenAuditLogsForDataShareRequestStatusChangeToThatStatusAreReturned()
        {
            var testItems = CreateTestItems();

            var testDataShareRequestId = testItems.Fixture.Create<Guid>();

            var testStatusTypes = new List<DataShareRequestStatusType> { DataShareRequestStatusType.Accepted, DataShareRequestStatusType.Rejected };

            var testAuditLogDataShareRequestStatusChangeModelDatas = testItems.Fixture
                .Build<AuditLogDataShareRequestStatusChangeModelData>()
                .CreateMany();

            testItems.MockAuditLogRepository
                .Setup(x => x.GetAuditLogsForDataShareRequestStatusChangesSetAsync(
                    testDataShareRequestId,
                    null,
                    testStatusTypes))
                .ReturnsAsync(testAuditLogDataShareRequestStatusChangeModelDatas);
            
            var result = await testItems.AuditLogService.GetAuditLogsForDataShareRequestStatusChangeToStatusAsync(
                testDataShareRequestId, testStatusTypes);

            Assert.Multiple(() =>
            {
                foreach (var testAuditLogDataShareRequestStatusChangeModelData in testAuditLogDataShareRequestStatusChangeModelDatas)
                {
                    Assert.That(result.FirstOrDefault(x =>
                            x!.AuditLogDataShareRequestStatusChange_Id == testAuditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_Id),
                        Is.Not.Null);
                }
            });
        }
        #endregion

        #region GetMostRecentAuditLogForDataShareRequestStatusChangeAsync() Tests
        [Test]
        public async Task GivenADataShareRequestId_WhenIGetMostRecentAuditLogForDataShareRequestStatusChangeAsync_ThenTheQueryToTheRepositoryIsMadeUsingTheGivenDataShareRequestId()
        {
            var testItems = CreateTestItems();

            var testDataShareRequestId = testItems.Fixture.Create<Guid>();

            await testItems.AuditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeAsync(
                testDataShareRequestId);

            testItems.MockAuditLogRepository.Verify(x => x.GetAuditLogsForDataShareRequestStatusChangesAsync(
                    testDataShareRequestId, null, null),
                Times.Once);
        }

        [Test]
        public async Task GivenNoAuditLogsAreFoundForTheDataShareRequest_WhenIGetMostRecentAuditLogForDataShareRequestStatusChangeAsync_ThenNullIsReturned()
        {
            var testItems = CreateTestItems();

            var result = await testItems.AuditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeAsync(
                It.IsAny<Guid>());

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GivenASingleAuditLogsIsFoundForTheDataShareRequest_WhenIGetMostRecentAuditLogForDataShareRequestStatusChangeAsync_ThenThatAuditLogIsReturned()
        {
            var testItems = CreateTestItems();

            var testAuditLogDataShareRequestStatusChangeModelData = CreateTestAuditLogDataShareRequestStatusChangeModelData();

            testItems.MockAuditLogRepository.Setup(x => x.GetAuditLogsForDataShareRequestStatusChangesAsync(
                    It.IsAny<Guid>(), null, null))
                .ReturnsAsync(() => [testAuditLogDataShareRequestStatusChangeModelData]);

            var result = await testItems.AuditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeAsync(
                It.IsAny<Guid>());

            Assert.That(result, Is.EqualTo(testAuditLogDataShareRequestStatusChangeModelData));
        }

        [Test]
        public async Task GivenAMultipleAuditLogsAreFoundForTheDataShareRequest_WhenIGetMostRecentAuditLogForDataShareRequestStatusChangeAsync_ThenTheMostRecentAuditLogIsReturned()
        {
            var testItems = CreateTestItems();

            var baseChangedAtTime = testItems.Fixture.Create<DateTime>();

            var testAuditLogDataShareRequestStatusChangeModelData1 = CreateTestAuditLogDataShareRequestStatusChangeModelData(changedAt: baseChangedAtTime.AddSeconds(1));
            var testAuditLogDataShareRequestStatusChangeModelData2 = CreateTestAuditLogDataShareRequestStatusChangeModelData(changedAt: baseChangedAtTime);
            var testAuditLogDataShareRequestStatusChangeModelData3 = CreateTestAuditLogDataShareRequestStatusChangeModelData(changedAt: baseChangedAtTime.AddSeconds(2));
            var testAuditLogDataShareRequestStatusChangeModelData4 = CreateTestAuditLogDataShareRequestStatusChangeModelData(changedAt: baseChangedAtTime.AddSeconds(-1));

            var testAuditLogDataShareRequestStatusChangeModelDatas = new List<AuditLogDataShareRequestStatusChangeModelData>
            {
                testAuditLogDataShareRequestStatusChangeModelData1,
                testAuditLogDataShareRequestStatusChangeModelData2,
                testAuditLogDataShareRequestStatusChangeModelData3,
                testAuditLogDataShareRequestStatusChangeModelData4
            };

            testItems.MockAuditLogRepository.Setup(x => x.GetAuditLogsForDataShareRequestStatusChangesAsync(
                    It.IsAny<Guid>(), null, null))
                .ReturnsAsync(() => testAuditLogDataShareRequestStatusChangeModelDatas);

            var result = await testItems.AuditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeAsync(
                It.IsAny<Guid>());

            Assert.That(result, Is.EqualTo(testAuditLogDataShareRequestStatusChangeModelData3));
        }
        #endregion

        #region GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync() Tests
        [Test]
        public async Task GivenADataShareRequestId_WhenIGetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync_ThenTheQueryToTheRepositoryIsMadeUsingTheGivenDataShareRequestId()
        {
            var testItems = CreateTestItems();

            var testDataShareRequestId = testItems.Fixture.Create<Guid>();

            await testItems.AuditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                testDataShareRequestId, It.IsAny<DataShareRequestStatusType>());

            testItems.MockAuditLogRepository.Verify(x => x.GetAuditLogsForDataShareRequestStatusChangesAsync(
                    testDataShareRequestId, null, It.IsAny<DataShareRequestStatusType>()),
                Times.Once);
        }


        [Theory]
        public async Task GivenADataShareRequestStatus_WhenIGetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync_ThenTheQueryToTheRepositoryIsMadeUsingTheGivenDataShareRequestStatus(
            DataShareRequestStatusType? dataShareRequestStatusType)
        {
            var testItems = CreateTestItems();

            await testItems.AuditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                It.IsAny<Guid>(), dataShareRequestStatusType);

            testItems.MockAuditLogRepository.Verify(x => x.GetAuditLogsForDataShareRequestStatusChangesAsync(
                    It.IsAny<Guid>(), null, dataShareRequestStatusType),
                Times.Once);
        }

        [Test]
        public async Task GivenNoAuditLogsAreFoundForTheDataShareRequest_WhenIGetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync_ThenNullIsReturned()
        {
            var testItems = CreateTestItems();

            var result = await testItems.AuditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                It.IsAny<Guid>(),
                It.IsAny<DataShareRequestStatusType>());

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GivenASingleAuditLogsIsFoundForTheDataShareRequest_WhenIGetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync_ThenThatAuditLogIsReturned()
        {
            var testItems = CreateTestItems();

            var testAuditLogDataShareRequestStatusChangeModelData = CreateTestAuditLogDataShareRequestStatusChangeModelData();

            testItems.MockAuditLogRepository.Setup(x => x.GetAuditLogsForDataShareRequestStatusChangesAsync(
                    It.IsAny<Guid>(), null, It.IsAny<DataShareRequestStatusType>()))
                .ReturnsAsync(() => [testAuditLogDataShareRequestStatusChangeModelData]);

            var result = await testItems.AuditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                It.IsAny<Guid>(), It.IsAny<DataShareRequestStatusType>());

            Assert.That(result, Is.EqualTo(testAuditLogDataShareRequestStatusChangeModelData));
        }

        [Test]
        public async Task GivenAMultipleAuditLogsAreFoundForTheDataShareRequest_WhenIGetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync_ThenTheMostRecentAuditLogIsReturned()
        {
            var testItems = CreateTestItems();

            var baseChangedAtTime = testItems.Fixture.Create<DateTime>();

            var testAuditLogDataShareRequestStatusChangeModelData1 = CreateTestAuditLogDataShareRequestStatusChangeModelData(changedAt: baseChangedAtTime.AddSeconds(1));
            var testAuditLogDataShareRequestStatusChangeModelData2 = CreateTestAuditLogDataShareRequestStatusChangeModelData(changedAt: baseChangedAtTime);
            var testAuditLogDataShareRequestStatusChangeModelData3 = CreateTestAuditLogDataShareRequestStatusChangeModelData(changedAt: baseChangedAtTime.AddSeconds(2));
            var testAuditLogDataShareRequestStatusChangeModelData4 = CreateTestAuditLogDataShareRequestStatusChangeModelData(changedAt: baseChangedAtTime.AddSeconds(-1));

            var testAuditLogDataShareRequestStatusChangeModelDatas = new List<AuditLogDataShareRequestStatusChangeModelData>
            {
                testAuditLogDataShareRequestStatusChangeModelData1,
                testAuditLogDataShareRequestStatusChangeModelData2,
                testAuditLogDataShareRequestStatusChangeModelData3,
                testAuditLogDataShareRequestStatusChangeModelData4
            };

            testItems.MockAuditLogRepository.Setup(x => x.GetAuditLogsForDataShareRequestStatusChangesAsync(
                    It.IsAny<Guid>(), null, It.IsAny<DataShareRequestStatusType>()))
                .ReturnsAsync(() => testAuditLogDataShareRequestStatusChangeModelDatas);

            var result = await testItems.AuditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                It.IsAny<Guid>(), It.IsAny<DataShareRequestStatusType>());

            Assert.That(result, Is.EqualTo(testAuditLogDataShareRequestStatusChangeModelData3));
        }
        #endregion

        #region Test Data Creation
        private static AuditLogDataShareRequestStatusChangeModelData CreateTestAuditLogDataShareRequestStatusChangeModelData(
            DateTime? changedAt = null)
        {
            return new AuditLogDataShareRequestStatusChangeModelData
            {
                AuditLogDataShareRequestStatusChange_ChangedAtUtc = changedAt ?? It.IsAny<DateTime>()
            };
        }
        #endregion

        #region Test Item Creation
        private static TestItems CreateTestItems()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var mockAuditLogModelDataFactory = Mock.Get(fixture.Create<IAuditLogModelDataFactory>());
            var mockAuditLogRepository = Mock.Get(fixture.Create<IAuditLogRepository>());
            var mockServiceOperationResultFactory = Mock.Get(fixture.Create<IServiceOperationResultFactory>());

            var auditLogService = new AuditLogService(
                mockAuditLogModelDataFactory.Object,
                mockAuditLogRepository.Object,
                mockServiceOperationResultFactory.Object);

            return new TestItems(
                fixture,
                auditLogService,
                mockAuditLogModelDataFactory,
                mockAuditLogRepository,
                mockServiceOperationResultFactory);
        }

        private class TestItems(
            IFixture fixture,
            IAuditLogService auditLogService,
            Mock<IAuditLogModelDataFactory> mockAuditLogModelDataFactory,
            Mock<IAuditLogRepository> mockAuditLogRepository,
            Mock<IServiceOperationResultFactory> mockServiceOperationResultFactory)
        {
            public IFixture Fixture { get; } = fixture;
            public IAuditLogService AuditLogService { get; } = auditLogService;
            public Mock<IAuditLogModelDataFactory> MockAuditLogModelDataFactory { get; } = mockAuditLogModelDataFactory;
            public Mock<IAuditLogRepository> MockAuditLogRepository { get; } = mockAuditLogRepository;
            public Mock<IServiceOperationResultFactory> MockServiceOperationResultFactory { get; } = mockServiceOperationResultFactory;
        }
        #endregion
    }
}
