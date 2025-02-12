using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.Answers.DsrAnswerSummaries;

[TestFixture]
public class DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelDataTests
{
    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData_WhenISetItemId_ThenItemIdIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData = new DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData();

        var testItemId = new Guid("926B6116-A0F7-4CEA-9B39-4A9C92399DD8");

        testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData.DataShareRequestAnswersSummaryQuestionPartAnswerItemOptionSelectionItem_ItemId = testItemId;

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData.DataShareRequestAnswersSummaryQuestionPartAnswerItemOptionSelectionItem_ItemId;

        Assert.That(result, Is.EqualTo(testItemId));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData_WhenISetOrderWithinAnswerPartResponse_ThenOrderWithinAnswerPartResponseIsSet(
        [Values(-1, 0, 999)] int testOrderWithinAnswerPartResponse)
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData = new DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData();

        testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData.DataShareRequestAnswersSummaryQuestionPartAnswerItemOptionSelectionItem_OrderWithinAnswerPartResponse = testOrderWithinAnswerPartResponse;

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData.DataShareRequestAnswersSummaryQuestionPartAnswerItemOptionSelectionItem_OrderWithinAnswerPartResponse;

        Assert.That(result, Is.EqualTo(testOrderWithinAnswerPartResponse));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData_WhenISetSelectionOptionText_ThenSelectionOptionTextIsSet(
        [Values("", "  ", "abc")] string testSelectionOptionText)
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData = new DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData();

        testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData.DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption_SelectionOptionText = testSelectionOptionText;

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData.DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption_SelectionOptionText;

        Assert.That(result, Is.EqualTo(testSelectionOptionText));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData_WhenISetSupplementaryAnswerText_ThenSupplementaryAnswerTextIsSet(
        [Values(null, "", "  ", "abc")] string testSupplementaryAnswerText)
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData = new DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData();

        testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData.DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption_SupplementaryAnswerText = testSupplementaryAnswerText;

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData.DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption_SupplementaryAnswerText;

        Assert.That(result, Is.EqualTo(testSupplementaryAnswerText));
    }
}