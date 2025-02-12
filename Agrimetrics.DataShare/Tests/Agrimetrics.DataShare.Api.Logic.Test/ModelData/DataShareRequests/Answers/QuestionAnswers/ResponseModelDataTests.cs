using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.Answers.DsrRequestQuestionAnswers;

[TestFixture]
public class QuestionPartAnswerResponseModelDataTests
{
    [Test]
    public void GivenAQuestionPartAnswerResponseModelData_WhenISetId_ThenIdIsSet()
    {
        var testQuestionPartAnswerResponseModelData = new QuestionPartAnswerResponseModelData();

        var testId = new Guid("468E137A-46DA-4779-97D2-671652AF8A3E");

        testQuestionPartAnswerResponseModelData.QuestionPartAnswerResponse_Id = testId;

        var result = testQuestionPartAnswerResponseModelData.QuestionPartAnswerResponse_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenAQuestionPartAnswerResponseModelData_WhenISetOrderWithinAnswerPart_ThenOrderWithinAnswerPartIsSet(
        [Values(-1, 0, 999)] int testOrderWithinAnswerPart)
    {
        var testQuestionPartAnswerResponseModelData = new QuestionPartAnswerResponseModelData();

        testQuestionPartAnswerResponseModelData.QuestionPartAnswerResponse_OrderWithinAnswerPart = testOrderWithinAnswerPart;

        var result = testQuestionPartAnswerResponseModelData.QuestionPartAnswerResponse_OrderWithinAnswerPart;

        Assert.That(result, Is.EqualTo(testOrderWithinAnswerPart));
    }

    [Theory]
    public void GivenAQuestionPartAnswerResponseModelData_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testQuestionPartAnswerResponseModelData = new QuestionPartAnswerResponseModelData();

        testQuestionPartAnswerResponseModelData.QuestionPartAnswerResponse_InputType = testInputType;

        var result = testQuestionPartAnswerResponseModelData.QuestionPartAnswerResponse_InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }

    [Test]
    public void GivenAQuestionPartAnswerResponseModelData_WhenISetANullResponseItem_ThenResponseItemIsSet()
    {
        var testQuestionPartAnswerResponseModelData = new QuestionPartAnswerResponseModelData();

        var testResponseItem = (QuestionPartAnswerResponseItemModelData?) null;

        testQuestionPartAnswerResponseModelData.QuestionPartAnswerResponse_ResponseItem = testResponseItem;

        var result = testQuestionPartAnswerResponseModelData.QuestionPartAnswerResponse_ResponseItem;

        Assert.That(result, Is.EqualTo(testResponseItem));
    }

    [Test]
    public void GivenAQuestionPartAnswerResponseModelData_WhenISetResponseItem_ThenResponseItemIsSet()
    {
        var testQuestionPartAnswerResponseModelData = new QuestionPartAnswerResponseModelData();

        var testResponseItem = new TestQuestionPartAnswerResponseItemModelData();

        testQuestionPartAnswerResponseModelData.QuestionPartAnswerResponse_ResponseItem = testResponseItem;

        var result = testQuestionPartAnswerResponseModelData.QuestionPartAnswerResponse_ResponseItem;

        Assert.That(result, Is.EqualTo(testResponseItem));
    }

    private class TestQuestionPartAnswerResponseItemModelData : QuestionPartAnswerResponseItemModelData;
}