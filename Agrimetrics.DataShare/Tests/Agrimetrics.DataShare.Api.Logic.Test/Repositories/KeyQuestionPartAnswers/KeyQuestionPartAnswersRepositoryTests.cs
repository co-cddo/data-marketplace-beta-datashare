using System.ComponentModel;
using Agrimetrics.DataShare.Api.Db.DbAccess;
using AutoFixture.AutoMoq;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Data;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.Repositories.KeyQuestionPartAnswers;
using Agrimetrics.DataShare.Api.Logic.Test.TestHelpers;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.KeyQuestionParts;

namespace Agrimetrics.DataShare.Api.Logic.Test.Repositories.KeyQuestionPartAnswers;

[TestFixture]
public class KeyQuestionPartAnswersRepositoryTests
{
    #region GetKeyQuestionPartResponsesAsync() Tests
    [Test]
    public void GivenAnInvalidQuestionPartKeyType_WhenIGetKeyQuestionPartResponsesAsync_ThenAnInvalidEnumArgumentIsThrown()
    {
        var testItems = CreateTestItems();

        var testQuestionPartKey = (QuestionPartKeyType) Enum.GetValues<QuestionPartKeyType>().Cast<int>().Max() + 1;

        Assert.That(() => testItems.KeyQuestionPartAnswersRepository.GetKeyQuestionPartResponsesAsync(
                testItems.Fixture.Create<Guid>(),
                testQuestionPartKey),
            Throws.TypeOf<InvalidEnumArgumentException>().With.Property("ParamName").EqualTo("questionPartKey"));
    }

