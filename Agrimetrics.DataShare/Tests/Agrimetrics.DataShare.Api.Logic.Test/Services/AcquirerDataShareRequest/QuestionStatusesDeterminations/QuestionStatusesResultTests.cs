using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestQuestionStatusesDeterminations;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AcquirerDataShareRequest.DataShareRequestQuestionStatusesDeterminations;

[TestFixture]
public class DataShareRequestQuestionStatusesDeterminationResultTests
{
    [Theory]
    public void GivenQuestionsRemainThatRequireAResponse_WhenIConstructAnInstanceOfDataShareRequestQuestionStatusesDeterminationResult_ThenQuestionsRemainThatRequireAResponseIsSetToTheGivenValue(
        bool questionsRemainThatRequireAResponse)
    {
        var questionStatusesDeterminationResult = new DataShareRequestQuestionStatusesDeterminationResult
        {
            QuestionsRemainThatRequireAResponse = questionsRemainThatRequireAResponse,
            QuestionStatusDeterminationResults = []
        };

        Assert.That(questionStatusesDeterminationResult.QuestionsRemainThatRequireAResponse, Is.EqualTo(questionsRemainThatRequireAResponse));
    }

    [Test]
    public void GivenAnyQuestionStatusDeterminationResults_WhenIConstructAnInstanceOfDataShareRequestQuestionStatusesDeterminationResult_ThenQuestionStatusDeterminationResultsIsConfiguredToTheGivenValue()
    {
        var testItems = CreateTestItems();

        var testQuestionStatusDeterminationResults =
            testItems.Fixture.CreateMany<IDataShareRequestQuestionStatusDeterminationResult>();

        var questionStatusesDeterminationResult = new DataShareRequestQuestionStatusesDeterminationResult
        {
            QuestionsRemainThatRequireAResponse = It.IsAny<bool>(),
            QuestionStatusDeterminationResults = testQuestionStatusDeterminationResults
        };

        Assert.That(questionStatusesDeterminationResult.QuestionStatusDeterminationResults, Is.EqualTo(testQuestionStatusDeterminationResults));
    }

    [Test]
    public void GivenQuestionStatusData_WhenIConstructAnInstanceOfDataShareRequestQuestionStatusDeterminationResult_ThenQuestionStatusDataIsConfiguredToTheGivenValue()
    {
        var testItems = CreateTestItems();

        var testQuestionStatusData = testItems.Fixture.Create<IDataShareRequestQuestionSetQuestionStatusDataModel>();

        var questionStatusDeterminationResult = new DataShareRequestQuestionStatusDeterminationResult
        {
            QuestionSetQuestionStatusData = testQuestionStatusData,
            PreviousQuestionStatus = It.IsAny<QuestionStatusType>()
        };

        Assert.That(questionStatusDeterminationResult.QuestionSetQuestionStatusData, Is.EqualTo(testQuestionStatusData));
    }

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        return new TestItems(fixture);
    }

    private class TestItems(IFixture fixture)
    {
        public IFixture Fixture { get; } = fixture;
    }
    #endregion
}