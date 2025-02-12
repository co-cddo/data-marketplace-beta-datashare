using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;

[TestFixture]
public class DataShareRequestAnswersSummaryQuestionGroupTests
{
    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionGroup_WhenISetOrderWithinSection_ThenOrderWithinSectionIsSet(
        [Values(-1, 0, 999)] int testOrderWithinSection)
    {
        var testDataShareRequestAnswersSummaryQuestionGroup = new DataShareRequestAnswersSummaryQuestionGroup();

        testDataShareRequestAnswersSummaryQuestionGroup.OrderWithinSection = testOrderWithinSection;

        var result = testDataShareRequestAnswersSummaryQuestionGroup.OrderWithinSection;

        Assert.That(result, Is.EqualTo(testOrderWithinSection));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionGroup_WhenISetMainQuestionSummary_ThenMainQuestionSummaryIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionGroup = new DataShareRequestAnswersSummaryQuestionGroup();

        var testMainQuestionSummary = new DataShareRequestAnswersSummaryQuestion();

        testDataShareRequestAnswersSummaryQuestionGroup.MainQuestionSummary = testMainQuestionSummary;

        var result = testDataShareRequestAnswersSummaryQuestionGroup.MainQuestionSummary;

        Assert.That(result, Is.SameAs(testMainQuestionSummary));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionGroup_WhenISetAnEmptySetOfBackingQuestionSummaries_ThenBackingQuestionSummariesIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionGroup = new DataShareRequestAnswersSummaryQuestionGroup();

        var testBackingQuestionSummaries = new List<DataShareRequestAnswersSummaryQuestion>();

        testDataShareRequestAnswersSummaryQuestionGroup.BackingQuestionSummaries = testBackingQuestionSummaries;

        var result = testDataShareRequestAnswersSummaryQuestionGroup.BackingQuestionSummaries;

        Assert.That(result, Is.EqualTo(testBackingQuestionSummaries));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionGroup_WhenISetBackingQuestionSummaries_ThenBackingQuestionSummariesIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionGroup = new DataShareRequestAnswersSummaryQuestionGroup();

        var testBackingQuestionSummaries = new List<DataShareRequestAnswersSummaryQuestion>{ new(), new(), new() };

        testDataShareRequestAnswersSummaryQuestionGroup.BackingQuestionSummaries = testBackingQuestionSummaries;

        var result = testDataShareRequestAnswersSummaryQuestionGroup.BackingQuestionSummaries;

        Assert.That(result, Is.EqualTo(testBackingQuestionSummaries));
    }
}