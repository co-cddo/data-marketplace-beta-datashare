using System.Data;
using Agrimetrics.DataShare.Api.Db.DbAccess;
using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.ModelData.Reporting;
using Agrimetrics.DataShare.Api.Logic.Repositories.Reporting;
using Agrimetrics.DataShare.Api.Logic.Test.TestHelpers;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Repositories.Reporting;

[TestFixture]
public class ReportingRepositoryTests
{
    #region GetAllReportingDataShareRequestInformationAsync() Tests
    [Test]
    public async Task GivenAReportingRepository_WhenIGetAllReportingDataShareRequestInformationAsync_ThenReportingDataShareRequestInformationModelDataIsObtainedFromTheDatabase()
    {
        var testItems = CreateTestItems();

        testItems.MockReportingSqlQueries.SetupGet(x => x.GetAllReportingDataShareRequestInformation)
            .Returns(() => "test sql query");

        var testReportingDataShareRequestInformationModelDatasFlattened = testItems.Fixture
            .Build<ReportingDataShareRequestInformationModelData>()
            .CreateMany().ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<Func<ReportingDataShareRequestInformationModelData, ReportingDataShareRequestStatusModelData, ReportingDataShareRequestInformationModelData>>(),
                nameof(ReportingDataShareRequestStatusModelData.Status_Id),
                null))
            .ReturnsAsync(() => testReportingDataShareRequestInformationModelDatasFlattened);

        var result = (await testItems.ReportingRepository.GetAllReportingDataShareRequestInformationAsync()).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Exactly(testReportingDataShareRequestInformationModelDatasFlattened.Count).Items);

            foreach (var testReportingDataShareRequestInformationModelData in testReportingDataShareRequestInformationModelDatasFlattened)
            {
                var resultReportingDataShareRequestInformationModelData = result.FirstOrDefault(x =>
                    x.DataShareRequest_Id == testReportingDataShareRequestInformationModelData.DataShareRequest_Id);

                Assert.That(resultReportingDataShareRequestInformationModelData, Is.Not.Null);
            }
        });
    }

    [Test]
    public void GivenGettingReportingDataShareRequestInformationWillFail_WhenIGetAllReportingDataShareRequestInformationAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testException = testItems.Fixture.Create<Exception>();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                It.IsAny<string>(),
                It.IsAny<Func<ReportingDataShareRequestInformationModelData, ReportingDataShareRequestStatusModelData,
                    ReportingDataShareRequestInformationModelData>>(),
                nameof(ReportingDataShareRequestStatusModelData.Status_Id),
                null))
            .Throws(() => testException);

        Assert.That(async () => await testItems.ReportingRepository.GetAllReportingDataShareRequestInformationAsync(),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.Message.EqualTo("Failed to GetAllReportingDataShareRequestInformation from database").And
                .With.InnerException.SameAs(testException));
    }

    [Test]
    public async Task GivenAReportingRepository_WhenIGetAllReportingDataShareRequestInformationAsync_ThenTheMappingFunctionIsRun()
    {
        var testItems = CreateTestItems();

        testItems.MockReportingSqlQueries.SetupGet(x => x.GetAllReportingDataShareRequestInformation)
            .Returns(() => "test sql query");

        var testReportingDataShareRequestInformationModelDatasFlattened = testItems.Fixture
            .Build<ReportingDataShareRequestInformationModelData>()
            .CreateMany().ToList();

        var mappingFunctionHasBeenRun = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<Func<ReportingDataShareRequestInformationModelData, ReportingDataShareRequestStatusModelData, ReportingDataShareRequestInformationModelData>>(),
                nameof(ReportingDataShareRequestStatusModelData.Status_Id),
                null))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<ReportingDataShareRequestInformationModelData, ReportingDataShareRequestStatusModelData, ReportingDataShareRequestInformationModelData> mappingFunc,
                string _,
                object? _) =>
            {
                mappingFunctionHasBeenRun = true;

                mappingFunc(
                    testItems.Fixture.Create<ReportingDataShareRequestInformationModelData>(),
                    testItems.Fixture.Create<ReportingDataShareRequestStatusModelData>());
            })
            .ReturnsAsync(() => testReportingDataShareRequestInformationModelDatasFlattened);

        await testItems.ReportingRepository.GetAllReportingDataShareRequestInformationAsync();

        Assert.That(mappingFunctionHasBeenRun, Is.True);
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockLogger = Mock.Get(fixture.Freeze<ILogger<ReportingRepository>>());
        var mockDatabaseChannelCreation = Mock.Get(fixture.Freeze<IDatabaseChannelCreation>());

        var mockDatabaseChannelResources = mockDatabaseChannelCreation.CreateTestableDatabaseChannelResources(fixture);
        var mockDatabaseCommandRunner = Mock.Get(fixture.Freeze<IDatabaseCommandRunner>());
        var mockReportingSqlQueries = Mock.Get(fixture.Create<IReportingSqlQueries>());

        var reportingRepository = new ReportingRepository(
            mockLogger.Object,
            mockDatabaseChannelCreation.Object,
            mockDatabaseCommandRunner.Object,
            mockReportingSqlQueries.Object);

        return new TestItems(
            fixture,
            reportingRepository,
            mockDatabaseChannelResources.MockDbConnection,
            mockDatabaseChannelResources.MockDbTransaction,
            mockDatabaseCommandRunner,
            mockReportingSqlQueries);

    }

    private class TestItems(
        IFixture fixture,
        IReportingRepository reportingRepository,
        Mock<IDbConnection> mockDbConnection,
        Mock<IDbTransaction> mockDbTransaction,
        Mock<IDatabaseCommandRunner> mockDatabaseCommandRunner,
        Mock<IReportingSqlQueries> mockReportingSqlQueries)
    {
        public IFixture Fixture { get; } = fixture;
        public IReportingRepository ReportingRepository { get; } = reportingRepository;
        public Mock<IDbConnection> MockDbConnection { get; } = mockDbConnection;
        public Mock<IDbTransaction> MockDbTransaction { get; } = mockDbTransaction;
        public Mock<IDatabaseCommandRunner> MockDatabaseCommandRunner { get; } = mockDatabaseCommandRunner;
        public Mock<IReportingSqlQueries> MockReportingSqlQueries { get; } = mockReportingSqlQueries;
    }
    #endregion
}