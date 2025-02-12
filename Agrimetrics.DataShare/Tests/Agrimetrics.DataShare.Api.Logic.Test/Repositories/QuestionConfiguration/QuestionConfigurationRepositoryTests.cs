using Agrimetrics.DataShare.Api.Db.DbAccess;
using AutoFixture.AutoMoq;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Data;
using Agrimetrics.DataShare.Api.Logic.Repositories.QuestionConfiguration;
using Agrimetrics.DataShare.Api.Logic.Test.TestHelpers;
using Agrimetrics.DataShare.Api.Logic.ModelData.QuestionConfiguration.CompulsoryQuestions;
using Agrimetrics.DataShare.Api.Logic.Exceptions;

namespace Agrimetrics.DataShare.Api.Logic.Test.Repositories.QuestionConfiguration;

[TestFixture]
public class QuestionConfigurationRepositoryTests
{
    #region Compulsory Questions Tests
    #region GetCompulsoryQuestionsAsync() Tests
    [Test]
    public async Task GivenAQuestionConfigurationRepository_WhenIGetCompulsoryQuestionsAsync_ThenCompulsoryQuestionModelDataIsObtainedFromTheDatabase()
    {
        var testItems = CreateTestItems();

        testItems.MockQuestionConfigurationSqlQueries.SetupGet(x => x.GetCompulsoryQuestions)
            .Returns("test sql query");

        var testCompulsoryQuestionModelDatas = testItems.Fixture.CreateMany<CompulsoryQuestionModelData>().ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<CompulsoryQuestionModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testCompulsoryQuestionModelDatas);

        var result = (await testItems.QuestionConfigurationRepository.GetCompulsoryQuestionsAsync()).ToList();

