using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;

[TestFixture]
public class DataShareRequestQuestionAnswerPartResponseBaseTests
{
    [Theory]
    public void GivenADataShareRequestQuestionAnswerPartResponseBase_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testDataShareRequestQuestionAnswerPartResponseBase = new DataShareRequestQuestionAnswerPartResponseBase();

        testDataShareRequestQuestionAnswerPartResponseBase.InputType = testInputType;

        var result = testDataShareRequestQuestionAnswerPartResponseBase.InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartResponseBase_WhenISetOrderWithinAnswerPart_ThenOrderWithinAnswerPartIsSet(
        [Values(-1, 0, 999)] int testOrderWithinAnswerPart)
    {
        var testDataShareRequestQuestionAnswerPartResponseBase = new DataShareRequestQuestionAnswerPartResponseBase();

        testDataShareRequestQuestionAnswerPartResponseBase.OrderWithinAnswerPart = testOrderWithinAnswerPart;

        var result = testDataShareRequestQuestionAnswerPartResponseBase.OrderWithinAnswerPart;

        Assert.That(result, Is.EqualTo(testOrderWithinAnswerPart));
    }

    [Theory]
    public void GivenADataShareRequestQuestionAnswerPartResponseBase_WhenISetMultipleResponsesAreAllowed_ThenMultipleResponsesAreAllowedIsSet(
        bool testMultipleResponsesAreAllowed)
    {
        var testDataShareRequestQuestionAnswerPartResponseBase = new DataShareRequestQuestionAnswerPartResponseBase();

        testDataShareRequestQuestionAnswerPartResponseBase.MultipleResponsesAreAllowed = testMultipleResponsesAreAllowed;

        var result = testDataShareRequestQuestionAnswerPartResponseBase.MultipleResponsesAreAllowed;

        Assert.That(result, Is.EqualTo(testMultipleResponsesAreAllowed));
    }
}