using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests;

[TestFixture]
public class SubmissionDetailsAnswerResponseTests
{
    [Test]
    public void GivenASubmissionDetailsAnswerResponse_WhenISetOrderWithinAnswer_ThenOrderWithinAnswerIsSet(
        [Values(-1, 0, 999)] int testOrderWithinAnswer)
    {
        var testSubmissionDetailsAnswerResponse = new SubmissionDetailsAnswerResponse();

        testSubmissionDetailsAnswerResponse.OrderWithinAnswer = testOrderWithinAnswer;

        var result = testSubmissionDetailsAnswerResponse.OrderWithinAnswer;

        Assert.That(result, Is.EqualTo(testOrderWithinAnswer));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerResponse_WhenISetResponseItem_ThenResponseItemIsSet()
    {
        var testSubmissionDetailsAnswerResponse = new SubmissionDetailsAnswerResponse();

        var testResponseItem = new TestSubmissionDetailsAnswerResponseItem();

        testSubmissionDetailsAnswerResponse.ResponseItem = testResponseItem;

        var result = testSubmissionDetailsAnswerResponse.ResponseItem;

        Assert.That(result, Is.SameAs(testResponseItem));
    }

    private class TestSubmissionDetailsAnswerResponseItem : SubmissionDetailsAnswerResponseItemBase;
}