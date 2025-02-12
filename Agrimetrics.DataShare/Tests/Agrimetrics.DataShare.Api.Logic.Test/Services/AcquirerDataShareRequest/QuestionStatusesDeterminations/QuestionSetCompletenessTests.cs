using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionSets;
using Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestQuestionStatusesDeterminations;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AcquirerDataShareRequest.DataShareRequestQuestionStatusesDeterminations;

public class DataShareRequestQuestionSetCompletenessDeterminationTests
{
    #region DetermineDataShareRequestQuestionSetCompleteness() Tests
    [Test]
    public void GivenANullSetOfQuestionsSetQuestionStatuses_WhenIDetermineDataShareRequestQuestionSetCompleteness_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.DataShareRequestQuestionSetCompletenessDetermination.DetermineDataShareRequestQuestionSetCompleteness(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("questionSetQuestionStatuses"));
    }

    [Test]
    public void GivenAnEmptySetOfQuestionsSetQuestionStatuses_WhenIDetermineDataShareRequestQuestionSetCompleteness_ThenResultContainsAnEmptySetOfQuestionsRequiringAResponse()
    {
        var testItems = CreateTestItems();

        IEnumerable<IDataShareRequestQuestionSetQuestionStatusDataModel> questionSetQuestionStatuses = [];

        var dataShareRequestQuestionSetCompletenessResult = testItems.DataShareRequestQuestionSetCompletenessDetermination.DetermineDataShareRequestQuestionSetCompleteness(
            questionSetQuestionStatuses);

        Assert.That(dataShareRequestQuestionSetCompletenessResult.QuestionsRequiringAResponse, Is.Empty);
    }

    [Test]
    public void GivenQuestionsOfAllStatuses_WhenICDetermineDataShareRequestQuestionSetCompleteness_ThenTheQuestionRequiringAResponseAreReturned()
    {
        var testItems = CreateTestItems();

        var testQuestionSetQuestionStatuses = Enum.GetValues<QuestionStatusType>().Select(questionStatus =>
        {
            var mockDataShareRequestQuestionStatusDataModel = new Mock<IDataShareRequestQuestionSetQuestionStatusDataModel>();

            mockDataShareRequestQuestionStatusDataModel.SetupGet(x => x.QuestionId).Returns(Guid.NewGuid);
            mockDataShareRequestQuestionStatusDataModel.SetupGet(x => x.QuestionStatus).Returns(questionStatus);

            return mockDataShareRequestQuestionStatusDataModel.Object;
        });

        var questionSetCompletenessDeterminationResult = 
            testItems.DataShareRequestQuestionSetCompletenessDetermination.DetermineDataShareRequestQuestionSetCompleteness(testQuestionSetQuestionStatuses);

        var result = questionSetCompletenessDeterminationResult.QuestionsRequiringAResponse.ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Exactly(3).Items);

            Assert.That(result.Any(x => x.QuestionStatus == QuestionStatusType.NotSet));
            Assert.That(result.Any(x => x.QuestionStatus == QuestionStatusType.CannotStartYet));
            Assert.That(result.Any(x => x.QuestionStatus == QuestionStatusType.NotStarted));
        });

    }
    #endregion

    #region DetermineDataShareRequestQuestionSetSectionCompleteness() Tests
    [Test]
    public void GivenANullQuestionSetSection_WhenIDetermineDataShareRequestQuestionSetSectionCompleteness_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.DataShareRequestQuestionSetCompletenessDetermination.DetermineDataShareRequestQuestionSetSectionCompleteness(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("questionSetSection"));
    }

    [Test]
    public void GivenAQuestionSetSectionWithNoQuestions_WhenIDetermineDataShareRequestQuestionSetSectionCompleteness_ThenResultContainsAnEmptySetOfQuestionsRequiringAResponse()
    {
        var testItems = CreateTestItems();

        var questionSetSection = new QuestionSetSectionSummaryModelData
        {
            QuestionSetSection_QuestionSummaries = []
        };

        var dataShareRequestQuestionSetSectionCompletionDeterminationResult = testItems.DataShareRequestQuestionSetCompletenessDetermination.DetermineDataShareRequestQuestionSetSectionCompleteness(
            questionSetSection);

        Assert.That(dataShareRequestQuestionSetSectionCompletionDeterminationResult.QuestionsRequiringAResponse, Is.Empty);
    }

    [Test]
    public void GivenAQuestionSetSectionWithQuestionsOfAllStatuses_WhenIConstructAnInstanceOfDataShareRequestQuestionSetCompletenessDeterminationResult_ThenQuestionsRequiringAResponseAreReturned()
    {
        var testItems = CreateTestItems();

        var questionsOfEachStatus = Enum.GetValues<QuestionStatusType>()
            .Select(questionStatus => new QuestionSummaryModelData {Question_QuestionStatus = questionStatus});

        var testQuestionSetSection = new QuestionSetSectionSummaryModelData
        {
            QuestionSetSection_QuestionSummaries = questionsOfEachStatus.ToList()
        };

        var dataShareRequestQuestionSetSectionCompletionDeterminationResult =
            testItems.DataShareRequestQuestionSetCompletenessDetermination.DetermineDataShareRequestQuestionSetSectionCompleteness(testQuestionSetSection);

        var result = dataShareRequestQuestionSetSectionCompletionDeterminationResult.QuestionsRequiringAResponse.ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Exactly(3).Items);

            Assert.That(result.Any(x => x.Question_QuestionStatus == QuestionStatusType.NotSet));
            Assert.That(result.Any(x => x.Question_QuestionStatus == QuestionStatusType.CannotStartYet));
            Assert.That(result.Any(x => x.Question_QuestionStatus == QuestionStatusType.NotStarted));
        });

    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var dataShareRequestQuestionSetCompletenessDetermination = new DataShareRequestQuestionSetCompletenessDetermination();

        return new TestItems(fixture,
            dataShareRequestQuestionSetCompletenessDetermination);
    }

    private class TestItems(
        IFixture fixture,
        IDataShareRequestQuestionSetCompletenessDetermination dataShareRequestQuestionSetCompletenessDetermination)
    {
        public IFixture Fixture { get; } = fixture;
        public IDataShareRequestQuestionSetCompletenessDetermination DataShareRequestQuestionSetCompletenessDetermination { get; } = dataShareRequestQuestionSetCompletenessDetermination;
    }
    #endregion
}