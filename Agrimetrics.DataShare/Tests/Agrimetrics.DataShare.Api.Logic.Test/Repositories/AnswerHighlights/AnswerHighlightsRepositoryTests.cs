using System.Data;
using AutoFixture.AutoMoq;
using AutoFixture;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using Moq;
using Agrimetrics.DataShare.Api.Logic.Repositories.AnswerHighlights;
using Agrimetrics.DataShare.Api.Db.DbAccess;
using Agrimetrics.DataShare.Api.Logic.Test.TestHelpers;
using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerHighlights;
using Agrimetrics.DataShare.Api.Logic.Exceptions;

namespace Agrimetrics.DataShare.Api.Logic.Test.Repositories.AnswerHighlights;

[TestFixture]
public class AnswerHighlightsRepositoryTests
{
    #region GetQuestionSetSelectionOptionQuestionHighlightModelDataAsync() Tests
    [Test]
    public async Task GivenAnAnswerHighlightsRepository_WhenIGetQuestionSetSelectionOptionQuestionHighlightModelDataAsync_ThenQuestionSetSelectionOptionQuestionHighlightIsObtainedFromTheDatabase()
    {
        var testItems = CreateTestItems();

        testItems.MockAnswerHighlightsSqlQueries.SetupGet(x => x.GetQuestionSetSelectionOptionQuestionHighlightModelDatas)
            .Returns(() => "test sql query");

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testQuestionSetSelectionOptionQuestionHighlightModelDatas =
            testItems.Fixture.CreateMany<QuestionSetSelectionOptionQuestionHighlightModelData>().ToList();

        bool? parametersAreCorrect = null;
        testItems.MockDatabaseCommandRunner.Setup(x => x
            .DbQueryAsync<QuestionSetSelectionOptionQuestionHighlightModelData>(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<object?>()))
            .Callback((IDbConnection _, IDbTransaction _, string _, object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                parametersAreCorrect = dynamicParameters.DataShareRequestId == testDataShareRequestId;
            })
            .ReturnsAsync(() => testQuestionSetSelectionOptionQuestionHighlightModelDatas);

        var result = (await testItems.AnswerHighlightsRepository.GetQuestionSetSelectionOptionQuestionHighlightModelDataAsync(
            testDataShareRequestId)).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Exactly(testQuestionSetSelectionOptionQuestionHighlightModelDatas.Count).Items);

            foreach (var testQuestionSetSelectionOptionQuestionHighlightModelData in testQuestionSetSelectionOptionQuestionHighlightModelDatas)
            {
                var resultQuestionSetSelectionOptionQuestionHighlightModelData = result.FirstOrDefault(x =>
                    x.QuestionSetSelectionOptionQuestionHighlight_Id == testQuestionSetSelectionOptionQuestionHighlightModelData.QuestionSetSelectionOptionQuestionHighlight_Id);

                Assert.That(resultQuestionSetSelectionOptionQuestionHighlightModelData, Is.Not.Null);
            }

