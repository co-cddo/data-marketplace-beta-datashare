using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionSets;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionSets;

[TestFixture]
public class QuestionSetSummaryModelDataTests
{
    [Test]
    public void GivenAQuestionSetSummaryModelData_WhenISetId_ThenIdIsSet()
    {
        var testQuestionSetSummaryModelData = new QuestionSetSummaryModelData();

        var testId = new Guid("C7A3C090-12D2-4046-93E2-E6AD6E17F8F6");

        testQuestionSetSummaryModelData.QuestionSet_Id = testId;

        var result = testQuestionSetSummaryModelData.QuestionSet_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Theory]
    public void GivenAQuestionSetSummaryModelData_WhenISetAnswersSectionComplete_ThenAnswersSectionCompleteIsSet(
        bool testAnswersSectionComplete)
    {
        var testQuestionSetSummaryModelData = new QuestionSetSummaryModelData();

        testQuestionSetSummaryModelData.QuestionSet_AnswersSectionComplete = testAnswersSectionComplete;

        var result = testQuestionSetSummaryModelData.QuestionSet_AnswersSectionComplete;

        Assert.That(result, Is.EqualTo(testAnswersSectionComplete));
    }

    [Test]
    public void GivenAQuestionSetSummaryModelData_WhenISetAnEmptySetOfSectionSummaries_ThenSectionSummariesIsSet()
    {
        var testQuestionSetSummaryModelData = new QuestionSetSummaryModelData();

        var testSectionSummaries = new List<QuestionSetSectionSummaryModelData>();

        testQuestionSetSummaryModelData.QuestionSet_SectionSummaries = testSectionSummaries;

        var result = testQuestionSetSummaryModelData.QuestionSet_SectionSummaries;

        Assert.That(result, Is.EqualTo(testSectionSummaries));
    }

    [Test]
    public void GivenAQuestionSetSummaryModelData_WhenISetSectionSummaries_ThenSectionSummariesIsSet()
    {
        var testQuestionSetSummaryModelData = new QuestionSetSummaryModelData();

        var testSectionSummaries = new List<QuestionSetSectionSummaryModelData> {new(), new(), new()};

        testQuestionSetSummaryModelData.QuestionSet_SectionSummaries = testSectionSummaries;

        var result = testQuestionSetSummaryModelData.QuestionSet_SectionSummaries;

        Assert.That(result, Is.EqualTo(testSectionSummaries));
    }
}