using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswerResponses;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.Answers.DsrQuestionAnswerResponses;

[TestFixture]
public class DsrQuestionAnswerPartResponseWriteModelDataTests
{
    [Test]
    public void GivenADataShareRequestQuestionAnswerPartResponseWriteModelData_WhenISetOrderWithinAnswerPart_ThenOrderWithinAnswerPartIsSet(
        [Values(-1, 0, 999)] int testOrderWithinAnswerPart)
    {
        var testDataShareRequestQuestionAnswerPartResponseWriteModelData = new TestDataShareRequestQuestionAnswerPartResponseWriteModelData
        {
            OrderWithinAnswerPart = testOrderWithinAnswerPart
        };

        var result = testDataShareRequestQuestionAnswerPartResponseWriteModelData.OrderWithinAnswerPart;

        Assert.That(result, Is.EqualTo(testOrderWithinAnswerPart));
    }

    private class TestDataShareRequestQuestionAnswerPartResponseWriteModelData : DataShareRequestQuestionAnswerPartResponseWriteModelData;
}