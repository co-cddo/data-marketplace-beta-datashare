using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.Answers.DsrAnswerSummaries;

[TestFixture]
public class DataShareRequestAnswersSummaryQuestionPartResponseModelDataTests
{
    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartResponseModelData_WhenISetResponseId_ThenResponseIdIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionPartResponseModelData = new DataShareRequestAnswersSummaryQuestionPartResponseModelData();

        var testResponseId = new Guid("3F897B76-DE1C-44F7-AA96-A3207884FDC5");

        testDataShareRequestAnswersSummaryQuestionPartResponseModelData.DataShareRequestAnswersSummaryQuestionPartResponse_ResponseId = testResponseId;

        var result = testDataShareRequestAnswersSummaryQuestionPartResponseModelData.DataShareRequestAnswersSummaryQuestionPartResponse_ResponseId;

        Assert.That(result, Is.EqualTo(testResponseId));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartResponseModelData_WhenISetOrderWithinQuestionPart_ThenOrderWithinQuestionPartIsSet(
        [Values(-1, 0, 999)] int testOrderWithinQuestionPart)
    {
        var testDataShareRequestAnswersSummaryQuestionPartResponseModelData = new DataShareRequestAnswersSummaryQuestionPartResponseModelData();


        testDataShareRequestAnswersSummaryQuestionPartResponseModelData.DataShareRequestAnswersSummaryQuestionPartResponse_OrderWithinQuestionPart = testOrderWithinQuestionPart;

        var result = testDataShareRequestAnswersSummaryQuestionPartResponseModelData.DataShareRequestAnswersSummaryQuestionPartResponse_OrderWithinQuestionPart;

        Assert.That(result, Is.EqualTo(testOrderWithinQuestionPart));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartResponseModelData_WhenISetResponseItem_ThenResponseItemIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionPartResponseModelData = new DataShareRequestAnswersSummaryQuestionPartResponseModelData();

        var testResponseItem = new TestDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemModelData();

        testDataShareRequestAnswersSummaryQuestionPartResponseModelData.DataShareRequestAnswersSummaryQuestionPart_ResponseItem = testResponseItem;

        var result = testDataShareRequestAnswersSummaryQuestionPartResponseModelData.DataShareRequestAnswersSummaryQuestionPart_ResponseItem;

        Assert.That(result, Is.EqualTo(testResponseItem));
    }

    private class TestDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemModelData : DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemModelData;
}