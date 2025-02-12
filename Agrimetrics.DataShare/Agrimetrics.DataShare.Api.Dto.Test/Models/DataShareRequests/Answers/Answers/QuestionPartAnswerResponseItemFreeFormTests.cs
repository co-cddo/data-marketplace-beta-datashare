using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.Answers;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Answers.Answers;

[TestFixture]
public class QuestionPartAnswerResponseItemFreeFormTests
{
    [Theory]
    public void GivenAQuestionPartAnswerResponseItemFreeForm_WhenIGetInputType_ThenInputTypeIsFreeForm(
        QuestionPartResponseInputType testInputType)
    {
        var testQuestionPartAnswerResponseItemFreeForm = new QuestionPartAnswerResponseItemFreeForm();

        var result = testQuestionPartAnswerResponseItemFreeForm.InputType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseInputType.FreeForm));
    }

    [Theory]
    public void GivenAQuestionPartAnswerResponseItemFreeForm_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testQuestionPartAnswerResponseItemFreeForm = new QuestionPartAnswerResponseItemFreeForm();

        testQuestionPartAnswerResponseItemFreeForm.InputType = testInputType;

        var result = testQuestionPartAnswerResponseItemFreeForm.InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }

    [Test]
    public void GivenAQuestionPartAnswerResponseItemFreeForm_WhenISetEnteredValue_ThenInputTypeIsSet(
        [Values("", "  ", "abc")] string testEnteredValue)
    {
        var testQuestionPartAnswerResponseItemFreeForm = new QuestionPartAnswerResponseItemFreeForm();

        testQuestionPartAnswerResponseItemFreeForm.EnteredValue = testEnteredValue;

        var result = testQuestionPartAnswerResponseItemFreeForm.EnteredValue;

        Assert.That(result, Is.EqualTo(testEnteredValue));
    }

    [Theory]
    public void GivenAQuestionPartAnswerResponseItemFreeForm_WhenISetValueEntryDeclined_ThenValueEntryDeclinedIsSet(
        bool testValueEntryDeclined)
    {
        var testQuestionPartAnswerResponseItemFreeForm = new QuestionPartAnswerResponseItemFreeForm();

        testQuestionPartAnswerResponseItemFreeForm.ValueEntryDeclined = testValueEntryDeclined;

        var result = testQuestionPartAnswerResponseItemFreeForm.ValueEntryDeclined;

        Assert.That(result, Is.EqualTo(testValueEntryDeclined));
    }
}