        Assert.That(result, Is.EqualTo(testCompulsoryQuestionModelDatas));
    }

    [Test]
    public void GivenGettingCompulsoryQuestionsWillFail_WhenIGetCompulsoryQuestionsAsync_ThenAnDatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockQuestionConfigurationSqlQueries.SetupGet(x => x.GetCompulsoryQuestions)
            .Returns("test sql query");

        var testException = new Exception("test error message");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<CompulsoryQuestionModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<object?>()))
            .Throws(() => testException);

        Assert.That(async () => await testItems.QuestionConfigurationRepository.GetCompulsoryQuestionsAsync(),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.Message.EqualTo("Failed to GetCompulsoryQuestions from database").And
                .With.InnerException.With.Message.EqualTo("test error message"));
    }
    #endregion

    #region SetCompulsoryQuestionAsync() Tests
    [Test]
    public async Task GivenAQuestionId_WhenISetCompulsoryQuestionAsync_ThenTheQuestionIsSetToBeCompulsoryInTheDatabase()
    {
        var testItems = CreateTestItems();

        testItems.MockQuestionConfigurationSqlQueries.SetupGet(x => x.GetCompulsoryQuestions)
            .Returns("test sql query");

        testItems.MockQuestionConfigurationSqlQueries.SetupGet(x => x.SetCompulsoryQuestion)
            .Returns("test sql command");

        var testCompulsoryQuestionModelDatas = testItems.Fixture.CreateMany<CompulsoryQuestionModelData>().ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<CompulsoryQuestionModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testCompulsoryQuestionModelDatas);

        var testQuestionId = testItems.Fixture.Create<Guid>();

        var questionHasBeenSetCompulsory = false;
        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteScalarAsync(
            testItems.MockDbConnection.Object,
            testItems.MockDbTransaction.Object,
            "test sql command",
            It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                questionHasBeenSetCompulsory = dynamicParameters.QuestionId == testQuestionId;
            });

        await testItems.QuestionConfigurationRepository.SetCompulsoryQuestionAsync(testQuestionId);

        Assert.That(questionHasBeenSetCompulsory, Is.True);
    }

    [Test]
    public void GivenAQuestionIdThatIsAlreadyCompulsory_WhenISetCompulsoryQuestionAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockQuestionConfigurationSqlQueries.SetupGet(x => x.GetCompulsoryQuestions)
            .Returns("test sql query");

        testItems.MockQuestionConfigurationSqlQueries.SetupGet(x => x.SetCompulsoryQuestion)
            .Returns("test sql command");

        var testCompulsoryQuestionModelDatas = testItems.Fixture.CreateMany<CompulsoryQuestionModelData>().ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<CompulsoryQuestionModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testCompulsoryQuestionModelDatas);

        var testQuestionId = testCompulsoryQuestionModelDatas.First().CompulsoryQuestion_QuestionId;

        Assert.That(async () => await testItems.QuestionConfigurationRepository.SetCompulsoryQuestionAsync(testQuestionId),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.Message.EqualTo("Failed to SetCompulsoryQuestion in database").And
                .With.InnerException.With.Message.EqualTo("Question is already compulsory"));
    }
    #endregion

    #region ClearCompulsoryQuestionAsync() Tests
    [Test]
    public async Task GivenAQuestionId_WhenIClearCompulsoryQuestionAsync_ThenTheQuestionIsSetToBeNotCompulsoryInTheDatabase()
    {
        var testItems = CreateTestItems();

        testItems.MockQuestionConfigurationSqlQueries.SetupGet(x => x.GetCompulsoryQuestions)
            .Returns("test sql query");

        testItems.MockQuestionConfigurationSqlQueries.SetupGet(x => x.ClearCompulsoryQuestion)
            .Returns("test sql command");

        var testCompulsoryQuestionModelDatas = testItems.Fixture.CreateMany<CompulsoryQuestionModelData>().ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<CompulsoryQuestionModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testCompulsoryQuestionModelDatas);

        var testQuestionId = testCompulsoryQuestionModelDatas.First().CompulsoryQuestion_QuestionId;

        var questionHasBeenSetNotCompulsory = false;
        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteScalarAsync(
            testItems.MockDbConnection.Object,
            testItems.MockDbTransaction.Object,
            "test sql command",
            It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                questionHasBeenSetNotCompulsory = dynamicParameters.QuestionId == testQuestionId;
            });

        await testItems.QuestionConfigurationRepository.ClearCompulsoryQuestionAsync(testQuestionId);

        Assert.That(questionHasBeenSetNotCompulsory, Is.True);
    }

    [Test]
    public void GivenAQuestionIdThatIsNotCompulsory_WhenIClearCompulsoryQuestionAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockQuestionConfigurationSqlQueries.SetupGet(x => x.GetCompulsoryQuestions)
            .Returns("test sql query");

        testItems.MockQuestionConfigurationSqlQueries.SetupGet(x => x.ClearCompulsoryQuestion)
            .Returns("test sql command");

        var testCompulsoryQuestionModelDatas = testItems.Fixture.CreateMany<CompulsoryQuestionModelData>().ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<CompulsoryQuestionModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testCompulsoryQuestionModelDatas);

        var testQuestionId = testItems.Fixture.Create<Guid>();

        Assert.That(async () => await testItems.QuestionConfigurationRepository.ClearCompulsoryQuestionAsync(testQuestionId),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.Message.EqualTo("Failed to ClearCompulsoryQuestion in database").And
                .With.InnerException.With.Message.EqualTo("Question is not compulsory"));
    }
    #endregion
    #endregion

    #region Compulsory Supplier-Mandated Questions Tests
    #region GetCompulsorySupplierMandatedQuestionsAsync() Tests
    [Test]
    public async Task GivenAQuestionConfigurationRepository_WhenIGetCompulsorySupplierMandatedQuestionsAsync_ThenCompulsorySupplierMandatedQuestionModelDataIsObtainedFromTheDatabase()
    {
        var testItems = CreateTestItems();

        testItems.MockQuestionConfigurationSqlQueries.SetupGet(x => x.GetCompulsorySupplierMandatedQuestions)
            .Returns("test sql query");

        var testCompulsorySupplierMandatedQuestionModelDatas = testItems.Fixture.CreateMany<CompulsorySupplierMandatedQuestionModelData>().ToList();

        var testSupplierOrganisationId = testItems.Fixture.Create<int>();

        var supplierOrganisationIsCorrect = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<CompulsorySupplierMandatedQuestionModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                supplierOrganisationIsCorrect = dynamicParameters.SupplierOrganisationId == testSupplierOrganisationId;
            })
            .ReturnsAsync(() => testCompulsorySupplierMandatedQuestionModelDatas);

        var result = (await testItems.QuestionConfigurationRepository.GetCompulsorySupplierMandatedQuestionsAsync(
            testSupplierOrganisationId)).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(testCompulsorySupplierMandatedQuestionModelDatas));

            Assert.That(supplierOrganisationIsCorrect, Is.True);
        });
    }

    [Test]
    public void GivenGettingCompulsorySupplierMandatedQuestionsWillFail_WhenIGetCompulsorySupplierMandatedQuestionsAsync_ThenAnDatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockQuestionConfigurationSqlQueries.SetupGet(x => x.GetCompulsorySupplierMandatedQuestions)
            .Returns("test sql query");

        var testException = new Exception("test error message");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<CompulsorySupplierMandatedQuestionModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<object?>()))
            .Throws(() => testException);

        Assert.That(async () => await testItems.QuestionConfigurationRepository.GetCompulsorySupplierMandatedQuestionsAsync(
                testItems.Fixture.Create<int>()),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.Message.EqualTo("Failed to GetCompulsorySupplierMandatedQuestions from database").And
                .With.InnerException.With.Message.EqualTo("test error message"));
    }
    #endregion

    #region SetCompulsorySupplierMandatedQuestionAsync() Tests
    [Test]
    public async Task GivenAQuestionId_WhenISetCompulsorySupplierMandatedQuestionAsync_ThenTheQuestionIsSetToBeCompulsorySupplierMandatedInTheDatabase()
    {
        var testItems = CreateTestItems();

        testItems.MockQuestionConfigurationSqlQueries.SetupGet(x => x.GetCompulsorySupplierMandatedQuestions)
            .Returns("test sql query");

        testItems.MockQuestionConfigurationSqlQueries.SetupGet(x => x.SetCompulsorySupplierMandatedQuestion)
            .Returns("test sql command");

        var testCompulsorySupplierMandatedQuestionModelDatas = testItems.Fixture.CreateMany<CompulsorySupplierMandatedQuestionModelData>().ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<CompulsorySupplierMandatedQuestionModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testCompulsorySupplierMandatedQuestionModelDatas);

        var testSupplierOrganisationId = testItems.Fixture.Create<int>();
        var testQuestionId = testItems.Fixture.Create<Guid>();

        var questionHasBeenSetCompulsorySupplierMandated = false;
        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteScalarAsync(
            testItems.MockDbConnection.Object,
            testItems.MockDbTransaction.Object,
            "test sql command",
            It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                questionHasBeenSetCompulsorySupplierMandated = 
                    dynamicParameters.SupplierOrganisationId == testSupplierOrganisationId &&
                    dynamicParameters.QuestionId == testQuestionId;
            });

        await testItems.QuestionConfigurationRepository.SetCompulsorySupplierMandatedQuestionAsync(testSupplierOrganisationId, testQuestionId);

        Assert.That(questionHasBeenSetCompulsorySupplierMandated, Is.True);
    }

    [Test]
    public void GivenAQuestionIdThatIsAlreadyCompulsorySupplierMandated_WhenISetCompulsorySupplierMandatedQuestionAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockQuestionConfigurationSqlQueries.SetupGet(x => x.GetCompulsorySupplierMandatedQuestions)
            .Returns("test sql query");

        testItems.MockQuestionConfigurationSqlQueries.SetupGet(x => x.SetCompulsorySupplierMandatedQuestion)
            .Returns("test sql command");

        var testCompulsorySupplierMandatedQuestionModelDatas = testItems.Fixture.CreateMany<CompulsorySupplierMandatedQuestionModelData>().ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<CompulsorySupplierMandatedQuestionModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testCompulsorySupplierMandatedQuestionModelDatas);

        var testSupplierOrganisationId = testItems.Fixture.Create<int>();
        var testQuestionId = testCompulsorySupplierMandatedQuestionModelDatas.First().CompulsorySupplierMandatedQuestion_QuestionId;

        Assert.That(async () => await testItems.QuestionConfigurationRepository.SetCompulsorySupplierMandatedQuestionAsync(
                testSupplierOrganisationId,
                testQuestionId),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.Message.EqualTo("Failed to SetCompulsorySupplierMandatedQuestion in database").And
                .With.InnerException.With.Message.EqualTo("Question is already supplier-mandated compulsory"));
    }
    #endregion

    #region ClearCompulsorySupplierMandatedQuestionAsync() Tests
    [Test]
    public async Task GivenAQuestionId_WhenIClearCompulsorySupplierMandatedQuestionAsync_ThenTheQuestionIsSetToBeNotCompulsorySupplierMandatedInTheDatabase()
    {
        var testItems = CreateTestItems();

        testItems.MockQuestionConfigurationSqlQueries.SetupGet(x => x.GetCompulsorySupplierMandatedQuestions)
            .Returns("test sql query");

        testItems.MockQuestionConfigurationSqlQueries.SetupGet(x => x.ClearCompulsorySupplierMandatedQuestion)
            .Returns("test sql command");

        var testCompulsorySupplierMandatedQuestionModelDatas = testItems.Fixture.CreateMany<CompulsorySupplierMandatedQuestionModelData>().ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<CompulsorySupplierMandatedQuestionModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testCompulsorySupplierMandatedQuestionModelDatas);

        var testSupplierOrganisationId = testItems.Fixture.Create<int>();
        var testQuestionId = testCompulsorySupplierMandatedQuestionModelDatas.First().CompulsorySupplierMandatedQuestion_QuestionId;

        var questionHasBeenSetNotCompulsorySupplierMandated = false;
        testItems.MockDatabaseCommandRunner.Setup(x => x.DbExecuteScalarAsync(
            testItems.MockDbConnection.Object,
            testItems.MockDbTransaction.Object,
            "test sql command",
            It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                questionHasBeenSetNotCompulsorySupplierMandated =
                    dynamicParameters.SupplierOrganisationId == testSupplierOrganisationId && 
                    dynamicParameters.QuestionId == testQuestionId;
            });

        await testItems.QuestionConfigurationRepository.ClearCompulsorySupplierMandatedQuestionAsync(testSupplierOrganisationId, testQuestionId);

        Assert.That(questionHasBeenSetNotCompulsorySupplierMandated, Is.True);
    }

    [Test]
    public void GivenAQuestionIdThatIsNotCompulsorySupplierMandated_WhenIClearCompulsorySupplierMandatedQuestionAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockQuestionConfigurationSqlQueries.SetupGet(x => x.GetCompulsorySupplierMandatedQuestions)
            .Returns("test sql query");

        testItems.MockQuestionConfigurationSqlQueries.SetupGet(x => x.ClearCompulsorySupplierMandatedQuestion)
            .Returns("test sql command");

        var testCompulsorySupplierMandatedQuestionModelDatas = testItems.Fixture.CreateMany<CompulsorySupplierMandatedQuestionModelData>().ToList();

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync<CompulsorySupplierMandatedQuestionModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<object?>()))
            .ReturnsAsync(() => testCompulsorySupplierMandatedQuestionModelDatas);

        var testSupplierOrganisationId = testItems.Fixture.Create<int>();
        var testQuestionId = testItems.Fixture.Create<Guid>();

        Assert.That(async () => await testItems.QuestionConfigurationRepository.ClearCompulsorySupplierMandatedQuestionAsync(testSupplierOrganisationId, testQuestionId),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.Message.EqualTo("Failed to ClearCompulsorySupplierMandatedQuestion in database").And
                .With.InnerException.With.Message.EqualTo("Question is not supplier-mandated compulsory"));
    }
    #endregion
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockLogger = Mock.Get(fixture.Freeze<ILogger<QuestionConfigurationRepository>>());
        var mockDatabaseChannelCreation = Mock.Get(fixture.Freeze<IDatabaseChannelCreation>());

        var mockDatabaseChannelResources = mockDatabaseChannelCreation.CreateTestableDatabaseChannelResources(fixture);
        var mockDatabaseCommandRunner = Mock.Get(fixture.Freeze<IDatabaseCommandRunner>());
        var mockQuestionConfigurationSqlQueries = Mock.Get(fixture.Create<IQuestionConfigurationSqlQueries>());

        var questionConfigurationRepository = new QuestionConfigurationRepository(
            mockLogger.Object,
            mockDatabaseChannelCreation.Object,
            mockDatabaseCommandRunner.Object,
            mockQuestionConfigurationSqlQueries.Object);

        return new TestItems(
            fixture,
            questionConfigurationRepository,
            mockDatabaseChannelResources.MockDbConnection,
            mockDatabaseChannelResources.MockDbTransaction,
            mockDatabaseCommandRunner,
            mockQuestionConfigurationSqlQueries);
    }

    private class TestItems(
        IFixture fixture,
        IQuestionConfigurationRepository questionConfigurationRepository,
        Mock<IDbConnection> mockDbConnection,
        Mock<IDbTransaction> mockDbTransaction,
        Mock<IDatabaseCommandRunner> mockDatabaseCommandRunner,
        Mock<IQuestionConfigurationSqlQueries> mockQuestionConfigurationSqlQueries)
    {
        public IFixture Fixture { get; } = fixture;
        public IQuestionConfigurationRepository QuestionConfigurationRepository { get; } = questionConfigurationRepository;
        public Mock<IDbConnection> MockDbConnection { get; } = mockDbConnection;
        public Mock<IDbTransaction> MockDbTransaction { get; } = mockDbTransaction;
        public Mock<IDatabaseCommandRunner> MockDatabaseCommandRunner { get; } = mockDatabaseCommandRunner;
        public Mock<IQuestionConfigurationSqlQueries> MockQuestionConfigurationSqlQueries { get; } = mockQuestionConfigurationSqlQueries;
    }
    #endregion
}
