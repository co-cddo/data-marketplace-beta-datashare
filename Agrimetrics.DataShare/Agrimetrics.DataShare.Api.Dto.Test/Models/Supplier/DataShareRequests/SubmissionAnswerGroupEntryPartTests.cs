using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests;

[TestFixture]
public class SubmissionAnswerGroupEntryPartTests
{
    [Test]
    public void GivenASubmissionAnswerGroupEntryPart_WhenISetOrderWithinGroupEntry_ThenOrderWithinGroupEntryIsSet(
        [Values(-1, 0, 999)] int testOrderWithinGroupEntry)
    {
        var testSubmissionAnswerGroupEntryPart = new SubmissionAnswerGroupEntryPart();

        testSubmissionAnswerGroupEntryPart.OrderWithinGroupEntry = testOrderWithinGroupEntry;

        var result = testSubmissionAnswerGroupEntryPart.OrderWithinGroupEntry;

        Assert.That(result, Is.EqualTo(testOrderWithinGroupEntry));
    }

    [Test]
    public void GivenASubmissionAnswerGroupEntryPart_WhenISetQuestionPartText_ThenQuestionPartTextIsSet(
        [Values("", "  ", "abc")] string testQuestionPartText)
    {
        var testSubmissionAnswerGroupEntryPart = new SubmissionAnswerGroupEntryPart();

        testSubmissionAnswerGroupEntryPart.QuestionPartText = testQuestionPartText;

        var result = testSubmissionAnswerGroupEntryPart.QuestionPartText;

        Assert.That(result, Is.EqualTo(testQuestionPartText));
    }

    [Theory]
    public void GivenASubmissionAnswerGroupEntryPart_WhenISetResponseInputType_ThenResponseInputTypeIsSet(
        QuestionPartResponseInputType testResponseInputType)
    {
        var testSubmissionAnswerGroupEntryPart = new SubmissionAnswerGroupEntryPart();

        testSubmissionAnswerGroupEntryPart.ResponseInputType = testResponseInputType;

        var result = testSubmissionAnswerGroupEntryPart.ResponseInputType;

        Assert.That(result, Is.EqualTo(testResponseInputType));
    }

    [Theory]
    public void GivenASubmissionAnswerGroupEntryPart_WhenISetResponseFormatType_ThenResponseFormatTypeIsSet(
        QuestionPartResponseFormatType testResponseFormatType)
    {
        var testSubmissionAnswerGroupEntryPart = new SubmissionAnswerGroupEntryPart();

        testSubmissionAnswerGroupEntryPart.ResponseFormatType = testResponseFormatType;

        var result = testSubmissionAnswerGroupEntryPart.ResponseFormatType;

        Assert.That(result, Is.EqualTo(testResponseFormatType));
    }

    [Theory]
    public void GivenASubmissionAnswerGroupEntryPart_WhenISetMultipleResponsesAllowed_ThenMultipleResponsesAllowedIsSet(
        bool testMultipleResponsesAllowed)
    {
        var testSubmissionAnswerGroupEntryPart = new SubmissionAnswerGroupEntryPart();

        testSubmissionAnswerGroupEntryPart.MultipleResponsesAllowed = testMultipleResponsesAllowed;

        var result = testSubmissionAnswerGroupEntryPart.MultipleResponsesAllowed;

        Assert.That(result, Is.EqualTo(testMultipleResponsesAllowed));
    }

    [Test]
    public void GivenASubmissionAnswerGroupEntryPart_WhenISetCollectionDescriptionIfMultipleResponsesAllowed_ThenCollectionDescriptionIfMultipleResponsesAllowedIsSet(
        [Values(null, "", "  ", "abc")] string? testCollectionDescriptionIfMultipleResponsesAllowed)
    {
        var testSubmissionAnswerGroupEntryPart = new SubmissionAnswerGroupEntryPart();

        testSubmissionAnswerGroupEntryPart.CollectionDescriptionIfMultipleResponsesAllowed = testCollectionDescriptionIfMultipleResponsesAllowed;

        var result = testSubmissionAnswerGroupEntryPart.CollectionDescriptionIfMultipleResponsesAllowed;

        Assert.That(result, Is.EqualTo(testCollectionDescriptionIfMultipleResponsesAllowed));
    }

    [Test]
    public void GivenASubmissionAnswerGroupEntryPart_WhenISetAnEmptySetOfResponses_ThenResponsesIsSet()
    {
        var testSubmissionAnswerGroupEntryPart = new SubmissionAnswerGroupEntryPart();

        var testResponses = new List<SubmissionDetailsAnswerResponse>();

        testSubmissionAnswerGroupEntryPart.Responses = testResponses;

        var result = testSubmissionAnswerGroupEntryPart.Responses;

        Assert.That(result, Is.EqualTo(testResponses));
    }

    [Test]
    public void GivenASubmissionAnswerGroupEntryPart_WhenISetResponses_ThenResponsesIsSet()
    {
        var testSubmissionAnswerGroupEntryPart = new SubmissionAnswerGroupEntryPart();

        var testResponses = new List<SubmissionDetailsAnswerResponse> {new(), new(), new()};

        testSubmissionAnswerGroupEntryPart.Responses = testResponses;

        var result = testSubmissionAnswerGroupEntryPart.Responses;

        Assert.That(result, Is.EqualTo(testResponses));
    }
}