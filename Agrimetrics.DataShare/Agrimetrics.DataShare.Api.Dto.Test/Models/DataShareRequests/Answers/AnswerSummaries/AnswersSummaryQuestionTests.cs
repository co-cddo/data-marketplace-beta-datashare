using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;

[TestFixture]
public class DataShareRequestAnswersSummaryQuestionTests
{
    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestion_WhenISetQuestionId_ThenQuestionIdIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestion = new DataShareRequestAnswersSummaryQuestion();

        var testQuestionId = new Guid("B518EBE0-B174-4775-8860-4CCD2B649389");

        testDataShareRequestAnswersSummaryQuestion.QuestionId = testQuestionId;

        var result = testDataShareRequestAnswersSummaryQuestion.QuestionId;

        Assert.That(result, Is.EqualTo(testQuestionId));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestion_WhenISetQuestionHeader_ThenQuestionHeaderIsSet(
        [Values("", "  ", "abc")] string testQuestionHeader)
    {
        var testDataShareRequestAnswersSummaryQuestion = new DataShareRequestAnswersSummaryQuestion();

        testDataShareRequestAnswersSummaryQuestion.QuestionHeader = testQuestionHeader;

        var result = testDataShareRequestAnswersSummaryQuestion.QuestionHeader;

        Assert.That(result, Is.EqualTo(testQuestionHeader));
    }

    [Theory]
    public void GivenADataShareRequestAnswersSummaryQuestion_WhenISetQuestionIsApplicable_ThenQuestionIsApplicableIsSet(
        bool testQuestionIsApplicable)
    {
        var testDataShareRequestAnswersSummaryQuestion = new DataShareRequestAnswersSummaryQuestion();

        testDataShareRequestAnswersSummaryQuestion.QuestionIsApplicable = testQuestionIsApplicable;

        var result = testDataShareRequestAnswersSummaryQuestion.QuestionIsApplicable;

        Assert.That(result, Is.EqualTo(testQuestionIsApplicable));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestion_WhenISetAnEmptySetOfSummaryQuestionParts_ThenSummaryQuestionPartsIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestion = new DataShareRequestAnswersSummaryQuestion();

        var testSummaryQuestionParts = new List<DataShareRequestAnswersSummaryQuestionPart>();

        testDataShareRequestAnswersSummaryQuestion.SummaryQuestionParts = testSummaryQuestionParts;

        var result = testDataShareRequestAnswersSummaryQuestion.SummaryQuestionParts;

        Assert.That(result, Is.EqualTo(testSummaryQuestionParts));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestion_WhenISetSummaryQuestionParts_ThenSummaryQuestionPartsIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestion = new DataShareRequestAnswersSummaryQuestion();

        var testSummaryQuestionParts = new List<DataShareRequestAnswersSummaryQuestionPart>{ new(), new(), new() };

        testDataShareRequestAnswersSummaryQuestion.SummaryQuestionParts = testSummaryQuestionParts;

        var result = testDataShareRequestAnswersSummaryQuestion.SummaryQuestionParts;

        Assert.That(result, Is.EqualTo(testSummaryQuestionParts));
    }
}