            Assert.That(parametersAreCorrect, Is.True);
        });
    }

    [Test]
    public void GivenGettingQuestionSetSelectionOptionQuestionHighlightModelDataWillFail_WhenIGetQuestionSetSelectionOptionQuestionHighlightModelDataAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testException = new Exception("test error message");

        testItems.MockDatabaseCommandRunner.Setup(x => x
                .DbQueryAsync<QuestionSetSelectionOptionQuestionHighlightModelData>(
                    testItems.MockDbConnection.Object,
                    testItems.MockDbTransaction.Object,
                    It.IsAny<string>(),
                    It.IsAny<object?>()))
            .Throws(() => testException);

        Assert.That(async () => await testItems.AnswerHighlightsRepository.GetQuestionSetSelectionOptionQuestionHighlightModelDataAsync(testItems.Fixture.Create<Guid>()),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.InnerException.SameAs(testException).And
                .With.Message.EqualTo("Failed to GetQuestionSetSelectionOptionQuestionHighlightModelData from database"));
    }
    #endregion

    #region GetDataShareRequestSelectionOptionsModelDataAsync() Tests
    [Test]
    public async Task GivenAnAnswerHighlightsRepository_WhenIGetDataShareRequestSelectionOptionsModelDataAsync_ThenDataShareRequestSelectionOptionsDataIsObtainedFromTheDatabase()
    {
        var testItems = CreateTestItems();

        testItems.MockAnswerHighlightsSqlQueries.SetupGet(x => x.GetDataShareRequestSelectedOptionsModelDatas)
            .Returns(() => "test sql query");

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testDataShareRequestSelectionOptionsModelDatas =
            testItems.Fixture.CreateMany<DataShareRequestSelectionOptionsModelData>().ToList();

        bool? parametersAreCorrect = null;
        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<Func<DataShareRequestSelectionOptionsModelData, DataShareRequestSelectedOptionModelData, DataShareRequestSelectionOptionsModelData>>(),
                nameof(DataShareRequestSelectedOptionModelData.DataShareRequestSelectedOption_OptionSelectionId),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<DataShareRequestSelectionOptionsModelData, DataShareRequestSelectedOptionModelData, DataShareRequestSelectionOptionsModelData> _,
                string _,
                object? parameters) => 
            {
                dynamic dynamicParameters = parameters!;

                parametersAreCorrect = dynamicParameters.DataShareRequestId == testDataShareRequestId;

            })
            .ReturnsAsync(() => testDataShareRequestSelectionOptionsModelDatas);

        var result = await testItems.AnswerHighlightsRepository.GetDataShareRequestSelectionOptionsModelDataAsync(testDataShareRequestId);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(testDataShareRequestSelectionOptionsModelDatas.First()));

            Assert.That(parametersAreCorrect, Is.True);
        });
    }

    [Test]
    public void GivenGettingDataShareRequestSelectionOptionsModelDataWillFail_WhenIGetDataShareRequestSelectionOptionsModelDataAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testException = new Exception("test error message");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                It.IsAny<string>(),
                It.IsAny<Func<DataShareRequestSelectionOptionsModelData, DataShareRequestSelectedOptionModelData, DataShareRequestSelectionOptionsModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testException);

        Assert.That(async () => await testItems.AnswerHighlightsRepository.GetDataShareRequestSelectionOptionsModelDataAsync(testItems.Fixture.Create<Guid>()),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.InnerException.SameAs(testException).And
                .With.Message.EqualTo("Failed to GetDataShareRequestSelectionOptionsModelData from database"));
    }

    [Test]
    public async Task GivenAnAnswerHighlightsRepository_WhenIGetDataShareRequestSelectionOptionsModelDataAsync_ThenTheMappingFunctionIsRun()
    {
        var testItems = CreateTestItems();

        testItems.MockAnswerHighlightsSqlQueries.SetupGet(x => x.GetDataShareRequestSelectedOptionsModelDatas)
            .Returns(() => "test sql query");

        var testDataShareRequestSelectionOptionsModelDatas =
            testItems.Fixture.CreateMany<DataShareRequestSelectionOptionsModelData>().ToList();

        var mappingFunctionHasBeenRun = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<Func<DataShareRequestSelectionOptionsModelData, DataShareRequestSelectedOptionModelData, DataShareRequestSelectionOptionsModelData>>(),
                nameof(DataShareRequestSelectedOptionModelData.DataShareRequestSelectedOption_OptionSelectionId),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<DataShareRequestSelectionOptionsModelData, DataShareRequestSelectedOptionModelData, DataShareRequestSelectionOptionsModelData> mappingFunc,
                string _,
                object? _) =>
            {
                mappingFunctionHasBeenRun = true;

                mappingFunc(
                    testItems.Fixture.Create<DataShareRequestSelectionOptionsModelData>(),
                    testItems.Fixture.Create<DataShareRequestSelectedOptionModelData>());
            })
            .ReturnsAsync(() => testDataShareRequestSelectionOptionsModelDatas);

        await testItems.AnswerHighlightsRepository.GetDataShareRequestSelectionOptionsModelDataAsync(testItems.Fixture.Create<Guid>());

        Assert.That(mappingFunctionHasBeenRun, Is.True);
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockLogger = Mock.Get(fixture.Freeze<ILogger<AnswerHighlightsRepository>>());
        var mockDatabaseChannelCreation = Mock.Get(fixture.Freeze<IDatabaseChannelCreation>());

        var mockDatabaseChannelResources = mockDatabaseChannelCreation.CreateTestableDatabaseChannelResources(fixture);
        var mockDatabaseCommandRunner = Mock.Get(fixture.Freeze<IDatabaseCommandRunner>());
        var mockAnswerHighlightsSqlQueries = Mock.Get(fixture.Create<IAnswerHighlightsSqlQueries>());

        var answerHighlightsRepository = new AnswerHighlightsRepository(
            mockLogger.Object,
            mockDatabaseChannelCreation.Object,
            mockDatabaseCommandRunner.Object,
            mockAnswerHighlightsSqlQueries.Object);

        return new TestItems(
            fixture,
            answerHighlightsRepository,
            mockDatabaseChannelResources.MockDbConnection,
            mockDatabaseChannelResources.MockDbTransaction,
            mockDatabaseCommandRunner,
            mockAnswerHighlightsSqlQueries);
    }

    private class TestItems(
        IFixture fixture,
        IAnswerHighlightsRepository answerHighlightsRepository,
        Mock<IDbConnection> mockDbConnection,
        Mock<IDbTransaction> mockDbTransaction,
        Mock<IDatabaseCommandRunner> mockDatabaseCommandRunner,
        Mock<IAnswerHighlightsSqlQueries> mockAnswerHighlightsSqlQueries)
    {
        public IFixture Fixture { get; } = fixture;
        public IAnswerHighlightsRepository AnswerHighlightsRepository { get; } = answerHighlightsRepository;
        public Mock<IDbConnection> MockDbConnection { get; } = mockDbConnection;
        public Mock<IDbTransaction> MockDbTransaction { get; } = mockDbTransaction;
        public Mock<IDatabaseCommandRunner> MockDatabaseCommandRunner { get; } = mockDatabaseCommandRunner;
        public Mock<IAnswerHighlightsSqlQueries> MockAnswerHighlightsSqlQueries { get; } = mockAnswerHighlightsSqlQueries;
    }
    #endregion
}
