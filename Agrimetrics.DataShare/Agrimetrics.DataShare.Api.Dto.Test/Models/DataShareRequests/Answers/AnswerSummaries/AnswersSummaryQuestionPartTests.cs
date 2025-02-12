using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;

[TestFixture]
public class DataShareRequestAnswersSummaryQuestionPartTests
{
    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPart_WhenISetOrderWithinQuestion_ThenOrderWithinQuestionIsSet(
        [Values(-1, 0, 999)] int testOrderWithinQuestion)
    {
        var testDataShareRequestAnswersSummaryQuestionPart = new DataShareRequestAnswersSummaryQuestionPart();

        testDataShareRequestAnswersSummaryQuestionPart.OrderWithinQuestion = testOrderWithinQuestion;

        var result = testDataShareRequestAnswersSummaryQuestionPart.OrderWithinQuestion;

        Assert.That(result, Is.EqualTo(testOrderWithinQuestion));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPart_WhenISetQuestionPartText_ThenQuestionPartTextIsSet(
        [Values("", "  ", "abc")] string testQuestionPartText)
    {
        var testDataShareRequestAnswersSummaryQuestionPart = new DataShareRequestAnswersSummaryQuestionPart();

        testDataShareRequestAnswersSummaryQuestionPart.QuestionPartText = testQuestionPartText;

        var result = testDataShareRequestAnswersSummaryQuestionPart.QuestionPartText;

        Assert.That(result, Is.EqualTo(testQuestionPartText));
    }

    [Theory]
    public void GivenADataShareRequestAnswersSummaryQuestionPart_WhenISetMultipleResponsesAllowed_ThenMultipleResponsesAllowedIsSet(
        bool testMultipleResponsesAllowed)
    {
        var testDataShareRequestAnswersSummaryQuestionPart = new DataShareRequestAnswersSummaryQuestionPart();

        testDataShareRequestAnswersSummaryQuestionPart.MultipleResponsesAllowed = testMultipleResponsesAllowed;

        var result = testDataShareRequestAnswersSummaryQuestionPart.MultipleResponsesAllowed;

        Assert.That(result, Is.EqualTo(testMultipleResponsesAllowed));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPart_WhenISetMultipleResponsesCollectionHeaderIfMultipleResponsesAllowed_ThenMultipleResponsesCollectionHeaderIfMultipleResponsesAllowedIsSet(
        [Values("", "  ", "abc")] string testMultipleResponsesCollectionHeaderIfMultipleResponsesAllowed)
    {
        var testDataShareRequestAnswersSummaryQuestionPart = new DataShareRequestAnswersSummaryQuestionPart();

        testDataShareRequestAnswersSummaryQuestionPart.MultipleResponsesCollectionHeaderIfMultipleResponsesAllowed = testMultipleResponsesCollectionHeaderIfMultipleResponsesAllowed;

        var result = testDataShareRequestAnswersSummaryQuestionPart.MultipleResponsesCollectionHeaderIfMultipleResponsesAllowed;

        Assert.That(result, Is.EqualTo(testMultipleResponsesCollectionHeaderIfMultipleResponsesAllowed));
    }

    [Theory]
    public void GivenADataShareRequestAnswersSummaryQuestionPart_WhenISetResponseInputType_ThenResponseInputTypeIsSet(
        QuestionPartResponseInputType testResponseInputType)
    {
        var testDataShareRequestAnswersSummaryQuestionPart = new DataShareRequestAnswersSummaryQuestionPart();

        testDataShareRequestAnswersSummaryQuestionPart.ResponseInputType = testResponseInputType;

        var result = testDataShareRequestAnswersSummaryQuestionPart.ResponseInputType;

        Assert.That(result, Is.EqualTo(testResponseInputType));
    }

    [Theory]
    public void GivenADataShareRequestAnswersSummaryQuestionPart_WhenISetResponseFormatType_ThenResponseFormatTypeIsSet(
        QuestionPartResponseFormatType testResponseFormatType)
    {
        var testDataShareRequestAnswersSummaryQuestionPart = new DataShareRequestAnswersSummaryQuestionPart();

        testDataShareRequestAnswersSummaryQuestionPart.ResponseFormatType = testResponseFormatType;

        var result = testDataShareRequestAnswersSummaryQuestionPart.ResponseFormatType;

        Assert.That(result, Is.EqualTo(testResponseFormatType));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPart_WhenISetAnEmptySetOfResponses_ThenResponsesIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionPart = new DataShareRequestAnswersSummaryQuestionPart();

        var testResponses = new List<DataShareRequestAnswersSummaryQuestionPartAnswerResponse>();

        testDataShareRequestAnswersSummaryQuestionPart.Responses = testResponses;

        var result = testDataShareRequestAnswersSummaryQuestionPart.Responses;

        Assert.That(result, Is.EqualTo(testResponses));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPart_WhenISetResponses_ThenResponsesIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionPart = new DataShareRequestAnswersSummaryQuestionPart();

        var testResponses = new List<DataShareRequestAnswersSummaryQuestionPartAnswerResponse>{ new(), new(), new() };

        testDataShareRequestAnswersSummaryQuestionPart.Responses = testResponses;

        var result = testDataShareRequestAnswersSummaryQuestionPart.Responses;

        Assert.That(result, Is.EqualTo(testResponses));
    }
}