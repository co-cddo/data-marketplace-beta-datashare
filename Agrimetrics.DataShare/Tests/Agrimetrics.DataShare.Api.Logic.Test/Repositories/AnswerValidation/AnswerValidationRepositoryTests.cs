using Agrimetrics.DataShare.Api.Db.DbAccess;
using AutoFixture.AutoMoq;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Data;
using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.Repositories.AnswerValidation;
using Agrimetrics.DataShare.Api.Logic.Test.TestHelpers;
using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;

namespace Agrimetrics.DataShare.Api.Logic.Test.Repositories.AnswerValidation;

[TestFixture]
public class AnswerValidationRepositoryTests
{
    #region GetDataShareRequestSelectionOptionsModelDataAsync() Tests
    [Test]
    public async Task GivenAnAnswerValidationRepository_WhenIGetQuestionPartAnswerValidationRulesAsync_ThenQuestionPartAnswerValidationRuleSetModelDataIsObtainedFromTheDatabase()
    {
        var testItems = CreateTestItems();

        testItems.MockAnswerValidationSqlQueries.SetupGet(x => x.GetQuestionPartAnswerValidationRuleSet)
            .Returns(() => "test sql query");

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testQuestionPartId = testItems.Fixture.Create<Guid>();

        var testQuestionPartAnswerValidationRuleSetModelDatas =
            testItems.Fixture.CreateMany<QuestionPartAnswerValidationRuleSetModelData>().ToList();

        bool? parametersAreCorrect = null;
        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<Func<QuestionPartAnswerValidationRuleSetModelData, QuestionPartAnswerValidationRuleModelData?, QuestionPartAnswerValidationRuleSetModelData>>(),
                nameof(QuestionPartAnswerValidationRuleModelData.QuestionPartAnswerValidationRule_RuleId),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<QuestionPartAnswerValidationRuleSetModelData, QuestionPartAnswerValidationRuleModelData?, QuestionPartAnswerValidationRuleSetModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                parametersAreCorrect =
                    dynamicParameters.DataShareRequestId == testDataShareRequestId &&
                    dynamicParameters.QuestionPartId == testQuestionPartId;

            })
            .ReturnsAsync(() => testQuestionPartAnswerValidationRuleSetModelDatas);

        var result = await testItems.AnswerValidationRepository.GetQuestionPartAnswerValidationRulesAsync(
            testDataShareRequestId, testQuestionPartId);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(testQuestionPartAnswerValidationRuleSetModelDatas.First()));

            Assert.That(parametersAreCorrect, Is.True);
        });
    }

    [Test]
    public void GivenGettingQuestionPartAnswerValidationRulesWillFail_WhenIGetQuestionPartAnswerValidationRulesAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testException = new Exception("test error message");

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                It.IsAny<string>(),
                It.IsAny<Func<QuestionPartAnswerValidationRuleSetModelData, QuestionPartAnswerValidationRuleModelData?, QuestionPartAnswerValidationRuleSetModelData>>(),
                It.IsAny<string>(),
                It.IsAny<object?>()))
            .Throws(() => testException);

        Assert.That(async () => await testItems.AnswerValidationRepository.GetQuestionPartAnswerValidationRulesAsync(
                testItems.Fixture.Create<Guid>(),
                testItems.Fixture.Create<Guid>()),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.InnerException.SameAs(testException).And
                .With.Message.EqualTo("Failed to GetQuestionPartAnswerValidationRules from database"));
    }

    [Test]
    public async Task GivenAnAnswerValidationRepository_WhenIGetQuestionPartAnswerValidationRulesAsync_ThenTheMappingFunctionIsRun()
    {
        var testItems = CreateTestItems();

        testItems.MockAnswerValidationSqlQueries.SetupGet(x => x.GetQuestionPartAnswerValidationRuleSet)
            .Returns(() => "test sql query");

        var testDataShareRequestSelectionOptionsModelDatas =
            testItems.Fixture.CreateMany<QuestionPartAnswerValidationRuleSetModelData>().ToList();

        var mappingFunctionHasBeenRun = false;

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test sql query",
                It.IsAny<Func<QuestionPartAnswerValidationRuleSetModelData, QuestionPartAnswerValidationRuleModelData?, QuestionPartAnswerValidationRuleSetModelData>>(),
                nameof(QuestionPartAnswerValidationRuleModelData.QuestionPartAnswerValidationRule_RuleId),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<QuestionPartAnswerValidationRuleSetModelData, QuestionPartAnswerValidationRuleModelData?, QuestionPartAnswerValidationRuleSetModelData> mappingFunc,
                string _,
                object? _) =>
            {
                mappingFunctionHasBeenRun = true;

                mappingFunc(
                    testItems.Fixture.Create<QuestionPartAnswerValidationRuleSetModelData>(),
                    testItems.Fixture.Create<QuestionPartAnswerValidationRuleModelData>());
            })
            .ReturnsAsync(() => testDataShareRequestSelectionOptionsModelDatas);

        await testItems.AnswerValidationRepository.GetQuestionPartAnswerValidationRulesAsync(
            testItems.Fixture.Create<Guid>(),
            testItems.Fixture.Create<Guid>());

        Assert.That(mappingFunctionHasBeenRun, Is.True);
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockLogger = Mock.Get(fixture.Freeze<ILogger<AnswerValidationRepository>>());
        var mockDatabaseChannelCreation = Mock.Get(fixture.Freeze<IDatabaseChannelCreation>());

        var mockDatabaseChannelResources = mockDatabaseChannelCreation.CreateTestableDatabaseChannelResources(fixture);
        var mockDatabaseCommandRunner = Mock.Get(fixture.Freeze<IDatabaseCommandRunner>());
        var mockAnswerValidationSqlQueries = Mock.Get(fixture.Create<IAnswerValidationSqlQueries>());

        var answerValidationRepository = new AnswerValidationRepository(
            mockLogger.Object,
            mockDatabaseChannelCreation.Object,
            mockDatabaseCommandRunner.Object,
            mockAnswerValidationSqlQueries.Object);

        return new TestItems(
            fixture,
            answerValidationRepository,
            mockDatabaseChannelResources.MockDbConnection,
            mockDatabaseChannelResources.MockDbTransaction,
            mockDatabaseCommandRunner,
            mockAnswerValidationSqlQueries);
    }

    private class TestItems(
        IFixture fixture,
        IAnswerValidationRepository answerValidationRepository,
        Mock<IDbConnection> mockDbConnection,
        Mock<IDbTransaction> mockDbTransaction,
        Mock<IDatabaseCommandRunner> mockDatabaseCommandRunner,
        Mock<IAnswerValidationSqlQueries> mockAnswerValidationSqlQueries)
    {
        public IFixture Fixture { get; } = fixture;
        public IAnswerValidationRepository AnswerValidationRepository { get; } = answerValidationRepository;
        public Mock<IDbConnection> MockDbConnection { get; } = mockDbConnection;
        public Mock<IDbTransaction> MockDbTransaction { get; } = mockDbTransaction;
        public Mock<IDatabaseCommandRunner> MockDatabaseCommandRunner { get; } = mockDatabaseCommandRunner;
        public Mock<IAnswerValidationSqlQueries> MockAnswerValidationSqlQueries { get; } = mockAnswerValidationSqlQueries;
    }
    #endregion
}