    [Test]
    public async Task GivenAFreeFormQuestionPart_WhenIGetKeyQuestionPartResponsesAsync_ThenQuestionPartAnswersAreObtainedFromTheDatabase()
    {
        var testItems = CreateTestItems();

        testItems.MockKeyQuestionPartAnswersSqlQueries.SetupGet(x => x.GetKeyQuestionPartModelData)
            .Returns(() => "test question part sql query");

        testItems.MockKeyQuestionPartAnswersSqlQueries.SetupGet(x => x.GetKeyQuestionPartFreeFormResponseItemModelDatas)
            .Returns("test response item sql query");

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testQuestionPartKeyType = testItems.Fixture.Create<QuestionPartKeyType>();

        #region Set Up Key Question Answer Part Data
        var testKeyQuestionPartAnswerResponseItemFreeFormModelDatas = testItems.Fixture
            .Build<KeyQuestionPartAnswerResponseItemFreeFormModelData>()
            .CreateMany()
            .ToList();

        var testKeyQuestionPartAnswerResponseModelDatas = testItems.Fixture
            .Build<KeyQuestionPartAnswerResponseModelData>()
            .Without(x => x.KeyQuestionPartAnswerResponse_ResponseItem)
            .CreateMany()
            .Select((item, index) =>
            {
                item.KeyQuestionPartAnswerResponse_ResponseItem = testKeyQuestionPartAnswerResponseItemFreeFormModelDatas[index];
                return item;
            })
            .ToList();

        var testKeyQuestionPartAnswerModelDatas = testItems.Fixture
            .Build<KeyQuestionPartAnswerModelData>()
            .With(x => x.KeyQuestionPartAnswer_ResponseFormatType, QuestionPartResponseInputType.FreeForm)
            .With(x => x.KeyQuestionPartAnswer_AnswerPartResponses, testKeyQuestionPartAnswerResponseModelDatas)
            .CreateMany(1)
            .ToList();
        #endregion

        testItems.MockDatabaseCommandRunner.Setup(x =>
                x.DbQuerySingleAsync<KeyQuestionPartAnswerResponseItemFreeFormModelData>(
                    testItems.MockDbConnection.Object,
                    testItems.MockDbTransaction.Object,
                    "test response item sql query",
                    It.IsAny<object?>()))
            .ReturnsAsync((IDbConnection _, IDbTransaction _, string _, object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                var answerPartResponseId = dynamicParameters.AnswerPartResponseId;

                var testKeyQuestionPartAnswerResponseModelData = testKeyQuestionPartAnswerResponseModelDatas.Single(x =>
                    x.KeyQuestionPart_AnswerPartResponseId == answerPartResponseId);

                var freeFormResponseItem = (KeyQuestionPartAnswerResponseItemFreeFormModelData) testKeyQuestionPartAnswerResponseModelData.KeyQuestionPartAnswerResponse_ResponseItem!;

                return freeFormResponseItem;
            });

        bool? parametersAreCorrect = null;
        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test question part sql query",
                It.IsAny<Func<KeyQuestionPartAnswerModelData, KeyQuestionPartAnswerResponseModelData?, KeyQuestionPartAnswerModelData>>(),
                nameof(KeyQuestionPartAnswerResponseModelData.KeyQuestionPart_AnswerPartResponseId),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<KeyQuestionPartAnswerModelData, KeyQuestionPartAnswerResponseModelData?, KeyQuestionPartAnswerModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                parametersAreCorrect =
                    dynamicParameters.DataShareRequestId == testDataShareRequestId &&
                    dynamicParameters.QuestionPartKey == testQuestionPartKeyType.ToString();

            })
            .ReturnsAsync(() => testKeyQuestionPartAnswerModelDatas);

        var result = (await testItems.KeyQuestionPartAnswersRepository.GetKeyQuestionPartResponsesAsync(
            testDataShareRequestId, testQuestionPartKeyType)).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Exactly(testKeyQuestionPartAnswerResponseItemFreeFormModelDatas.Count).Items);
            
            foreach (var testKeyQuestionPartAnswerResponseItemFreeFormModelData in testKeyQuestionPartAnswerResponseItemFreeFormModelDatas)
            {
                var resultResponse = result.FirstOrDefault(x =>
                    x == testKeyQuestionPartAnswerResponseItemFreeFormModelData.KeyQuestionPartAnswerResponseItemFreeForm_EnteredValue);

                Assert.That(resultResponse, Is.Not.Null);
            }

            Assert.That(parametersAreCorrect, Is.True);
        });
    }

    [Test]
    public async Task GivenAnOptionSelectionQuestionPart_WhenIGetKeyQuestionPartResponsesAsync_ThenQuestionPartAnswersAreObtainedFromTheDatabase()
    {
        var testItems = CreateTestItems();

        testItems.MockKeyQuestionPartAnswersSqlQueries.SetupGet(x => x.GetKeyQuestionPartModelData)
            .Returns(() => "test question part sql query");

        testItems.MockKeyQuestionPartAnswersSqlQueries.SetupGet(x => x.GetKeyQuestionPartOptionSelectionResponseItemModelDatas)
            .Returns("test response item sql query");

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testQuestionPartKeyType = testItems.Fixture.Create<QuestionPartKeyType>();

        #region Set Up Key Question Answer Part Data
        var testKeyQuestionPartAnswerResponseItemOptionSelectionModelDatas = testItems.Fixture
            .Build<KeyQuestionPartAnswerResponseItemOptionSelectionModelData>()
            .With(x => x.KeyQuestionPartAnswerResponseItemOptionSelection_SelectedOptions,
                testItems.Fixture
                    .Build<KeyQuestionPartAnswerResponseItemSelectedOptionModelData>()
                    .CreateMany().ToList())
            .CreateMany()
            .ToList();

        var testKeyQuestionPartAnswerResponseModelDatas = testItems.Fixture
            .Build<KeyQuestionPartAnswerResponseModelData>()
            .Without(x => x.KeyQuestionPartAnswerResponse_ResponseItem)
            .CreateMany()
            .Select((item, index) =>
            {
                item.KeyQuestionPartAnswerResponse_ResponseItem = testKeyQuestionPartAnswerResponseItemOptionSelectionModelDatas[index];
                return item;
            })
            .ToList();

        var testKeyQuestionPartAnswerModelDatas = testItems.Fixture
            .Build<KeyQuestionPartAnswerModelData>()
            .With(x => x.KeyQuestionPartAnswer_ResponseFormatType, QuestionPartResponseInputType.OptionSelection)
            .With(x => x.KeyQuestionPartAnswer_AnswerPartResponses, testKeyQuestionPartAnswerResponseModelDatas)
            .CreateMany(1)
            .ToList();
        #endregion

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                    testItems.MockDbConnection.Object,
                    testItems.MockDbTransaction.Object,
                    "test response item sql query",
                    It.IsAny<Func<KeyQuestionPartAnswerResponseItemOptionSelectionModelData, KeyQuestionPartAnswerResponseItemSelectedOptionModelData, KeyQuestionPartAnswerResponseItemOptionSelectionModelData>>(),
                    nameof(KeyQuestionPartAnswerResponseItemSelectedOptionModelData.KeyQuestionPartAnswerResponseItemSelectedOption_SelectedOptionValue),
                    It.IsAny<object?>()))
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<KeyQuestionPartAnswerResponseItemOptionSelectionModelData, KeyQuestionPartAnswerResponseItemSelectedOptionModelData, KeyQuestionPartAnswerResponseItemOptionSelectionModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                var answerPartResponseId = dynamicParameters.AnswerPartResponseId;

                var testKeyQuestionPartAnswerResponseModelData = testKeyQuestionPartAnswerResponseModelDatas.Single(x =>
                    x.KeyQuestionPart_AnswerPartResponseId == answerPartResponseId);

                var optionSelectionResponseItem = (KeyQuestionPartAnswerResponseItemOptionSelectionModelData)testKeyQuestionPartAnswerResponseModelData.KeyQuestionPartAnswerResponse_ResponseItem!;

                return [optionSelectionResponseItem];
            });

        bool? parametersAreCorrect = null;
        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test question part sql query",
                It.IsAny<Func<KeyQuestionPartAnswerModelData, KeyQuestionPartAnswerResponseModelData?, KeyQuestionPartAnswerModelData>>(),
                nameof(KeyQuestionPartAnswerResponseModelData.KeyQuestionPart_AnswerPartResponseId),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<KeyQuestionPartAnswerModelData, KeyQuestionPartAnswerResponseModelData?, KeyQuestionPartAnswerModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                parametersAreCorrect =
                    dynamicParameters.DataShareRequestId == testDataShareRequestId &&
                    dynamicParameters.QuestionPartKey == testQuestionPartKeyType.ToString();

            })
            .ReturnsAsync(() => testKeyQuestionPartAnswerModelDatas);

        var result = (await testItems.KeyQuestionPartAnswersRepository.GetKeyQuestionPartResponsesAsync(
            testDataShareRequestId, testQuestionPartKeyType)).ToList();

        Assert.Multiple(() =>
        {
            var testSelectedOptionItems =
                testKeyQuestionPartAnswerResponseItemOptionSelectionModelDatas.First().KeyQuestionPartAnswerResponseItemOptionSelection_SelectedOptions.ToList();

            Assert.That(result, Has.Exactly(testSelectedOptionItems.Count).Items);

            foreach (var testSelectedOptionItem in testSelectedOptionItems)
            {
                var resultResponse = result.FirstOrDefault(x =>
                    x == testSelectedOptionItem.KeyQuestionPartAnswerResponseItemSelectedOption_SelectedOptionValue);

                Assert.That(resultResponse, Is.Not.Null);
            }

            Assert.That(parametersAreCorrect, Is.True);
        });
    }

    [Test]
    public async Task GivenAnOptionSelectionQuestionPart_WhenIGetKeyQuestionPartResponsesAsync_ThenTheMappingFunctionIsRun()
    {
        var testItems = CreateTestItems();

        testItems.MockKeyQuestionPartAnswersSqlQueries.SetupGet(x => x.GetKeyQuestionPartModelData)
            .Returns(() => "test question part sql query");

        testItems.MockKeyQuestionPartAnswersSqlQueries.SetupGet(x => x.GetKeyQuestionPartOptionSelectionResponseItemModelDatas)
            .Returns("test response item sql query");

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testQuestionPartKeyType = testItems.Fixture.Create<QuestionPartKeyType>();

        #region Set Up Key Question Answer Part Data
        var testKeyQuestionPartAnswerResponseItemOptionSelectionModelDatas = testItems.Fixture
            .Build<KeyQuestionPartAnswerResponseItemOptionSelectionModelData>()
            .With(x => x.KeyQuestionPartAnswerResponseItemOptionSelection_SelectedOptions,
                testItems.Fixture
                    .Build<KeyQuestionPartAnswerResponseItemSelectedOptionModelData>()
                    .CreateMany().ToList())
            .CreateMany()
            .ToList();

        var testKeyQuestionPartAnswerResponseModelDatas = testItems.Fixture
            .Build<KeyQuestionPartAnswerResponseModelData>()
            .Without(x => x.KeyQuestionPartAnswerResponse_ResponseItem)
            .CreateMany()
            .Select((item, index) =>
            {
                item.KeyQuestionPartAnswerResponse_ResponseItem = testKeyQuestionPartAnswerResponseItemOptionSelectionModelDatas[index];
                return item;
            })
            .ToList();

        var testKeyQuestionPartAnswerModelDatas = testItems.Fixture
            .Build<KeyQuestionPartAnswerModelData>()
            .With(x => x.KeyQuestionPartAnswer_ResponseFormatType, QuestionPartResponseInputType.OptionSelection)
            .With(x => x.KeyQuestionPartAnswer_AnswerPartResponses, testKeyQuestionPartAnswerResponseModelDatas)
            .CreateMany(1)
            .ToList();
        #endregion

        var optionSelectionResponseMappingFunctionHasBeenRun = false;
        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                    testItems.MockDbConnection.Object,
                    testItems.MockDbTransaction.Object,
                    "test response item sql query",
                    It.IsAny<Func<KeyQuestionPartAnswerResponseItemOptionSelectionModelData, KeyQuestionPartAnswerResponseItemSelectedOptionModelData, KeyQuestionPartAnswerResponseItemOptionSelectionModelData>>(),
                    nameof(KeyQuestionPartAnswerResponseItemSelectedOptionModelData.KeyQuestionPartAnswerResponseItemSelectedOption_SelectedOptionValue),
                    It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<KeyQuestionPartAnswerResponseItemOptionSelectionModelData, KeyQuestionPartAnswerResponseItemSelectedOptionModelData?, KeyQuestionPartAnswerResponseItemOptionSelectionModelData> mappingFunc,
                string _,
                object? _) =>
            {
                optionSelectionResponseMappingFunctionHasBeenRun = true;

                mappingFunc(
                    testItems.Fixture.Create<KeyQuestionPartAnswerResponseItemOptionSelectionModelData>(),
                    testItems.Fixture.Create<KeyQuestionPartAnswerResponseItemSelectedOptionModelData>());
            })
            .ReturnsAsync((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<KeyQuestionPartAnswerResponseItemOptionSelectionModelData, KeyQuestionPartAnswerResponseItemSelectedOptionModelData, KeyQuestionPartAnswerResponseItemOptionSelectionModelData> _,
                string _,
                object? parameters) =>
            {
                dynamic dynamicParameters = parameters!;

                var answerPartResponseId = dynamicParameters.AnswerPartResponseId;

                var testKeyQuestionPartAnswerResponseModelData = testKeyQuestionPartAnswerResponseModelDatas.Single(x =>
                    x.KeyQuestionPart_AnswerPartResponseId == answerPartResponseId);

                var optionSelectionResponseItem = (KeyQuestionPartAnswerResponseItemOptionSelectionModelData)testKeyQuestionPartAnswerResponseModelData.KeyQuestionPartAnswerResponse_ResponseItem!;

                return [optionSelectionResponseItem];
            });

        var answerPartMappingFunctionHasBeenRun = false;
        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test question part sql query",
                It.IsAny<Func<KeyQuestionPartAnswerModelData, KeyQuestionPartAnswerResponseModelData?, KeyQuestionPartAnswerModelData>>(),
                nameof(KeyQuestionPartAnswerResponseModelData.KeyQuestionPart_AnswerPartResponseId),
                It.IsAny<object?>()))
            .Callback((
                IDbConnection _,
                IDbTransaction _,
                string _,
                Func<KeyQuestionPartAnswerModelData, KeyQuestionPartAnswerResponseModelData?, KeyQuestionPartAnswerModelData> mappingFunc,
                string _,
                object? _) =>
            {
                answerPartMappingFunctionHasBeenRun = true;

                mappingFunc(
                    testItems.Fixture.Create<KeyQuestionPartAnswerModelData>(),
                    testItems.Fixture.Create<KeyQuestionPartAnswerResponseModelData>());
            })
            .ReturnsAsync(() => testKeyQuestionPartAnswerModelDatas);

        await testItems.KeyQuestionPartAnswersRepository.GetKeyQuestionPartResponsesAsync(testDataShareRequestId, testQuestionPartKeyType);

        Assert.Multiple(() =>
        {
            Assert.That(answerPartMappingFunctionHasBeenRun, Is.True);
            Assert.That(optionSelectionResponseMappingFunctionHasBeenRun, Is.True);
        });
    }

    [Test]
    public async Task GivenAQuestionPartWithNoResponses_WhenIGetKeyQuestionPartResponsesAsync_ThenNoResponsesAreReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockKeyQuestionPartAnswersSqlQueries.SetupGet(x => x.GetKeyQuestionPartModelData)
            .Returns(() => "test question part sql query");

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testQuestionPartKeyType = testItems.Fixture.Create<QuestionPartKeyType>();

        #region Set Up Key Question Answer Part Data
        var testKeyQuestionPartAnswerModelDatas = testItems.Fixture
            .Build<KeyQuestionPartAnswerModelData>()
            .With(x => x.KeyQuestionPartAnswer_AnswerPartResponses, [])
            .CreateMany(1)
            .ToList();
        #endregion

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test question part sql query",
                It.IsAny<Func<KeyQuestionPartAnswerModelData, KeyQuestionPartAnswerResponseModelData?, KeyQuestionPartAnswerModelData>>(),
                nameof(KeyQuestionPartAnswerResponseModelData.KeyQuestionPart_AnswerPartResponseId),
                It.IsAny<object?>()))
            .ReturnsAsync(() => testKeyQuestionPartAnswerModelDatas);

        var result = (await testItems.KeyQuestionPartAnswersRepository.GetKeyQuestionPartResponsesAsync(
            testDataShareRequestId, testQuestionPartKeyType)).ToList();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GivenAnInvalidResponseFormatType_WhenIGetKeyQuestionPartResponsesAsync_ThenADatabaseAccessGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockKeyQuestionPartAnswersSqlQueries.SetupGet(x => x.GetKeyQuestionPartModelData)
            .Returns(() => "test question part sql query");

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testQuestionPartKeyType = testItems.Fixture.Create<QuestionPartKeyType>();

        #region Set Up Key Question Answer Part Data
        var testResponseFormatType = (QuestionPartResponseInputType) Enum.GetValues<QuestionPartResponseInputType>().Cast<int>().Max() + 1;

        var testKeyQuestionPartAnswerModelDatas = testItems.Fixture
            .Build<KeyQuestionPartAnswerModelData>()
            .With(x => x.KeyQuestionPartAnswer_ResponseFormatType, testResponseFormatType)
            .CreateMany(1)
            .ToList();
        #endregion

        testItems.MockDatabaseCommandRunner.Setup(x => x.DbQueryAsync(
                testItems.MockDbConnection.Object,
                testItems.MockDbTransaction.Object,
                "test question part sql query",
                It.IsAny<Func<KeyQuestionPartAnswerModelData, KeyQuestionPartAnswerResponseModelData?, KeyQuestionPartAnswerModelData>>(),
                nameof(KeyQuestionPartAnswerResponseModelData.KeyQuestionPart_AnswerPartResponseId),
                It.IsAny<object?>()))
            .ReturnsAsync(() => testKeyQuestionPartAnswerModelDatas);

        Assert.That(async () => await testItems.KeyQuestionPartAnswersRepository.GetKeyQuestionPartResponsesAsync(
            testDataShareRequestId, testQuestionPartKeyType),
            Throws.TypeOf<DatabaseAccessGeneralException>()
                .With.Message.EqualTo("Failed to GetKeyQuestionPartAnswer from database").And
                .With.InnerException.With.Message.EqualTo("Cannot report an answer response for a no input question"));
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockLogger = Mock.Get(fixture.Freeze<ILogger<KeyQuestionPartAnswersRepository>>());
        var mockDatabaseChannelCreation = Mock.Get(fixture.Freeze<IDatabaseChannelCreation>());

        var mockDatabaseChannelResources = mockDatabaseChannelCreation.CreateTestableDatabaseChannelResources(fixture);
        var mockDatabaseCommandRunner = Mock.Get(fixture.Freeze<IDatabaseCommandRunner>());
        var mockKeyQuestionPartAnswersSqlQueries = Mock.Get(fixture.Create<IKeyQuestionPartAnswersSqlQueries>());

        var keyQuestionPartAnswersRepository = new KeyQuestionPartAnswersRepository(
            mockLogger.Object,
            mockDatabaseChannelCreation.Object,
            mockDatabaseCommandRunner.Object,
            mockKeyQuestionPartAnswersSqlQueries.Object);

        return new TestItems(
            fixture,
            keyQuestionPartAnswersRepository,
            mockDatabaseChannelResources.MockDbConnection,
            mockDatabaseChannelResources.MockDbTransaction,
            mockDatabaseCommandRunner,
            mockKeyQuestionPartAnswersSqlQueries);
    }

    private class TestItems(
        IFixture fixture,
        IKeyQuestionPartAnswersRepository keyQuestionPartAnswersRepository,
        Mock<IDbConnection> mockDbConnection,
        Mock<IDbTransaction> mockDbTransaction,
        Mock<IDatabaseCommandRunner> mockDatabaseCommandRunner,
        Mock<IKeyQuestionPartAnswersSqlQueries> mockKeyQuestionPartAnswersSqlQueries)
    {
        public IFixture Fixture { get; } = fixture;
        public IKeyQuestionPartAnswersRepository KeyQuestionPartAnswersRepository { get; } = keyQuestionPartAnswersRepository;
        public Mock<IDbConnection> MockDbConnection { get; } = mockDbConnection;
        public Mock<IDbTransaction> MockDbTransaction { get; } = mockDbTransaction;
        public Mock<IDatabaseCommandRunner> MockDatabaseCommandRunner { get; } = mockDatabaseCommandRunner;
        public Mock<IKeyQuestionPartAnswersSqlQueries> MockKeyQuestionPartAnswersSqlQueries { get; } = mockKeyQuestionPartAnswersSqlQueries;
    }
    #endregion
}
