using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.Answers;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Answers.Answers;

[TestFixture]
public class QuestionPartAnswerResponseTests
{
    [Theory]
    public void GivenAQuestionPartAnswerResponse_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testQuestionPartAnswerResponse = new QuestionPartAnswerResponse();

        testQuestionPartAnswerResponse.InputType = testInputType;

        var result = testQuestionPartAnswerResponse.InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }

    [Test]
    public void GivenAQuestionPartAnswerResponse_WhenISetOrderWithinAnswerPart_ThenOrderWithinAnswerPartIsSet(
        [Values(-1, 0, 999)] int testOrderWithinAnswerPart)
    {
        var testQuestionPartAnswerResponse = new QuestionPartAnswerResponse();

        testQuestionPartAnswerResponse.OrderWithinAnswerPart = testOrderWithinAnswerPart;

        var result = testQuestionPartAnswerResponse.OrderWithinAnswerPart;

        Assert.That(result, Is.EqualTo(testOrderWithinAnswerPart));
    }

    [Test]
    public void GivenAQuestionPartAnswerResponse_WhenISetANullResponseItem_ThenResponseItemIsSet()
    {
        var testQuestionPartAnswerResponse = new QuestionPartAnswerResponse();

        var testResponseItem = (TestQuestionPartAnswerResponseItem?) null;

        testQuestionPartAnswerResponse.ResponseItem = testResponseItem;

        var result = testQuestionPartAnswerResponse.ResponseItem;

        Assert.That(result, Is.EqualTo(testResponseItem));
    }

    [Test]
    public void GivenAQuestionPartAnswerResponse_WhenISetResponseItem_ThenResponseItemIsSet()
    {
        var testQuestionPartAnswerResponse = new QuestionPartAnswerResponse();

        var testResponseItem = new TestQuestionPartAnswerResponseItem();

        testQuestionPartAnswerResponse.ResponseItem = testResponseItem;

        var result = testQuestionPartAnswerResponse.ResponseItem;

        Assert.That(result, Is.SameAs(testResponseItem));
    }

    [Test]
    public void GivenAQuestionPartAnswerResponse_WhenISetAnEmptySetOfValidationErrors_ThenValidationErrorsIsSet()
    {
        var testQuestionPartAnswerResponse = new QuestionPartAnswerResponse();

        var testValidationErrors = new List<string>();

        testQuestionPartAnswerResponse.ValidationErrors = testValidationErrors;

        var result = testQuestionPartAnswerResponse.ValidationErrors;

        Assert.That(result, Is.EqualTo(testValidationErrors));
    }

    [Test]
    public void GivenAQuestionPartAnswerResponse_WhenISetValidationErrors_ThenValidationErrorsIsSet()
    {
        var testQuestionPartAnswerResponse = new QuestionPartAnswerResponse();

        var testValidationErrors = new List<string> {"aaa", "bbb", "ccc"};

        testQuestionPartAnswerResponse.ValidationErrors = testValidationErrors;

        var result = testQuestionPartAnswerResponse.ValidationErrors;

        Assert.That(result, Is.EqualTo(testValidationErrors));
    }

    private class TestQuestionPartAnswerResponseItem : QuestionPartAnswerResponseItemBase;
}