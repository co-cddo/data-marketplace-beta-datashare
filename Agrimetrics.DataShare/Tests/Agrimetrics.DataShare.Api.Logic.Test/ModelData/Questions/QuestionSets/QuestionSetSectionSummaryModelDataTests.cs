using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionSets;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionSets;

[TestFixture]
public class QuestionSetSectionSummaryModelDataTests
{
    [Test]
    public void GivenAQuestionSetSectionSummaryModelData_WhenISetId_ThenIdIsSet()
    {
        var testQuestionSetSectionSummaryModelData = new QuestionSetSectionSummaryModelData();

        var testId = new Guid("584180B9-1B07-41A0-83EA-AC036FE3E2F0");

        testQuestionSetSectionSummaryModelData.QuestionSetSection_Id = testId;

        var result = testQuestionSetSectionSummaryModelData.QuestionSetSection_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenAQuestionSetSectionSummaryModelData_WhenISetNumber_ThenNumberIsSet(
        [Values(-1, 0, 999)] int testNumber)
    {
        var testQuestionSetSectionSummaryModelData = new QuestionSetSectionSummaryModelData();

        testQuestionSetSectionSummaryModelData.QuestionSetSection_Number = testNumber;

        var result = testQuestionSetSectionSummaryModelData.QuestionSetSection_Number;

        Assert.That(result, Is.EqualTo(testNumber));
    }

    [Test]
    public void GivenAQuestionSetSectionSummaryModelData_WhenISetHeader_ThenHeaderIsSet(
        [Values("", "  ", "abc")] string testHeader)
    {
        var testQuestionSetSectionSummaryModelData = new QuestionSetSectionSummaryModelData();

        testQuestionSetSectionSummaryModelData.QuestionSetSection_Header = testHeader;

        var result = testQuestionSetSectionSummaryModelData.QuestionSetSection_Header;

        Assert.That(result, Is.EqualTo(testHeader));
    }

    [Theory]
    public void GivenAQuestionSetSectionSummaryModelData_WhenISetIsComplete_ThenIsCompleteIsSet(
        bool testIsComplete)
    {
        var testQuestionSetSectionSummaryModelData = new QuestionSetSectionSummaryModelData();

        testQuestionSetSectionSummaryModelData.QuestionSetSection_IsComplete = testIsComplete;

        var result = testQuestionSetSectionSummaryModelData.QuestionSetSection_IsComplete;

        Assert.That(result, Is.EqualTo(testIsComplete));
    }

    [Test]
    public void GivenAQuestionSetSectionSummaryModelData_WhenISetAnEmptySetOfQuestionSummaries_ThenQuestionSummariesIsSet()
    {
        var testQuestionSetSectionSummaryModelData = new QuestionSetSectionSummaryModelData();

        var testQuestionSummaries = new List<QuestionSummaryModelData>();

        testQuestionSetSectionSummaryModelData.QuestionSetSection_QuestionSummaries = testQuestionSummaries;

        var result = testQuestionSetSectionSummaryModelData.QuestionSetSection_QuestionSummaries;

        Assert.That(result, Is.EqualTo(testQuestionSummaries));
    }

    [Test]
    public void GivenAQuestionSetSectionSummaryModelData_WhenISetQuestionSummaries_ThenQuestionSummariesIsSet()
    {
        var testQuestionSetSectionSummaryModelData = new QuestionSetSectionSummaryModelData();

        var testQuestionSummaries = new List<QuestionSummaryModelData> {new(), new(), new()};

        testQuestionSetSectionSummaryModelData.QuestionSetSection_QuestionSummaries = testQuestionSummaries;

        var result = testQuestionSetSectionSummaryModelData.QuestionSetSection_QuestionSummaries;

        Assert.That(result, Is.EqualTo(testQuestionSummaries));
    }
}

