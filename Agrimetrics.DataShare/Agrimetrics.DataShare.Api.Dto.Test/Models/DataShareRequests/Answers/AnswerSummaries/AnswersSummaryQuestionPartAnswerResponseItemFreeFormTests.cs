using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;

[TestFixture]
public class DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeFormTests
{
    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm_WhenIGetInputType_ThenInputTypeIsFreeForm()
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm = new DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm();

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm.InputType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseInputType.FreeForm));
    }

    [Theory]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm = new DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm();

        testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm.InputType = testInputType;

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm.InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm_WhenISetAnswerValue_ThenAnswerValueIsSet(
        [Values("", "  ", "abc")] string testAnswerValue)
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm = new DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm();

        testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm.AnswerValue = testAnswerValue;

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm.AnswerValue;

        Assert.That(result, Is.EqualTo(testAnswerValue));
    }

    [Theory]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm_WhenISetValueEntryDeclined_ThenValueEntryDeclinedIsSet(
        bool testValueEntryDeclined)
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm = new DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm();

        testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm.ValueEntryDeclined = testValueEntryDeclined;

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm.ValueEntryDeclined;

        Assert.That(result, Is.EqualTo(testValueEntryDeclined));
    }
}