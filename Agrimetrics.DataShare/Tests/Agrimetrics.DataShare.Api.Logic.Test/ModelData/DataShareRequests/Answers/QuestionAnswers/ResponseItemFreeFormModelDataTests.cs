using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.Answers.DsrRequestQuestionAnswers;

[TestFixture]
public class QuestionPartAnswerResponseItemFreeFormModelDataTests
{
    [Test]
    public void GivenAQuestionPartAnswerResponseItemFreeFormModelData_WhenIGetInputType_ThenInputTypeIsFreeForm()
    {
        var testQuestionPartAnswerResponseItemFreeFormModelData = new QuestionPartAnswerResponseItemFreeFormModelData();

        var result = testQuestionPartAnswerResponseItemFreeFormModelData.QuestionPartAnswerItem_InputType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseInputType.FreeForm));
    }

    [Theory]
    public void GivenAQuestionPartAnswerResponseItemFreeFormModelData_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testQuestionPartAnswerResponseItemFreeFormModelData = new QuestionPartAnswerResponseItemFreeFormModelData();

        testQuestionPartAnswerResponseItemFreeFormModelData.QuestionPartAnswerItem_InputType = testInputType;

        var result = testQuestionPartAnswerResponseItemFreeFormModelData.QuestionPartAnswerItem_InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }

    [Test]
    public void GivenAQuestionPartAnswerResponseItemFreeFormModelData_WhenISetEnteredValue_ThenEnteredValueIsSet(
        [Values("", "  ", "abc")] string testEnteredValue)
    {
        var testQuestionPartAnswerResponseItemFreeFormModelData = new QuestionPartAnswerResponseItemFreeFormModelData();

        testQuestionPartAnswerResponseItemFreeFormModelData.QuestionPartAnswerItemFreeForm_EnteredValue = testEnteredValue;

        var result = testQuestionPartAnswerResponseItemFreeFormModelData.QuestionPartAnswerItemFreeForm_EnteredValue;

        Assert.That(result, Is.EqualTo(testEnteredValue));
    }

    [Theory]
    public void GivenAQuestionPartAnswerResponseItemFreeFormModelData_WhenISetValueEntryDeclined_ThenValueEntryDeclinedIsSet(
        bool testValueEntryDeclined)
    {
        var testQuestionPartAnswerResponseItemFreeFormModelData = new QuestionPartAnswerResponseItemFreeFormModelData();

        testQuestionPartAnswerResponseItemFreeFormModelData.QuestionPartAnswerItemFreeForm_ValueEntryDeclined = testValueEntryDeclined;

        var result = testQuestionPartAnswerResponseItemFreeFormModelData.QuestionPartAnswerItemFreeForm_ValueEntryDeclined;

        Assert.That(result, Is.EqualTo(testValueEntryDeclined));
    }
}