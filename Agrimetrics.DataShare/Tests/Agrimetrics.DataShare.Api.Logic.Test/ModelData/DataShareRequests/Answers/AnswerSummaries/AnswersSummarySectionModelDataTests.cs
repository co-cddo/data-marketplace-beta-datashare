using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.Answers.DsrAnswerSummaries;

[TestFixture]
public class DataShareRequestAnswersSummarySectionModelDataTests
{
    [Test]
    public void GivenADataShareRequestAnswersSummarySectionModelData_WhenISetSectionId_ThenSectionIdIsSet()
    {
        var testDataShareRequestAnswersSummarySectionModelData = new DataShareRequestAnswersSummarySectionModelData();

        var testSectionId = new Guid("513E0C65-D1CB-453A-9F57-D7A850547C8C");

        testDataShareRequestAnswersSummarySectionModelData.DataShareRequestAnswersSummarySection_SectionId = testSectionId;

        var result = testDataShareRequestAnswersSummarySectionModelData.DataShareRequestAnswersSummarySection_SectionId;

        Assert.That(result, Is.EqualTo(testSectionId));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummarySectionModelData_WhenISetOrderWithinSummary_ThenOrderWithinSummaryIsSet(
        [Values(-1, 0, 999)] int testOrderWithinSummary)
    {
        var testDataShareRequestAnswersSummarySectionModelData = new DataShareRequestAnswersSummarySectionModelData();

        testDataShareRequestAnswersSummarySectionModelData.DataShareRequestAnswersSummarySection_OrderWithinSummary = testOrderWithinSummary;

        var result = testDataShareRequestAnswersSummarySectionModelData.DataShareRequestAnswersSummarySection_OrderWithinSummary;

        Assert.That(result, Is.EqualTo(testOrderWithinSummary));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummarySectionModelData_WhenISetSectionHeader_ThenSectionHeaderIsSet(
        [Values("", "  ", "abc")] string testSectionHeader)
    {
        var testDataShareRequestAnswersSummarySectionModelData = new DataShareRequestAnswersSummarySectionModelData();

        testDataShareRequestAnswersSummarySectionModelData.DataShareRequestAnswersSummarySection_SectionHeader = testSectionHeader;

        var result = testDataShareRequestAnswersSummarySectionModelData.DataShareRequestAnswersSummarySection_SectionHeader;

        Assert.That(result, Is.EqualTo(testSectionHeader));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummarySectionModelData_WhenISetAnEmptySetOfQuestionGroups_ThenQuestionGroupsIsSet()
    {
        var testDataShareRequestAnswersSummarySectionModelData = new DataShareRequestAnswersSummarySectionModelData();

        var testQuestionGroups = new List<DataShareRequestAnswersSummaryQuestionGroupModelData>();

        testDataShareRequestAnswersSummarySectionModelData.DataShareRequestAnswersSummarySection_QuestionGroups = testQuestionGroups;

        var result = testDataShareRequestAnswersSummarySectionModelData.DataShareRequestAnswersSummarySection_QuestionGroups;

        Assert.That(result, Is.EqualTo(testQuestionGroups));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummarySectionModelData_WhenISetQuestionGroups_ThenQuestionGroupsIsSet()
    {
        var testDataShareRequestAnswersSummarySectionModelData = new DataShareRequestAnswersSummarySectionModelData();

        var testQuestionGroups = new List<DataShareRequestAnswersSummaryQuestionGroupModelData> {new(), new(), new()};

        testDataShareRequestAnswersSummarySectionModelData.DataShareRequestAnswersSummarySection_QuestionGroups = testQuestionGroups;

        var result = testDataShareRequestAnswersSummarySectionModelData.DataShareRequestAnswersSummarySection_QuestionGroups;

        Assert.That(result, Is.EqualTo(testQuestionGroups));
    }
}