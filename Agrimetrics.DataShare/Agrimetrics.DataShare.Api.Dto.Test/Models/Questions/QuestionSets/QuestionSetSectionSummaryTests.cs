using Agrimetrics.DataShare.Api.Dto.Models.Questions;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionSets;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionSets;

[TestFixture]
public class QuestionSetSectionSummaryTests
{
    [Test]
    public void GivenAQuestionSetSectionSummary_WhenISetId_ThenIdIsSet()
    {
        var testQuestionSetSectionSummary = new QuestionSetSectionSummary();

        var testId = new Guid("DD73BCB4-9812-4D09-AE7D-4F8F07DAFAE2");

        testQuestionSetSectionSummary.Id = testId;

        var result = testQuestionSetSectionSummary.Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenAQuestionSetSectionSummary_WhenISetSectionNumber_ThenSectionNumberIsSet(
        [Values(-1, 0, 999)] int testSectionNumber)
    {
        var testQuestionSetSectionSummary = new QuestionSetSectionSummary();

        testQuestionSetSectionSummary.SectionNumber = testSectionNumber;

        var result = testQuestionSetSectionSummary.SectionNumber;

        Assert.That(result, Is.EqualTo(testSectionNumber));
    }

    [Test]
    public void GivenAQuestionSetSectionSummary_WhenISetSectionHeader_ThenSectionHeaderIsSet(
        [Values("", "  ", "abc")] string testSectionHeader)
    {
        var testQuestionSetSectionSummary = new QuestionSetSectionSummary();

        testQuestionSetSectionSummary.SectionHeader = testSectionHeader;

        var result = testQuestionSetSectionSummary.SectionHeader;

        Assert.That(result, Is.EqualTo(testSectionHeader));
    }

    [Theory]
    public void GivenAQuestionSetSectionSummary_WhenISetSectionIsComplete_ThenSectionIsCompleteIsSet(
        bool testSectionIsComplete)
    {
        var testQuestionSetSectionSummary = new QuestionSetSectionSummary();

        testQuestionSetSectionSummary.SectionIsComplete = testSectionIsComplete;

        var result = testQuestionSetSectionSummary.SectionIsComplete;

        Assert.That(result, Is.EqualTo(testSectionIsComplete));
    }

    [Test]
    public void GivenAQuestionSetSectionSummary_WhenISetAnEmptySetOfQuestionSummaries_ThenQuestionSummariesIsSet()
    {
        var testQuestionSetSectionSummary = new QuestionSetSectionSummary();

        var testQuestionSummaries = new List<QuestionSummary>();

        testQuestionSetSectionSummary.QuestionSummaries = testQuestionSummaries;

        var result = testQuestionSetSectionSummary.QuestionSummaries;

        Assert.That(result, Is.EqualTo(testQuestionSummaries));
    }

    [Test]
    public void GivenAQuestionSetSectionSummary_WhenISetQuestionSummaries_ThenQuestionSummariesIsSet()
    {
        var testQuestionSetSectionSummary = new QuestionSetSectionSummary();

        var testQuestionSummaries = new List<QuestionSummary> {new(), new(), new()};

        testQuestionSetSectionSummary.QuestionSummaries = testQuestionSummaries;

        var result = testQuestionSetSectionSummary.QuestionSummaries;

        Assert.That(result, Is.EqualTo(testQuestionSummaries));
    }
}