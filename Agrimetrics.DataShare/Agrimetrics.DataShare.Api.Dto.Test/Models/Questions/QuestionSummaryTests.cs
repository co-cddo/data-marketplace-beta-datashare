using Agrimetrics.DataShare.Api.Dto.Models.Questions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions;

[TestFixture]
public class QuestionSummaryTests
{
    [Test]
    public void GivenAQuestionSummary_WhenISetQuestionId_ThenQuestionIdIsSet()
    {
        var testQuestionSummary = new QuestionSummary();

        var testQuestionId = new Guid("66F8D351-54D4-4807-A42B-BEAFDA8E3C3A");

        testQuestionSummary.QuestionId = testQuestionId;

        var result = testQuestionSummary.QuestionId;

        Assert.That(result, Is.EqualTo(testQuestionId));
    }

    [Test]
    public void GivenAQuestionSummary_WhenISetQuestionOrderWithinQuestionSetSection_ThenQuestionOrderWithinQuestionSetSectionIsSet(
        [Values(-1, 0, 999)] int testQuestionOrderWithinQuestionSetSection)
    {
        var testQuestionSummary = new QuestionSummary();

        testQuestionSummary.QuestionOrderWithinQuestionSetSection = testQuestionOrderWithinQuestionSetSection;

        var result = testQuestionSummary.QuestionOrderWithinQuestionSetSection;

        Assert.That(result, Is.EqualTo(testQuestionOrderWithinQuestionSetSection));
    }

    [Test]
    public void GivenAQuestionSummary_WhenISetQuestionHeader_ThenQuestionHeaderIsSet(
        [Values("", "  ", "abc")] string testQuestionHeader)
    {
        var testQuestionSummary = new QuestionSummary();

        testQuestionSummary.QuestionHeader = testQuestionHeader;

        var result = testQuestionSummary.QuestionHeader;

        Assert.That(result, Is.EqualTo(testQuestionHeader));
    }

    [Theory]
    public void GivenAQuestionSummary_WhenISetQuestionStatus_ThenQuestionStatusIsSet(
        QuestionStatus testQuestionStatus)
    {
        var testQuestionSummary = new QuestionSummary();

        testQuestionSummary.QuestionStatus = testQuestionStatus;

        var result = testQuestionSummary.QuestionStatus;

        Assert.That(result, Is.EqualTo(testQuestionStatus));
    }

    [Theory]
    public void GivenAQuestionSummary_WhenISetQuestionCanBeAnswered_ThenQuestionCanBeAnsweredIsSet(
        bool testQuestionCanBeAnswered)
    {
        var testQuestionSummary = new QuestionSummary();

        testQuestionSummary.QuestionCanBeAnswered = testQuestionCanBeAnswered;

        var result = testQuestionSummary.QuestionCanBeAnswered;

        Assert.That(result, Is.EqualTo(testQuestionCanBeAnswered));
    }
}