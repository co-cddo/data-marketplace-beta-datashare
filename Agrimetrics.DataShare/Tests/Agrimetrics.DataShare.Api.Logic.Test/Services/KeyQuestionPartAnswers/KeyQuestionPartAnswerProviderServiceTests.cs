using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.KeyQuestionParts;
using AutoFixture.AutoMoq;
using AutoFixture;
using NUnit.Framework;
using Agrimetrics.DataShare.Api.Logic.Services.KeyQuestionPartAnswers;
using Agrimetrics.DataShare.Api.Logic.Repositories.KeyQuestionPartAnswers;
using Agrimetrics.DataShare.Api.Logic.Test.TestHelpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.KeyQuestionPartAnswers;

[TestFixture]
public class KeyQuestionPartAnswerProviderServiceTests
{
    #region GetDateRequiredQuestionPartAnswerAsync() Tests
    [Test]
    public async Task GivenMultipleAnswersStoredForTheQuestionPart_WhenIGetDateRequiredQuestionPartAnswerAsync_ThenNullIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockKeyQuestionPartAnswersRepository.Setup(x => x.GetKeyQuestionPartResponsesAsync(
                testDataShareRequestId, QuestionPartKeyType.DateRequired))
            .ReturnsAsync(() => new List<string> { "thing 1", "thing 2" });

        var result = await testItems.KeyQuestionPartAnswerProviderService.GetDateRequiredQuestionPartAnswerAsync(
            testDataShareRequestId);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GivenMultipleAnswersStoredForTheQuestionPart_WhenIGetDateRequiredQuestionPartAnswerAsync_ThenAnErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockKeyQuestionPartAnswersRepository.Setup(x => x.GetKeyQuestionPartResponsesAsync(
                testDataShareRequestId, QuestionPartKeyType.DateRequired))
            .ReturnsAsync(() => new List<string> {"thing 1", "thing 2"});

