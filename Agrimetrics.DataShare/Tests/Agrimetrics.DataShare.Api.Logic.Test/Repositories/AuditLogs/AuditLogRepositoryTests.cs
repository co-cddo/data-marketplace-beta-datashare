using Agrimetrics.DataShare.Api.Db.DbAccess;
using AutoFixture.AutoMoq;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Data;
using Agrimetrics.DataShare.Api.Logic.Repositories.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.Test.TestHelpers;
using Agrimetrics.DataShare.Api.Logic.Repositories.AuditLogs.ParameterModels;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using System.ComponentModel;
using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;

namespace Agrimetrics.DataShare.Api.Logic.Test.Repositories.AuditLogs;

[TestFixture]
public class AuditLogRepositoryTests
{
    #region RecordDataShareRequestStatusChangeAsync() Tests
    [Test]
    public void GivenAnInvalidFromStatus_WhenIRecordDataShareRequestStatusChangeAsync_ThenAnInvalidEnumArgumentExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestStatusType = (DataShareRequestStatusType) Enum.GetValues<DataShareRequestStatusType>().Cast<int>().Max() + 1;

        var testRecordDataShareRequestStatusChangeParameters = testItems.Fixture
            .Build<RecordDataShareRequestStatusChangeParameters>()
            .With(x => x.FromStatus, testDataShareRequestStatusType)
            .Create();

