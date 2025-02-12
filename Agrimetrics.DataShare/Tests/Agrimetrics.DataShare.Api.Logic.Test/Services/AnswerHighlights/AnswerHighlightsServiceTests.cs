using AutoFixture.AutoMoq;
using AutoFixture;
using NUnit.Framework;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerHighlights;
using Microsoft.Extensions.Logging;
using Moq;
using Agrimetrics.DataShare.Api.Logic.Repositories.AnswerHighlights;
using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerHighlights;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Exceptions;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AnswerHighlights;

[TestFixture]
public class AnswerHighlightsServiceTests
{
    #region GetDataShareRequestsAnswerHighlightsAsync() Tests
    [Test]
    public async Task GivenADataShareRequestId_WhenIGetDataShareRequestsAnswerHighlightsAsync_ThenAListOfAnswerHighlightsForThatRequestIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        
        var testSelectedOptionModelDatas = testItems.Fixture
            .Build<DataShareRequestSelectedOptionModelData>()
            .CreateMany(1)
            .ToList();

        var testSelectedOptionsModelData = testItems.Fixture
            .Build<DataShareRequestSelectionOptionsModelData>()
            .With(x => x.DataShareRequestSelectionOptions_SelectedOptions, testSelectedOptionModelDatas)
            .Create();

        testItems.MockAnswerHighlightsRepository
            .Setup(x => x.GetDataShareRequestSelectionOptionsModelDataAsync(testDataShareRequestId))
            .ReturnsAsync(() => testSelectedOptionsModelData);

        var testQuestionSetSelectionOptionQuestionHighlightModelData1 = testItems.Fixture.Build<QuestionSetSelectionOptionQuestionHighlightModelData>()
            .With(x => x.QuestionSetSelectionOptionQuestionHighlight_SelectionOptionId, testSelectedOptionModelDatas[0].DataShareRequestSelectedOption_OptionSelectionId)
            .With(x => x.QuestionSetSelectionOptionQuestionHighlight_HighlightCondition, QuestionSetSelectionOptionQuestionHighlightConditionType.QuestionIsHighlightedIfOptionIsSelected)
            .With(x => x.QuestionSetSelectionOptionQuestionHighlight_ReasonHighlighted, "test highlight reason 1")
            .Create();

        var testQuestionSetSelectionOptionQuestionHighlightModelData2 = testItems.Fixture.Build<QuestionSetSelectionOptionQuestionHighlightModelData>()
            .With(x => x.QuestionSetSelectionOptionQuestionHighlight_SelectionOptionId, Guid.Empty)
            .With(x => x.QuestionSetSelectionOptionQuestionHighlight_HighlightCondition, QuestionSetSelectionOptionQuestionHighlightConditionType.QuestionIsHighlightedIfOptionIsNotSelected)
            .With(x => x.QuestionSetSelectionOptionQuestionHighlight_ReasonHighlighted, "test highlight reason 2")
            .Create();

        var testQuestionHighlightModelDatas = new List<QuestionSetSelectionOptionQuestionHighlightModelData>
        {
            testQuestionSetSelectionOptionQuestionHighlightModelData1,
            testQuestionSetSelectionOptionQuestionHighlightModelData2
        };

        testItems.MockAnswerHighlightsRepository
            .Setup(x => x.GetQuestionSetSelectionOptionQuestionHighlightModelDataAsync(testDataShareRequestId))
            .ReturnsAsync(() => testQuestionHighlightModelDatas);

        var result = (await testItems.AnswerHighlightsService.GetDataShareRequestsAnswerHighlightsAsync(testDataShareRequestId)).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Exactly(2).Items);

            Assert.That(result.Contains("test highlight reason 1"), Is.True);
            Assert.That(result.Contains("test highlight reason 2"), Is.True);
        });
    }

    [Test]
    public void GivenDataShareRequestsAnswerHighlightsWillThrowAnException_WhenIGetDataShareRequestsAnswerHighlightsAsync_ThenADataShareRequestGeneralExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testException = new Exception("oh noes!");

        testItems.MockAnswerHighlightsRepository
            .Setup(x => x.GetQuestionSetSelectionOptionQuestionHighlightModelDataAsync(testDataShareRequestId))
            .Throws(testException);
        
        Assert.Multiple(() =>
        {
            var resultException = Assert.ThrowsAsync<DataShareRequestGeneralException>(async () =>
                await testItems.AnswerHighlightsService.GetDataShareRequestsAnswerHighlightsAsync(testDataShareRequestId));

            Assert.That(resultException!.Message, Is.EqualTo("Failed to GetDataShareRequestsAnswerHighlights from AnswerHighlightsRepository"));
            Assert.That(resultException.InnerException, Is.SameAs(testException));
        });
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockLogger = Mock.Get(fixture.Freeze<ILogger<AnswerHighlightsService>>());
        var mockAnswerHighlightsRepository = Mock.Get(fixture.Freeze<IAnswerHighlightsRepository>());

        var answerHighlightsService = new AnswerHighlightsService(
            mockLogger.Object,
            mockAnswerHighlightsRepository.Object);

        return new TestItems(
            fixture,
            answerHighlightsService,
            mockAnswerHighlightsRepository);
    }

    private class TestItems(
        IFixture fixture,
        IAnswerHighlightsService answerHighlightsService,
        Mock<IAnswerHighlightsRepository> mockAnswerHighlightsRepository)
    {
        public IFixture Fixture { get; } = fixture;
        public IAnswerHighlightsService AnswerHighlightsService { get; } = answerHighlightsService;
        public Mock<IAnswerHighlightsRepository> MockAnswerHighlightsRepository { get; } = mockAnswerHighlightsRepository;
    }
    #endregion
}
