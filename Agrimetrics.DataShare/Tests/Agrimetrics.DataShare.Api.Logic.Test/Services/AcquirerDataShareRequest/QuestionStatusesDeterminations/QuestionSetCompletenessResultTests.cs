using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestQuestionStatusesDeterminations;
using AutoFixture;
using AutoFixture.AutoMoq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AcquirerDataShareRequest.DataShareRequestQuestionStatusesDeterminations;

public class DataShareRequestQuestionSetCompletenessDeterminationResultTests
{
    [Test]
    public void GivenAnEmptySetOfQuestionsRequiringAResponse_WhenIConstructAnInstanceOfDataShareRequestQuestionSetCompletenessDeterminationResult_ThenQuestionsRequiringAResponseIsEmpty()
    {
        var dataShareRequestQuestionSetCompletenessDeterminationResult = new DataShareRequestQuestionSetCompletenessDeterminationResult
        {
            QuestionsRequiringAResponse = []
        };

        Assert.That(dataShareRequestQuestionSetCompletenessDeterminationResult.QuestionsRequiringAResponse, Is.Empty);
    }

    [Test]
    public void GivenASetOfQuestionsRequiringAResponse_WhenIConstructAnInstanceOfDataShareRequestQuestionSetCompletenessDeterminationResult_ThenQuestionsRequiringAResponseIsConfiguredToTheGivenSet()
    {
        var testItems = CreateTestItems();

        var testQuestionsRequiringAResponse = testItems.Fixture.CreateMany<IDataShareRequestQuestionSetQuestionStatusDataModel>();

        var dataShareRequestQuestionSetCompletenessDeterminationResult = new DataShareRequestQuestionSetCompletenessDeterminationResult
        {
            QuestionsRequiringAResponse = testQuestionsRequiringAResponse
        };

        Assert.That(dataShareRequestQuestionSetCompletenessDeterminationResult.QuestionsRequiringAResponse, Is.EqualTo(testQuestionsRequiringAResponse));
    }

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        return new TestItems(fixture);
    }

    private class TestItems(
        IFixture fixture)
    {
        public IFixture Fixture { get; } = fixture;
    }
    #endregion
}