        Assert.That(() => testItems.KeyAuditLogRepository.RecordDataShareRequestStatusChangeAsync(testRecordDataShareRequestStatusChangeParameters),
            Throws.TypeOf<InvalidEnumArgumentException>().With.Property("ParamName").EqualTo(nameof(testRecordDataShareRequestStatusChangeParameters.FromStatus)));
    }

    [Test]
    public void GivenAnInvalidToStatus_WhenIRecordDataShareRequestStatusChangeAsync_ThenAnInvalidEnumArgumentExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestStatusType = (DataShareRequestStatusType)Enum.GetValues<DataShareRequestStatusType>().Cast<int>().Max() + 1;

        var testRecordDataShareRequestStatusChangeParameters = testItems.Fixture
            .Build<RecordDataShareRequestStatusChangeParameters>()
            .With(x => x.ToStatus, testDataShareRequestStatusType)
            .Create();

        Assert.That(() => testItems.KeyAuditLogRepository.RecordDataShareRequestStatusChangeAsync(testRecordDataShareRequestStatusChangeParameters),
            Throws.TypeOf<InvalidEnumArgumentException>().With.Property("ParamName").EqualTo(nameof(testRecordDataShareRequestStatusChangeParameters.ToStatus)));
    }

    [Test]
    public async Task GivenRecordDataShareRequestStatusChangeParameters_WhenIRecordDataShareRequestStatusChangeAsync_ThenTheGuidOfTheCreatedAuditLogEntryIsReturned()
    {
        var testItems = CreateTestItems();

        var testRecordDataShareRequestStatusChangeParameters = testItems.Fixture
            .Build<RecordDataShareRequestStatusChangeParameters>()
            .Create();

        testItems.MockAuditLogSqlQueries.SetupGet(x => x.RecordDataShareRequestStatusChange)
            .Returns("test sql command");

        var testAuditLogId = testItems.Fixture.Create<Guid>();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteScalarAsync<Guid>(
            testItems.MockDbConnection.Object,
            testItems.MockDbTransaction.Object,
            "test sql command",
            It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return 
                    dynamicParameters.DataShareRequestId == testRecordDataShareRequestStatusChangeParameters.DataShareRequestId &&
                    dynamicParameters.FromStatus == testRecordDataShareRequestStatusChangeParameters.FromStatus.ToString() &&
                    dynamicParameters.ToStatus == testRecordDataShareRequestStatusChangeParameters.ToStatus.ToString() &&
                    dynamicParameters.ChangedByUserId == testRecordDataShareRequestStatusChangeParameters.ChangedByUser.UserId &&
                    dynamicParameters.ChangedByUserOrganisationId == testRecordDataShareRequestStatusChangeParameters.ChangedByUser.OrganisationId
                    ? testAuditLogId
                    : Guid.Empty;
            });

        var result = await testItems.KeyAuditLogRepository.RecordDataShareRequestStatusChangeAsync(
            testRecordDataShareRequestStatusChangeParameters);

        Assert.That(result, Is.EqualTo(testAuditLogId));
    }

    [Test]
    public void GivenRecordingDataShareRequestStatusChangeWillFail_WhenIRecordDataShareRequestStatusChangeAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testRecordDataShareRequestStatusChangeParameters = testItems.Fixture
            .Build<RecordDataShareRequestStatusChangeParameters>()
            .Create();

        testItems.MockAuditLogSqlQueries.SetupGet(x => x.RecordDataShareRequestStatusChange)
            .Returns("test sql command");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteScalarAsync<Guid>(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.KeyAuditLogRepository.RecordDataShareRequestStatusChangeAsync(
            testRecordDataShareRequestStatusChangeParameters),
                Throws.TypeOf<DatabaseAccessGeneralException>()
                    .With.Message.EqualTo("Failed to RecordDataShareRequestStatusChange in database").And
                    .With.InnerException.With.Message.EqualTo("Failed to RecordStatusChange in database"));
    }
    #endregion

    #region RecordDataShareRequestStatusChangeWithCommentsAsync() Tests
    [Test]
    public void GivenAnInvalidFromStatus_WhenIRecordDataShareRequestStatusChangeWithCommentsAsync_ThenAnInvalidEnumArgumentExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestStatusType = (DataShareRequestStatusType)Enum.GetValues<DataShareRequestStatusType>().Cast<int>().Max() + 1;

        var testIRecordDataShareRequestStatusChangeWithCommentsParameters = testItems.Fixture
            .Build<RecordDataShareRequestStatusChangeWithCommentsParameters>()
            .With(x => x.FromStatus, testDataShareRequestStatusType)
            .Create();

        Assert.That(() => testItems.KeyAuditLogRepository.RecordDataShareRequestStatusChangeWithCommentsAsync(testIRecordDataShareRequestStatusChangeWithCommentsParameters),
            Throws.TypeOf<InvalidEnumArgumentException>().With.Property("ParamName").EqualTo(nameof(testIRecordDataShareRequestStatusChangeWithCommentsParameters.FromStatus)));
    }

    [Test]
    public void GivenAnInvalidToStatus_WhenIRecordDataShareRequestStatusChangeWithCommentsAsync_ThenAnInvalidEnumArgumentExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestStatusType = (DataShareRequestStatusType)Enum.GetValues<DataShareRequestStatusType>().Cast<int>().Max() + 1;

        var testIRecordDataShareRequestStatusChangeWithCommentsParameters = testItems.Fixture
            .Build<RecordDataShareRequestStatusChangeWithCommentsParameters>()
            .With(x => x.ToStatus, testDataShareRequestStatusType)
            .Create();

        Assert.That(() => testItems.KeyAuditLogRepository.RecordDataShareRequestStatusChangeWithCommentsAsync(testIRecordDataShareRequestStatusChangeWithCommentsParameters),
            Throws.TypeOf<InvalidEnumArgumentException>().With.Property("ParamName").EqualTo(nameof(testIRecordDataShareRequestStatusChangeWithCommentsParameters.ToStatus)));
    }

    [Test]
    public void GivenANullSetOfComments_WhenIRecordDataShareRequestStatusChangeWithCommentsAsync_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testIRecordDataShareRequestStatusChangeWithCommentsParameters = testItems.Fixture
            .Build<RecordDataShareRequestStatusChangeWithCommentsParameters>()
            .With(x => x.Comments, (IEnumerable<string>)null!)
            .Create();

        Assert.That(() => testItems.KeyAuditLogRepository.RecordDataShareRequestStatusChangeWithCommentsAsync(testIRecordDataShareRequestStatusChangeWithCommentsParameters),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo(nameof(testIRecordDataShareRequestStatusChangeWithCommentsParameters.Comments)));
    }

    [Test]
    public async Task GivenRecordDataShareRequestStatusChangeWithCommentsParameters_WhenIRecordDataShareRequestStatusChangeWithCommentsAsync_ThenTheGuidOfTheCreatedAuditLogEntryIsReturned()
    {
        var testItems = CreateTestItems();

        var testRecordDataShareRequestStatusChangeWithCommentsParameters = testItems.Fixture
            .Build<RecordDataShareRequestStatusChangeWithCommentsParameters>()
            .Create();

        testItems.MockAuditLogSqlQueries.SetupGet(x => x.RecordDataShareRequestStatusChange)
            .Returns("test sql command");

        var testAuditLogId = testItems.Fixture.Create<Guid>();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteScalarAsync<Guid>(
            testItems.MockDbConnection.Object,
            testItems.MockDbTransaction.Object,
            "test sql command",
            It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                return
                    dynamicParameters.DataShareRequestId == testRecordDataShareRequestStatusChangeWithCommentsParameters.DataShareRequestId &&
                    dynamicParameters.FromStatus == testRecordDataShareRequestStatusChangeWithCommentsParameters.FromStatus.ToString() &&
                    dynamicParameters.ToStatus == testRecordDataShareRequestStatusChangeWithCommentsParameters.ToStatus.ToString() &&
                    dynamicParameters.ChangedByUserId == testRecordDataShareRequestStatusChangeWithCommentsParameters.ChangedByUser.UserId &&
                    dynamicParameters.ChangedByUserOrganisationId == testRecordDataShareRequestStatusChangeWithCommentsParameters.ChangedByUser.OrganisationId
                    ? testAuditLogId
                    : Guid.Empty;
            });

        var result = await testItems.KeyAuditLogRepository.RecordDataShareRequestStatusChangeWithCommentsAsync(
            testRecordDataShareRequestStatusChangeWithCommentsParameters);

        Assert.That(result, Is.EqualTo(testAuditLogId));
    }

    [Test]
    public void GivenRecordingDataShareRequestStatusChangeWithCommentsWillFail_WhenIRecordDataShareRequestStatusChangeWithCommentsAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testRecordDataShareRequestStatusChangeWithCommentsParameters = testItems.Fixture
            .Build<RecordDataShareRequestStatusChangeWithCommentsParameters>()
            .Create();

        testItems.MockAuditLogSqlQueries.SetupGet(x => x.RecordDataShareRequestStatusChange)
            .Returns("test sql command");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteScalarAsync<Guid>(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                "test sql command",
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.KeyAuditLogRepository.RecordDataShareRequestStatusChangeWithCommentsAsync(
            testRecordDataShareRequestStatusChangeWithCommentsParameters),
                Throws.TypeOf<DatabaseAccessGeneralException>()
                    .With.Message.EqualTo("Failed to RecordDataShareRequestStatusChangeWithComments in database").And
                    .With.InnerException.With.Message.EqualTo("Failed to RecordStatusChange in database"));
    }

    [Test]
    public void GivenRecordingADataShareRequestStatusChangeCommentWillFail_WhenIRecordDataShareRequestStatusChangeWithCommentsAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testRecordDataShareRequestStatusChangeWithCommentsParameters = testItems.Fixture
            .Build<RecordDataShareRequestStatusChangeWithCommentsParameters>()
            .Create();

        testItems.MockAuditLogSqlQueries.SetupGet(x => x.RecordDataShareRequestStatusChangeComment)
            .Returns("test sql command");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteScalarAsync(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                "test sql command",
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.KeyAuditLogRepository.RecordDataShareRequestStatusChangeWithCommentsAsync(
                testRecordDataShareRequestStatusChangeWithCommentsParameters),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.Message.EqualTo("Failed to RecordDataShareRequestStatusChangeWithComments in database").And
                .With.InnerException.With.Message.EqualTo("Failed to RecordStatusChange in database"));
    }
    #endregion

    #region GetAuditLogsForDataShareRequestStatusChangesAsync() Tests
    [Test]
    public async Task GivenADataShareRequestId_WhenIGetAuditLogsForDataShareRequestStatusChangesAsync_ThenTheSetOfStatusChangesForThatRequestIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockAuditLogSqlQueries.SetupGet(x => x.GetDataShareRequestStatusChanges)
            .Returns("test sql query");

        var testAuditLogDataShareRequestStatusChangeModelData = testItems.Fixture
            .Build<AuditLogDataShareRequestStatusChangeModelData>()
            .With(x => x.AuditLogDataShareRequestStatusChange_Id, testDataShareRequestId)
            .CreateMany(1).ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
            testItems.MockDbConnection.Object,
            testItems.MockDbTransaction.Object,
            "test sql query",
            It.IsAny<Func<AuditLogDataShareRequestStatusChangeModelData, AuditLogDataShareRequestStatusChangeCommentModelData, AuditLogDataShareRequestStatusChangeModelData>>(),
            nameof(AuditLogDataShareRequestStatusChangeCommentModelData.AuditLogDataShareRequestStatusChangeComment_Id),
            It.IsAny<object?>()))
            .ReturnsAsync(() => testAuditLogDataShareRequestStatusChangeModelData);

        var result = (await testItems.KeyAuditLogRepository.GetAuditLogsForDataShareRequestStatusChangesAsync(
            testDataShareRequestId,
            null,
            testItems.Fixture.Create<DataShareRequestStatusType?>())).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.One.Items);

            Assert.That(result.Single().AuditLogDataShareRequestStatusChange_Id, Is.EqualTo(testDataShareRequestId));
        });
    }

    [Test]
    public async Task GivenAResultMappingFunction_WhenIGetAuditLogsForDataShareRequestStatusChangesAsync_ThenTheMappingFunctionIsRun()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockAuditLogSqlQueries.SetupGet(x => x.GetDataShareRequestStatusChanges)
            .Returns("test sql query");

        var testAuditLogDataShareRequestStatusChangeModelData = testItems.Fixture
            .Build<AuditLogDataShareRequestStatusChangeModelData>()
            .With(x => x.AuditLogDataShareRequestStatusChange_Id, testDataShareRequestId)
            .CreateMany(1).ToList();

        var mappingFunctionHasBeenRun = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<Func<AuditLogDataShareRequestStatusChangeModelData, AuditLogDataShareRequestStatusChangeCommentModelData, AuditLogDataShareRequestStatusChangeModelData>>(),
                nameof(AuditLogDataShareRequestStatusChangeCommentModelData.AuditLogDataShareRequestStatusChangeComment_Id),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<AuditLogDataShareRequestStatusChangeModelData, AuditLogDataShareRequestStatusChangeCommentModelData, AuditLogDataShareRequestStatusChangeModelData> mappingFunction,
                string _,
                object? _) =>
            {
                mappingFunctionHasBeenRun = true;

                mappingFunction(
                    testItems.Fixture.Create<AuditLogDataShareRequestStatusChangeModelData>(),
                    testItems.Fixture.Create<AuditLogDataShareRequestStatusChangeCommentModelData>());
            })
            .ReturnsAsync(() => testAuditLogDataShareRequestStatusChangeModelData);

        await testItems.KeyAuditLogRepository.GetAuditLogsForDataShareRequestStatusChangesAsync(
            testDataShareRequestId,
            testItems.Fixture.Create<DataShareRequestStatusType?>(),
            testItems.Fixture.Create<DataShareRequestStatusType?>());

        Assert.That(mappingFunctionHasBeenRun, Is.True);
    }

    [Test]
    public void GivenGettingAuditLogInformationWillFail_WhenIGetAuditLogsForDataShareRequestStatusChangesAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<Func<AuditLogDataShareRequestStatusChangeModelData, AuditLogDataShareRequestStatusChangeCommentModelData, AuditLogDataShareRequestStatusChangeModelData>>(),
                nameof(AuditLogDataShareRequestStatusChangeCommentModelData.AuditLogDataShareRequestStatusChangeComment_Id),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.KeyAuditLogRepository.GetAuditLogsForDataShareRequestStatusChangesAsync(
                testItems.Fixture.Create<Guid>(),
                null,
                testItems.Fixture.Create<DataShareRequestStatusType?>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to GetAuditLogsForDataShareRequestStatusChanges in database"));
    }
    #endregion

    #region GetAuditLogsForDataShareRequestStatusChangesSetAsync() Tests
    [Test]
    public async Task GivenADataShareRequestId_WhenIGetAuditLogsForDataShareRequestStatusChangesSetAsync_ThenTheSetOfStatusChangesForThatRequestIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockAuditLogSqlQueries.SetupGet(x => x.GetDataShareRequestStatusChanges)
            .Returns("test sql query");

        var testAuditLogDataShareRequestStatusChangeModelData = testItems.Fixture
            .Build<AuditLogDataShareRequestStatusChangeModelData>()
            .With(x => x.AuditLogDataShareRequestStatusChange_Id, testDataShareRequestId)
            .CreateMany(1).ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
            testItems.MockDbConnection.Object,
            testItems.MockDbTransaction.Object,
            "test sql query",
            It.IsAny<Func<AuditLogDataShareRequestStatusChangeModelData, AuditLogDataShareRequestStatusChangeCommentModelData, AuditLogDataShareRequestStatusChangeModelData>>(),
            nameof(AuditLogDataShareRequestStatusChangeCommentModelData.AuditLogDataShareRequestStatusChangeComment_Id),
            It.IsAny<object?>()))
            .ReturnsAsync(() => testAuditLogDataShareRequestStatusChangeModelData);

        var result = (await testItems.KeyAuditLogRepository.GetAuditLogsForDataShareRequestStatusChangesSetAsync(
            testDataShareRequestId,
            null,
            testItems.Fixture.CreateMany<DataShareRequestStatusType>())).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.One.Items);

            Assert.That(result.Single().AuditLogDataShareRequestStatusChange_Id, Is.EqualTo(testDataShareRequestId));
        });
    }

    [Test]
    public async Task GivenAResultMappingFunction_WhenIGetAuditLogsForDataShareRequestStatusChangesSetAsync_ThenTheMappingFunctionIsRun()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockAuditLogSqlQueries.SetupGet(x => x.GetDataShareRequestStatusChanges)
            .Returns("test sql query");

        var testAuditLogDataShareRequestStatusChangeModelData = testItems.Fixture
            .Build<AuditLogDataShareRequestStatusChangeModelData>()
            .With(x => x.AuditLogDataShareRequestStatusChange_Id, testDataShareRequestId)
            .CreateMany(1).ToList();

        var mappingFunctionHasBeenRun = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<Func<AuditLogDataShareRequestStatusChangeModelData, AuditLogDataShareRequestStatusChangeCommentModelData, AuditLogDataShareRequestStatusChangeModelData>>(),
                nameof(AuditLogDataShareRequestStatusChangeCommentModelData.AuditLogDataShareRequestStatusChangeComment_Id),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<AuditLogDataShareRequestStatusChangeModelData, AuditLogDataShareRequestStatusChangeCommentModelData, AuditLogDataShareRequestStatusChangeModelData> mappingFunction,
                string _,
                object? _) =>
            {
                mappingFunctionHasBeenRun = true;

                mappingFunction(
                    testItems.Fixture.Create<AuditLogDataShareRequestStatusChangeModelData>(),
                    testItems.Fixture.Create<AuditLogDataShareRequestStatusChangeCommentModelData>());
            })
            .ReturnsAsync(() => testAuditLogDataShareRequestStatusChangeModelData);

        await testItems.KeyAuditLogRepository.GetAuditLogsForDataShareRequestStatusChangesSetAsync(
            testDataShareRequestId,
            testItems.Fixture.CreateMany<DataShareRequestStatusType>(),
            testItems.Fixture.CreateMany<DataShareRequestStatusType>());

        Assert.That(mappingFunctionHasBeenRun, Is.True);
    }

    [Test]
    public void GivenGettingAuditLogInformationWillFail_WhenIGetAuditLogsForDataShareRequestStatusChangesSetAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                It.IsAny<IDbConnection>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<string>(),
                It.IsAny<Func<AuditLogDataShareRequestStatusChangeModelData, AuditLogDataShareRequestStatusChangeCommentModelData, AuditLogDataShareRequestStatusChangeModelData>>(),
                nameof(AuditLogDataShareRequestStatusChangeCommentModelData.AuditLogDataShareRequestStatusChangeComment_Id),
                It.IsAny<object?>()))
            .Throws(() => testItems.Fixture.Create<Exception>());

        Assert.That(async () => await testItems.KeyAuditLogRepository.GetAuditLogsForDataShareRequestStatusChangesSetAsync(
                testItems.Fixture.Create<Guid>(),
                null,
                testItems.Fixture.CreateMany<DataShareRequestStatusType>()),
            Throws.TypeOf<DatabaseAccessGeneralException>().With.Message.EqualTo("Failed to GetAuditLogsForDataShareRequestStatusChanges in database"));
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockLogger = Mock.Get(fixture.Freeze<ILogger<AuditLogRepository>>());
        var mockDatabaseChannelCreation = Mock.Get(fixture.Freeze<IDatabaseChannelCreation>());

        var mockDatabaseChannelResources = mockDatabaseChannelCreation.CreateTestableDatabaseChannelResources(fixture);
        var mockDatabaseCommandRunner = Mock.Get(fixture.Freeze<IDatabaseCommandRunner>());
        var mockAuditLogSqlQueries = Mock.Get(fixture.Create<IAuditLogSqlQueries>());

        var keyAuditLogRepository = new AuditLogRepository(
            mockLogger.Object,
            mockDatabaseChannelCreation.Object,
            mockDatabaseCommandRunner.Object,
            mockAuditLogSqlQueries.Object);

        return new TestItems(
            fixture,
            keyAuditLogRepository,
            mockDatabaseChannelResources.MockDbConnection,
            mockDatabaseChannelResources.MockDbTransaction,
            mockDatabaseCommandRunner,
            mockAuditLogSqlQueries);
    }

    private class TestItems(
        IFixture fixture,
        IAuditLogRepository keyAuditLogRepository,
        Mock<IDbConnection> mockDbConnection,
        Mock<IDbTransaction> mockDbTransaction,
        Mock<IDatabaseCommandRunner> mockDatabaseCommandRunner,
        Mock<IAuditLogSqlQueries> mockAuditLogSqlQueries)
    {
        public IFixture Fixture { get; } = fixture;
        public IAuditLogRepository KeyAuditLogRepository { get; } = keyAuditLogRepository;
        public Mock<IDbConnection> MockDbConnection { get; } = mockDbConnection;
        public Mock<IDbTransaction> MockDbTransaction { get; } = mockDbTransaction;
        public Mock<IDatabaseCommandRunner> MockDatabaseCommandRunner { get; } = mockDatabaseCommandRunner;
        public Mock<IAuditLogSqlQueries> MockAuditLogSqlQueries { get; } = mockAuditLogSqlQueries;
    }
    #endregion
}
