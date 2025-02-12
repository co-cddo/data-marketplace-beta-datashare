using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;

[TestFixture]
public class DataShareRequestAnswersSummaryQuestionPartAnswerResponseTest
{
    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponse_WhenISetOrderWithinQuestionPartAnswer_ThenOrderWithinQuestionPartAnswerIsSet(
        [Values(-1, 0, 999)] int testOrderWithinQuestionPartAnswer)
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponse = new DataShareRequestAnswersSummaryQuestionPartAnswerResponse();

        testDataShareRequestAnswersSummaryQuestionPartAnswerResponse.OrderWithinQuestionPartAnswer = testOrderWithinQuestionPartAnswer;

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponse.OrderWithinQuestionPartAnswer;

        Assert.That(result, Is.EqualTo(testOrderWithinQuestionPartAnswer));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponse_WhenISetANullQuestionPartAnswerResponseItem_ThenQuestionPartAnswerResponseItemIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponse = new DataShareRequestAnswersSummaryQuestionPartAnswerResponse();

        var testQuestionPartAnswerResponseItem = (TestDataShareRequestAnswersSummaryQuestionPartAnswerResponseItem?) null;

        testDataShareRequestAnswersSummaryQuestionPartAnswerResponse.QuestionPartAnswerResponseItem = testQuestionPartAnswerResponseItem;

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponse.QuestionPartAnswerResponseItem;

        Assert.That(result, Is.SameAs(testQuestionPartAnswerResponseItem));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponse_WhenISetQuestionPartAnswerResponseItem_ThenQuestionPartAnswerResponseItemIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponse = new DataShareRequestAnswersSummaryQuestionPartAnswerResponse();

        var testQuestionPartAnswerResponseItem = new TestDataShareRequestAnswersSummaryQuestionPartAnswerResponseItem();

        testDataShareRequestAnswersSummaryQuestionPartAnswerResponse.QuestionPartAnswerResponseItem = testQuestionPartAnswerResponseItem;

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponse.QuestionPartAnswerResponseItem;

        Assert.That(result, Is.SameAs(testQuestionPartAnswerResponseItem));
    }

    private class TestDataShareRequestAnswersSummaryQuestionPartAnswerResponseItem : DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemBase;
}