        await testItems.KeyQuestionPartAnswerProviderService.GetDateRequiredQuestionPartAnswerAsync(
            testDataShareRequestId);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "KeyQuestionPart 'Date Required' has multiple answers");
    }

    [Test]
    public async Task GivenNoAnswersStoredForTheQuestionPart_WhenIGetDateRequiredQuestionPartAnswerAsync_ThenNullIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockKeyQuestionPartAnswersRepository.Setup(x => x.GetKeyQuestionPartResponsesAsync(
                testDataShareRequestId, QuestionPartKeyType.DateRequired))
            .ReturnsAsync(() => new List<string>());

        var result = await testItems.KeyQuestionPartAnswerProviderService.GetDateRequiredQuestionPartAnswerAsync(
            testDataShareRequestId);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GivenTheAnswerStoredForTheQuestionPartIsNotEightCharactersLong_WhenIGetDateRequiredQuestionPartAnswerAsync_ThenNullIsReturned(
        [Values(1, 2, 3, 4, 5, 6, 7, 9, 10)] int stringLength)
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testAnswer = new string('x', stringLength);

        testItems.MockKeyQuestionPartAnswersRepository.Setup(x => x.GetKeyQuestionPartResponsesAsync(
                testDataShareRequestId, QuestionPartKeyType.DateRequired))
            .ReturnsAsync(() => new List<string>{ testAnswer });

        var result = await testItems.KeyQuestionPartAnswerProviderService.GetDateRequiredQuestionPartAnswerAsync(
            testDataShareRequestId);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GivenTheAnswerStoredForTheQuestionPartIsNotEightCharactersLong_WhenIGetDateRequiredQuestionPartAnswerAsync_ThenAnErrorIsLogged(
        [Values(1, 2, 3, 4, 5, 6, 7, 9, 10)] int stringLength)
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testAnswer = new string('x', stringLength);

        testItems.MockKeyQuestionPartAnswersRepository.Setup(x => x.GetKeyQuestionPartResponsesAsync(
                testDataShareRequestId, QuestionPartKeyType.DateRequired))
            .ReturnsAsync(() => new List<string> { testAnswer });

        await testItems.KeyQuestionPartAnswerProviderService.GetDateRequiredQuestionPartAnswerAsync(
            testDataShareRequestId);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "KeyQuestionPart 'Date Required' has unexpected value");
    }

    [Test]
    [TestCase("202512ab")] // Day non-numeric
    [TestCase("2025ab11")] // Month non-numeric
    [TestCase("abcd1211")] // Year non-numeric
    public async Task GivenTheAnswerStoredForTheQuestionPartHasNonNumericPart_WhenIGetDateRequiredQuestionPartAnswerAsync_ThenNullIsReturned(
        string testAnswer)
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        
        testItems.MockKeyQuestionPartAnswersRepository.Setup(x => x.GetKeyQuestionPartResponsesAsync(
                testDataShareRequestId, QuestionPartKeyType.DateRequired))
            .ReturnsAsync(() => new List<string> { testAnswer });

        var result = await testItems.KeyQuestionPartAnswerProviderService.GetDateRequiredQuestionPartAnswerAsync(
            testDataShareRequestId);

        Assert.That(result, Is.Null);
    }

    [Test]
    [TestCase("202512ab")] // Day non-numeric
    [TestCase("2025ab11")] // Month non-numeric
    [TestCase("abcd1211")] // Year non-numeric
    public async Task GivenTheAnswerStoredForTheQuestionPartHasNonNumericPart_WhenIGetDateRequiredQuestionPartAnswerAsync_ThenAnErrorIsLogged(
        string testAnswer)
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockKeyQuestionPartAnswersRepository.Setup(x => x.GetKeyQuestionPartResponsesAsync(
                testDataShareRequestId, QuestionPartKeyType.DateRequired))
            .ReturnsAsync(() => new List<string> { testAnswer });

        await testItems.KeyQuestionPartAnswerProviderService.GetDateRequiredQuestionPartAnswerAsync(
            testDataShareRequestId);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "KeyQuestionPart 'Date Required' has badly formed value");
    }

    [Test]
    [TestCase("20251299")] // Day too big
    [TestCase("20251200")] // Day too small
    [TestCase("20259911")] // Month too big
    [TestCase("20250011")] // Month too small
    public async Task GivenTheAnswerStoredForTheQuestionPartHasInvalidDate_WhenIGetDateRequiredQuestionPartAnswerAsync_ThenNullIsReturned(
        string testAnswer)
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockKeyQuestionPartAnswersRepository.Setup(x => x.GetKeyQuestionPartResponsesAsync(
                testDataShareRequestId, QuestionPartKeyType.DateRequired))
            .ReturnsAsync(() => new List<string> { testAnswer });

        var result = await testItems.KeyQuestionPartAnswerProviderService.GetDateRequiredQuestionPartAnswerAsync(
            testDataShareRequestId);

        Assert.That(result, Is.Null);
    }

    [Test]
    [TestCase("20251299")] // Day too big
    [TestCase("20251200")] // Day too small
    [TestCase("20259911")] // Month too big
    [TestCase("20250011")] // Month too small
    public async Task GivenTheAnswerStoredForTheQuestionPartHasInvalidDate_WhenIGetDateRequiredQuestionPartAnswerAsync_ThenAnErrorIsLogged(
        string testAnswer)
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockKeyQuestionPartAnswersRepository.Setup(x => x.GetKeyQuestionPartResponsesAsync(
                testDataShareRequestId, QuestionPartKeyType.DateRequired))
            .ReturnsAsync(() => new List<string> { testAnswer });

        await testItems.KeyQuestionPartAnswerProviderService.GetDateRequiredQuestionPartAnswerAsync(
            testDataShareRequestId);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "KeyQuestionPart 'Date Required' has an invalid date");
    }

    [Test]
    public async Task GivenTheAnswerStoredForTheQuestionPartHasAValidDate_WhenIGetDateRequiredQuestionPartAnswerAsync_ThenTheStoredValueIsReturned()
    {
        var testItems = CreateTestItems();
        
        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testDateTime = testItems.Fixture.Create<DateTime>();

        var testAnswer = testDateTime.ToString("yyyyMMdd");

        testItems.MockKeyQuestionPartAnswersRepository.Setup(x => x.GetKeyQuestionPartResponsesAsync(
                testDataShareRequestId, QuestionPartKeyType.DateRequired))
            .ReturnsAsync(() => new List<string> { testAnswer });

        var result = await testItems.KeyQuestionPartAnswerProviderService.GetDateRequiredQuestionPartAnswerAsync(
            testDataShareRequestId);

        Assert.Multiple(() =>
        {
            var resultAnswer = result!.Value;

            Assert.That(resultAnswer.Year, Is.EqualTo(testDateTime.Year));
            Assert.That(resultAnswer.Month, Is.EqualTo(testDateTime.Month));
            Assert.That(resultAnswer.Day, Is.EqualTo(testDateTime.Day));
            Assert.That(resultAnswer.Hour, Is.Zero);
            Assert.That(resultAnswer.Minute, Is.Zero);
            Assert.That(resultAnswer.Second, Is.Zero);
            Assert.That(resultAnswer.Millisecond, Is.Zero);
            Assert.That(resultAnswer.Kind, Is.EqualTo(DateTimeKind.Utc));
        });
    }
    #endregion

    #region GetProjectAimsQuestionPartAnswerAsync() Tests
    [Test]
    public async Task GivenMultipleAnswersStoredForTheQuestionPart_WhenIGetProjectAimsQuestionPartAnswerAsync_ThenNullIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockKeyQuestionPartAnswersRepository.Setup(x => x.GetKeyQuestionPartResponsesAsync(
                testDataShareRequestId, QuestionPartKeyType.ProjectAims))
            .ReturnsAsync(() => new List<string> { "thing 1", "thing 2" });

        var result = await testItems.KeyQuestionPartAnswerProviderService.GetProjectAimsQuestionPartAnswerAsync(
            testDataShareRequestId);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GivenMultipleAnswersStoredForTheQuestionPart_WhenIGetProjectAimsQuestionPartAnswerAsync_ThenAnErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockKeyQuestionPartAnswersRepository.Setup(x => x.GetKeyQuestionPartResponsesAsync(
                testDataShareRequestId, QuestionPartKeyType.ProjectAims))
            .ReturnsAsync(() => new List<string> { "thing 1", "thing 2" });

        await testItems.KeyQuestionPartAnswerProviderService.GetProjectAimsQuestionPartAnswerAsync(
            testDataShareRequestId);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "KeyQuestionPart 'Project Aims' has multiple answers");
    }

    [Test]
    public async Task GivenNoAnswerStoredForTheQuestionPart_WhenIGetProjectAimsQuestionPartAnswerAsync_ThenNullIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockKeyQuestionPartAnswersRepository.Setup(x => x.GetKeyQuestionPartResponsesAsync(
                testDataShareRequestId, QuestionPartKeyType.ProjectAims))
            .ReturnsAsync(() => new List<string>());

        var result = await testItems.KeyQuestionPartAnswerProviderService.GetProjectAimsQuestionPartAnswerAsync(
            testDataShareRequestId);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GivenAnswerStoredForTheQuestionPart_WhenIGetProjectAimsQuestionPartAnswerAsync_ThenTheStoredAnswerIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testAnswer = testItems.Fixture.Create<string>();

        testItems.MockKeyQuestionPartAnswersRepository.Setup(x => x.GetKeyQuestionPartResponsesAsync(
                testDataShareRequestId, QuestionPartKeyType.ProjectAims))
            .ReturnsAsync(() => new List<string>{ testAnswer });

        var result = await testItems.KeyQuestionPartAnswerProviderService.GetProjectAimsQuestionPartAnswerAsync(
            testDataShareRequestId);

        Assert.That(result, Is.EqualTo(testAnswer));
    }
    #endregion

    #region GetDataTypesQuestionPartAnswerAsync() Tests
    [Test]
    public async Task GivenMultipleAnswersStoredForTheQuestionPart_WhenIGetDataTypesQuestionPartAnswerAsync_ThenAllAnswersAreReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testAnswers = testItems.Fixture.CreateMany<string>().ToList();

        testItems.MockKeyQuestionPartAnswersRepository.Setup(x => x.GetKeyQuestionPartResponsesAsync(
                testDataShareRequestId, QuestionPartKeyType.DataTypes))
            .ReturnsAsync(() => testAnswers);

        var result = await testItems.KeyQuestionPartAnswerProviderService.GetDataTypesQuestionPartAnswerAsync(
            testDataShareRequestId);

        Assert.Multiple(() =>
        {
            var resultValues = result.ToList();

            Assert.That(resultValues, Has.Exactly(testAnswers.Count).Items);

            foreach (var testAnswer in testAnswers)
            {
                Assert.That(resultValues.Any(x => x.Equals(testAnswer)), Is.True);
            }
        });
    }

    [Test]
    public async Task GivenNoAnswerStoredForTheQuestionPart_WhenIGetDataTypesQuestionPartAnswerAsync_ThenAnEmptyCollectionIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        
        testItems.MockKeyQuestionPartAnswersRepository.Setup(x => x.GetKeyQuestionPartResponsesAsync(
                testDataShareRequestId, QuestionPartKeyType.DataTypes))
            .ReturnsAsync(() => new List<string>());

        var result = await testItems.KeyQuestionPartAnswerProviderService.GetDataTypesQuestionPartAnswerAsync(
            testDataShareRequestId);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GivenAnswerStoredForTheQuestionPart_WhenIGetDataTypesQuestionPartAnswerAsync_ThenTheStoredAnswerIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testAnswer = testItems.Fixture.Create<string>();

        testItems.MockKeyQuestionPartAnswersRepository.Setup(x => x.GetKeyQuestionPartResponsesAsync(
                testDataShareRequestId, QuestionPartKeyType.DataTypes))
            .ReturnsAsync(() => new List<string> { testAnswer });

        var result = await testItems.KeyQuestionPartAnswerProviderService.GetDataTypesQuestionPartAnswerAsync(
            testDataShareRequestId);

        Assert.Multiple(() =>
        {
            var resultValues = result.ToList();

            Assert.That(resultValues, Has.One.Items);
            Assert.That(resultValues.Single(), Is.EqualTo(testAnswer));
        });
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockLogger = Mock.Get(fixture.Freeze<ILogger<KeyQuestionPartAnswerProviderService>>());
        var mockKeyQuestionPartAnswersRepository = Mock.Get(fixture.Freeze<IKeyQuestionPartAnswersRepository>());

        var keyQuestionPartAnswerProviderService = new KeyQuestionPartAnswerProviderService(
            mockLogger.Object,
            mockKeyQuestionPartAnswersRepository.Object);

        return new TestItems(
            fixture,
            keyQuestionPartAnswerProviderService,
            mockLogger,
            mockKeyQuestionPartAnswersRepository);
    }

    private class TestItems(
        IFixture fixture,
        IKeyQuestionPartAnswerProviderService keyQuestionPartAnswerProviderService,
        Mock<ILogger<KeyQuestionPartAnswerProviderService>> mockLogger,
        Mock<IKeyQuestionPartAnswersRepository> mockKeyQuestionPartAnswersRepository)
    {
        public IFixture Fixture { get; } = fixture;
        public IKeyQuestionPartAnswerProviderService KeyQuestionPartAnswerProviderService { get; } = keyQuestionPartAnswerProviderService;
        public Mock<ILogger<KeyQuestionPartAnswerProviderService>> MockLogger { get; } = mockLogger;
        public Mock<IKeyQuestionPartAnswersRepository> MockKeyQuestionPartAnswersRepository { get; } = mockKeyQuestionPartAnswersRepository;
    }
    #endregion
}

