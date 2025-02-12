using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestQuestionStatusesDeterminations;
using AutoFixture;
using AutoFixture.AutoMoq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AcquirerDataShareRequest.DataShareRequestQuestionStatusesDeterminations;

public class DataShareRequestQuestionSetSectionCompletionDeterminationResultTests
{
    [Test]
    public void GivenAnEmptySetOfQuestionsRequiringAResponse_WhenIConstructAnInstanceOfDataShareRequestQuestionSetSectionCompletionDeterminationResult_ThenQuestionsRequiringAResponseIsEmpty()
    {
        var dataShareRequestQuestionSetSectionCompletionDeterminationResult = new DataShareRequestQuestionSetSectionCompletionDeterminationResult
        {
            QuestionsRequiringAResponse = []
        };

        Assert.That(dataShareRequestQuestionSetSectionCompletionDeterminationResult.QuestionsRequiringAResponse, Is.Empty);
    }

    [Test]
    public void GivenASetOfQuestionsRequiringAResponse_WhenIConstructAnInstanceOfDataShareRequestQuestionSetSectionCompletionDeterminationResult_ThenQuestionsRequiringAResponseIsConfiguredToTheGivenSet()
    {
        var testItems = CreateTestItems();

        var testQuestionsRequiringAResponse = testItems.Fixture.CreateMany<QuestionSummaryModelData>();

        var dataShareRequestQuestionSetSectionCompletionDeterminationResult = new DataShareRequestQuestionSetSectionCompletionDeterminationResult
        {
            QuestionsRequiringAResponse = testQuestionsRequiringAResponse
        };

        Assert.That(dataShareRequestQuestionSetSectionCompletionDeterminationResult.QuestionsRequiringAResponse, Is.EqualTo(testQuestionsRequiringAResponse));
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