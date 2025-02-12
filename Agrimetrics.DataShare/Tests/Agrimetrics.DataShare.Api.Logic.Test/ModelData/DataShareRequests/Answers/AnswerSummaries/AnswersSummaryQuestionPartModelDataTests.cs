using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.Answers.DsrAnswerSummaries;

[TestFixture]
public class DataShareRequestAnswersSummaryQuestionPartModelDataTests
{
    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartModelData_WhenISetQuestionPartId_ThenQuestionPartIdIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionPartModelData = new DataShareRequestAnswersSummaryQuestionPartModelData();

        var testQuestionPartId = new Guid("BE4CC5A2-A4C6-46A2-A87B-6FB2AFF6C4C6");

        testDataShareRequestAnswersSummaryQuestionPartModelData.DataShareRequestAnswersSummaryQuestionPart_QuestionPartId = testQuestionPartId;

        var result = testDataShareRequestAnswersSummaryQuestionPartModelData.DataShareRequestAnswersSummaryQuestionPart_QuestionPartId;

        Assert.That(result, Is.EqualTo(testQuestionPartId));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartModelData_WhenISetAnswerPartId_ThenAnswerPartIdIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionPartModelData = new DataShareRequestAnswersSummaryQuestionPartModelData();

        var testAnswerPartId = new Guid("BE4CC5A2-A4C6-46A2-A87B-6FB2AFF6C4C6");

        testDataShareRequestAnswersSummaryQuestionPartModelData.DataShareRequestAnswersSummaryQuestionPart_AnswerPartId = testAnswerPartId;

        var result = testDataShareRequestAnswersSummaryQuestionPartModelData.DataShareRequestAnswersSummaryQuestionPart_AnswerPartId;

