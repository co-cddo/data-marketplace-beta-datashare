using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Submissions;

[TestFixture]
public class SubmissionDetailsAnswerPartModelDataTests
{
    [Test]
    public void GivenASubmissionDetailsAnswerPartModelData_WhenISetId_ThenIdIsSet()
    {
        var testSubmissionDetailsAnswerPartModelData = new SubmissionDetailsAnswerPartModelData();

        var testId = new Guid("FE810C49-B80F-4EA0-9A46-D3BBB5BF49F6");

        testSubmissionDetailsAnswerPartModelData.SubmissionDetailsAnswerPart_Id = testId;

        var result = testSubmissionDetailsAnswerPartModelData.SubmissionDetailsAnswerPart_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerPartModelData_WhenISetOrderWithinAnswer_ThenOrderWithinAnswerIsSet(
        [Values(-1, 0, 999)] int testOrderWithinAnswer)
    {
        var testSubmissionDetailsAnswerPartModelData = new SubmissionDetailsAnswerPartModelData();

        testSubmissionDetailsAnswerPartModelData.SubmissionDetailsAnswerPart_OrderWithinAnswer = testOrderWithinAnswer;

        var result = testSubmissionDetailsAnswerPartModelData.SubmissionDetailsAnswerPart_OrderWithinAnswer;

        Assert.That(result, Is.EqualTo(testOrderWithinAnswer));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerPartModelData_WhenISetQuestionPartText_ThenQuestionPartTextIsSet(
        [Values("", "  ", "abc")] string testQuestionPartText)
    {
        var testSubmissionDetailsAnswerPartModelData = new SubmissionDetailsAnswerPartModelData();

        testSubmissionDetailsAnswerPartModelData.SubmissionDetailsAnswerPart_QuestionPartText = testQuestionPartText;

        var result = testSubmissionDetailsAnswerPartModelData.SubmissionDetailsAnswerPart_QuestionPartText;

        Assert.That(result, Is.EqualTo(testQuestionPartText));
    }

    [Theory]
    public void GivenASubmissionDetailsAnswerPartModelData_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testSubmissionDetailsAnswerPartModelData = new SubmissionDetailsAnswerPartModelData();

        testSubmissionDetailsAnswerPartModelData.SubmissionDetailsAnswerPart_InputType = testInputType;

        var result = testSubmissionDetailsAnswerPartModelData.SubmissionDetailsAnswerPart_InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }

    [Theory]
    public void GivenASubmissionDetailsAnswerPartModelData_WhenISetFormatType_ThenFormatTypeIsSet(
        QuestionPartResponseFormatType testFormatType)
    {
        var testSubmissionDetailsAnswerPartModelData = new SubmissionDetailsAnswerPartModelData();

        testSubmissionDetailsAnswerPartModelData.SubmissionDetailsAnswerPart_FormatType = testFormatType;

        var result = testSubmissionDetailsAnswerPartModelData.SubmissionDetailsAnswerPart_FormatType;

        Assert.That(result, Is.EqualTo(testFormatType));
    }

    [Theory]
    public void GivenASubmissionDetailsAnswerPartModelData_WhenISetMultipleResponsesAllowed_ThenMultipleResponsesAllowedIsSet(
        bool testMultipleResponsesAllowed)
    {
        var testSubmissionDetailsAnswerPartModelData = new SubmissionDetailsAnswerPartModelData();

        testSubmissionDetailsAnswerPartModelData.SubmissionDetailsAnswerPart_MultipleResponsesAllowed = testMultipleResponsesAllowed;

        var result = testSubmissionDetailsAnswerPartModelData.SubmissionDetailsAnswerPart_MultipleResponsesAllowed;

        Assert.That(result, Is.EqualTo(testMultipleResponsesAllowed));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerPartModelData_WhenISetCollectionDescriptionIfMultipleResponsesAllowed_ThenCollectionDescriptionIfMultipleResponsesAllowedIsSet(
        [Values(null, "", "  ", "abc")] string? testCollectionDescriptionIfMultipleResponsesAllowed)
    {
        var testSubmissionDetailsAnswerPartModelData = new SubmissionDetailsAnswerPartModelData();

        testSubmissionDetailsAnswerPartModelData.SubmissionDetailsAnswerPart_CollectionDescriptionIfMultipleResponsesAllowed = testCollectionDescriptionIfMultipleResponsesAllowed;

        var result = testSubmissionDetailsAnswerPartModelData.SubmissionDetailsAnswerPart_CollectionDescriptionIfMultipleResponsesAllowed;

        Assert.That(result, Is.EqualTo(testCollectionDescriptionIfMultipleResponsesAllowed));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerPartModelData_WhenISetAnEmptySetOfResponses_ThenResponsesIsSet()
    {
        var testSubmissionDetailsAnswerPartModelData = new SubmissionDetailsAnswerPartModelData();

        var testResponses = new List<SubmissionDetailsAnswerPartResponseModelData>();

        testSubmissionDetailsAnswerPartModelData.SubmissionDetailsAnswerPart_Responses = testResponses;

        var result = testSubmissionDetailsAnswerPartModelData.SubmissionDetailsAnswerPart_Responses;

        Assert.That(result, Is.EqualTo(testResponses));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerPartModelData_WhenISetResponses_ThenResponsesIsSet()
    {
        var testSubmissionDetailsAnswerPartModelData = new SubmissionDetailsAnswerPartModelData();

        var testResponses = new List<SubmissionDetailsAnswerPartResponseModelData> {new(), new(), new()};

        testSubmissionDetailsAnswerPartModelData.SubmissionDetailsAnswerPart_Responses = testResponses;

        var result = testSubmissionDetailsAnswerPartModelData.SubmissionDetailsAnswerPart_Responses;

        Assert.That(result, Is.EqualTo(testResponses));
    }
}