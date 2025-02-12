using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestQuestionStatusesDeterminations;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AcquirerDataShareRequest.DataShareRequestQuestionStatusesDeterminations;

[TestFixture]
public class DataShareRequestQuestionStatusDeterminationResultTests
{
    [Theory]
    public void GivenAnyPreviousQuestionStatus_WhenIConstructAnInstanceOfDataShareRequestQuestionStatusDeterminationResult_ThenPreviousQuestionStatusIsConfiguredToTheGivenValue(
        QuestionStatusType testPreviousQuestionStatus)
    {
        var questionStatusDeterminationResult = new DataShareRequestQuestionStatusDeterminationResult
        {
            QuestionSetQuestionStatusData = It.IsAny<IDataShareRequestQuestionSetQuestionStatusDataModel>(),
            PreviousQuestionStatus = testPreviousQuestionStatus
        };

        Assert.That(questionStatusDeterminationResult.PreviousQuestionStatus, Is.EqualTo(testPreviousQuestionStatus));
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