        Assert.That(result, Is.EqualTo(testAnswerPartId));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartModelData_WhenISetOrderWithinQuestion_ThenOrderWithinQuestionIsSet(
        [Values(-1, 0, 999)] int testOrderWithinQuestion)
    {
        var testDataShareRequestAnswersSummaryQuestionPartModelData = new DataShareRequestAnswersSummaryQuestionPartModelData();

        testDataShareRequestAnswersSummaryQuestionPartModelData.DataShareRequestAnswersSummaryQuestionPart_OrderWithinQuestion = testOrderWithinQuestion;

        var result = testDataShareRequestAnswersSummaryQuestionPartModelData.DataShareRequestAnswersSummaryQuestionPart_OrderWithinQuestion;

        Assert.That(result, Is.EqualTo(testOrderWithinQuestion));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartModelData_WhenISetQuestionPartText_ThenQuestionPartTextIsSet(
        [Values("", "  ", "abc")] string testQuestionPartText)
    {
        var testDataShareRequestAnswersSummaryQuestionPartModelData = new DataShareRequestAnswersSummaryQuestionPartModelData();

        testDataShareRequestAnswersSummaryQuestionPartModelData.DataShareRequestAnswersSummaryQuestionPart_QuestionPartText = testQuestionPartText;

        var result = testDataShareRequestAnswersSummaryQuestionPartModelData.DataShareRequestAnswersSummaryQuestionPart_QuestionPartText;

        Assert.That(result, Is.EqualTo(testQuestionPartText));
    }

    [Theory]
    public void GivenADataShareRequestAnswersSummaryQuestionPartModelData_WhenISetMultipleResponsesAllowed_ThenMultipleResponsesAllowedIsSet(
        bool testMultipleResponsesAllowed)
    {
        var testDataShareRequestAnswersSummaryQuestionPartModelData = new DataShareRequestAnswersSummaryQuestionPartModelData();

        testDataShareRequestAnswersSummaryQuestionPartModelData.DataShareRequestAnswersSummaryQuestionPart_MultipleResponsesAllowed = testMultipleResponsesAllowed;

        var result = testDataShareRequestAnswersSummaryQuestionPartModelData.DataShareRequestAnswersSummaryQuestionPart_MultipleResponsesAllowed;

        Assert.That(result, Is.EqualTo(testMultipleResponsesAllowed));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartModelData_WhenISetMultipleResponsesCollectionDescriptionIfMultipleResponsesAllowed_ThenMultipleResponsesCollectionDescriptionIfMultipleResponsesAllowedIsSet(
        [Values("", "  ", "abc")] string testMultipleResponsesCollectionDescriptionIfMultipleResponsesAllowed)
    {
        var testDataShareRequestAnswersSummaryQuestionPartModelData = new DataShareRequestAnswersSummaryQuestionPartModelData();

        testDataShareRequestAnswersSummaryQuestionPartModelData.DataShareRequestAnswersSummaryQuestionPart_MultipleResponsesCollectionDescriptionIfMultipleResponsesAllowed = testMultipleResponsesCollectionDescriptionIfMultipleResponsesAllowed;

        var result = testDataShareRequestAnswersSummaryQuestionPartModelData.DataShareRequestAnswersSummaryQuestionPart_MultipleResponsesCollectionDescriptionIfMultipleResponsesAllowed;

        Assert.That(result, Is.EqualTo(testMultipleResponsesCollectionDescriptionIfMultipleResponsesAllowed));
    }

    [Theory]
    public void GivenADataShareRequestAnswersSummaryQuestionPartModelData_WhenISetResponseInputType_ThenResponseInputTypeIsSet(
        QuestionPartResponseInputType testResponseInputType)
    {
        var testDataShareRequestAnswersSummaryQuestionPartModelData = new DataShareRequestAnswersSummaryQuestionPartModelData();

        testDataShareRequestAnswersSummaryQuestionPartModelData.DataShareRequestAnswersSummaryQuestionPart_ResponseInputType = testResponseInputType;

        var result = testDataShareRequestAnswersSummaryQuestionPartModelData.DataShareRequestAnswersSummaryQuestionPart_ResponseInputType;

        Assert.That(result, Is.EqualTo(testResponseInputType));
    }

    [Theory]
    public void GivenADataShareRequestAnswersSummaryQuestionPartModelData_WhenISetResponseFormatType_ThenResponseFormatTypeIsSet(
        QuestionPartResponseFormatType testResponseFormatType)
    {
        var testDataShareRequestAnswersSummaryQuestionPartModelData = new DataShareRequestAnswersSummaryQuestionPartModelData();

        testDataShareRequestAnswersSummaryQuestionPartModelData.DataShareRequestAnswersSummaryQuestionPart_ResponseFormatType = testResponseFormatType;

        var result = testDataShareRequestAnswersSummaryQuestionPartModelData.DataShareRequestAnswersSummaryQuestionPart_ResponseFormatType;

        Assert.That(result, Is.EqualTo(testResponseFormatType));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartModelData_WhenISetAnEmptySetOfResponses_ThenResponsesIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionPartModelData = new DataShareRequestAnswersSummaryQuestionPartModelData();

        var testResponses = new List<DataShareRequestAnswersSummaryQuestionPartResponseModelData>();

        testDataShareRequestAnswersSummaryQuestionPartModelData.DataShareRequestAnswersSummaryQuestionPart_Responses = testResponses;

        var result = testDataShareRequestAnswersSummaryQuestionPartModelData.DataShareRequestAnswersSummaryQuestionPart_Responses;

        Assert.That(result, Is.EqualTo(testResponses));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartModelData_WhenISetResponses_ThenResponsesIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionPartModelData = new DataShareRequestAnswersSummaryQuestionPartModelData();

        var testResponses = new List<DataShareRequestAnswersSummaryQuestionPartResponseModelData> {new(), new(), new()};

        testDataShareRequestAnswersSummaryQuestionPartModelData.DataShareRequestAnswersSummaryQuestionPart_Responses = testResponses;

        var result = testDataShareRequestAnswersSummaryQuestionPartModelData.DataShareRequestAnswersSummaryQuestionPart_Responses;

        Assert.That(result, Is.EqualTo(testResponses));
    }
}