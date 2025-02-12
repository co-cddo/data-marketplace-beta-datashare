using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;

[TestFixture]
public class DataShareRequestQuestionAnswerPartResponseFreeFormTests
{
    [Test]
    public void GivenADataShareRequestQuestionAnswerPartResponseFreeForm_WhenIGetInputType_ThenInputTypeIsFreeForm()
    {
        var testDataShareRequestQuestionAnswerPartResponseFreeForm = new DataShareRequestQuestionAnswerPartResponseFreeForm();

        var result = testDataShareRequestQuestionAnswerPartResponseFreeForm.InputType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseInputType.FreeForm));
    }

    [Theory]
    public void GivenADataShareRequestQuestionAnswerPartResponseFreeForm_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testDataShareRequestQuestionAnswerPartResponseFreeForm = new DataShareRequestQuestionAnswerPartResponseFreeForm();

        testDataShareRequestQuestionAnswerPartResponseFreeForm.InputType = testInputType;

        var result = testDataShareRequestQuestionAnswerPartResponseFreeForm.InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartResponseFreeForm_WhenISetEnteredValue_ThenEnteredValueIsSet(
        [Values("", "  ", "abc")] string testEnteredValue)
    {
        var testDataShareRequestQuestionAnswerPartResponseFreeForm = new DataShareRequestQuestionAnswerPartResponseFreeForm();

        testDataShareRequestQuestionAnswerPartResponseFreeForm.EnteredValue = testEnteredValue;

        var result = testDataShareRequestQuestionAnswerPartResponseFreeForm.EnteredValue;

        Assert.That(result, Is.EqualTo(testEnteredValue));
    }

    [Theory]
    public void GivenADataShareRequestQuestionAnswerPartResponseFreeForm_WhenISetValueEntryDeclined_ThenValueEntryDeclinedIsSet(
        bool testValueEntryDeclined)
    {
        var testDataShareRequestQuestionAnswerPartResponseFreeForm = new DataShareRequestQuestionAnswerPartResponseFreeForm();

        testDataShareRequestQuestionAnswerPartResponseFreeForm.ValueEntryDeclined = testValueEntryDeclined;

        var result = testDataShareRequestQuestionAnswerPartResponseFreeForm.ValueEntryDeclined;

        Assert.That(result, Is.EqualTo(testValueEntryDeclined));
    }
}