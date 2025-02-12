using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.Answers.DsrAnswerSummaries;

[TestFixture]
public class DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemModelDataTests
{
    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponseItemModelData_WhenISetResponseItemId_ThenResponseItemIdIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemModelData = new TestDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemModelData();

        var testResponseItemId = new Guid("902D9D3D-756F-4107-B995-CDF3D1346AFF");

        testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemModelData.DataShareRequestAnswersSummaryQuestionPartAnswerResponseItem_ResponseItemId = testResponseItemId;

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemModelData.DataShareRequestAnswersSummaryQuestionPartAnswerResponseItem_ResponseItemId;

        Assert.That(result, Is.EqualTo(testResponseItemId));
    }

    private class TestDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemModelData : DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemModelData;
}