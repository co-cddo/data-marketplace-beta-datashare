using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;

[TestFixture]
public class DataShareRequestAnswersSummarySectionTests
{
    [Test]
    public void GivenADataShareRequestAnswersSummarySection_WhenISetOrderWithinSummary_ThenOrderWithinSummaryIsSet(
        [Values(-1, 0, 999)] int testOrderWithinSummary)
    {
        var testDataShareRequestAnswersSummarySection = new DataShareRequestAnswersSummarySection();

        testDataShareRequestAnswersSummarySection.OrderWithinSummary = testOrderWithinSummary;

        var result = testDataShareRequestAnswersSummarySection.OrderWithinSummary;

        Assert.That(result, Is.EqualTo(testOrderWithinSummary));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummarySection_WhenISetSectionHeader_ThenSectionHeaderIsSet(
        [Values("", "  ", "abc")] string testSectionHeader)
    {
        var testDataShareRequestAnswersSummarySection = new DataShareRequestAnswersSummarySection();

        testDataShareRequestAnswersSummarySection.SectionHeader = testSectionHeader;

        var result = testDataShareRequestAnswersSummarySection.SectionHeader;

        Assert.That(result, Is.EqualTo(testSectionHeader));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummarySection_WhenISetAnEmptySetOfSummaryQuestionGroups_ThenSummaryQuestionGroupsIsSet()
    {
        var testDataShareRequestAnswersSummarySection = new DataShareRequestAnswersSummarySection();

        var testSummaryQuestionGroups = new List<DataShareRequestAnswersSummaryQuestionGroup>();

        testDataShareRequestAnswersSummarySection.SummaryQuestionGroups = testSummaryQuestionGroups;

        var result = testDataShareRequestAnswersSummarySection.SummaryQuestionGroups;

        Assert.That(result, Is.EqualTo(testSummaryQuestionGroups));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummarySection_WhenISetSummaryQuestionGroups_ThenSummaryQuestionGroupsIsSet()
    {
        var testDataShareRequestAnswersSummarySection = new DataShareRequestAnswersSummarySection();

        var testSummaryQuestionGroups = new List<DataShareRequestAnswersSummaryQuestionGroup> {new(), new(), new()};

        testDataShareRequestAnswersSummarySection.SummaryQuestionGroups = testSummaryQuestionGroups;

        var result = testDataShareRequestAnswersSummarySection.SummaryQuestionGroups;

        Assert.That(result, Is.EqualTo(testSummaryQuestionGroups));
    }
}