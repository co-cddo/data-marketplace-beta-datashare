using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.Answers.DsrAnswerSummaries;

[TestFixture]
public class DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeFormModelDataTests
{
    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeFormModelData_WhenIGetResponseInputType_ThenResponseInputTypeIsFreeForm()
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeFormModelData = new DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeFormModelData();

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeFormModelData.DataShareRequestAnswersSummaryQuestionPartAnswerResponseItem_ResponseInputType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseInputType.FreeForm));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeFormModelData_WhenISetAnswerValue_ThenAnswerValueIsSet(
        [Values("", "  ", "abc")] string testAnswerValue)
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeFormModelData = new DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeFormModelData();

        testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeFormModelData.DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm_AnswerValue = testAnswerValue;

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeFormModelData.DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm_AnswerValue;

        Assert.That(result, Is.EqualTo(testAnswerValue));
    }

    [Theory]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeFormModelData_WhenISetValueEntryDeclined_ThenValueEntryDeclinedIsSet(
        bool testValueEntryDeclined)
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeFormModelData = new DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeFormModelData();

        testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeFormModelData.DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm_ValueEntryDeclined = testValueEntryDeclined;

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeFormModelData.DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm_ValueEntryDeclined;

        Assert.That(result, Is.EqualTo(testValueEntryDeclined));
    }
}