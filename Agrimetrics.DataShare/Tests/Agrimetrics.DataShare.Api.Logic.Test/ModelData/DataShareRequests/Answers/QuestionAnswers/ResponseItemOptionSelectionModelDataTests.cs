using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.Answers.DsrRequestQuestionAnswers;

[TestFixture]
public class QuestionPartAnswerResponseItemOptionSelectionModelDataTests
{
    [Test]
    public void GivenAQuestionPartAnswerResponseItemOptionSelectionModelData_WhenIGetInputType_ThenInputTypeIsOptionSelection()
    {
        var testQuestionPartAnswerResponseItemOptionSelectionModelData = new QuestionPartAnswerResponseItemOptionSelectionModelData();

        var result = testQuestionPartAnswerResponseItemOptionSelectionModelData.QuestionPartAnswerItem_InputType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseInputType.OptionSelection));
    }

    [Theory]
    public void GivenAQuestionPartAnswerResponseItemOptionSelectionModelData_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testQuestionPartAnswerResponseItemOptionSelectionModelData = new QuestionPartAnswerResponseItemOptionSelectionModelData();

        testQuestionPartAnswerResponseItemOptionSelectionModelData.QuestionPartAnswerItem_InputType = testInputType;

        var result = testQuestionPartAnswerResponseItemOptionSelectionModelData.QuestionPartAnswerItem_InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }

    [Test]
    public void GivenAQuestionPartAnswerResponseItemOptionSelectionModelData_WhenISetAnEmptySetOfSelectedOptionItems_ThenSelectedOptionItemsIsSet()
    {
        var testQuestionPartAnswerResponseItemOptionSelectionModelData = new QuestionPartAnswerResponseItemOptionSelectionModelData();

        var testSelectedOptionItems = new List<QuestionPartAnswerItemSelectionOptionItemModelData>();

        testQuestionPartAnswerResponseItemOptionSelectionModelData.QuestionPartAnswerItem_SelectedOptionItems = testSelectedOptionItems;

        var result = testQuestionPartAnswerResponseItemOptionSelectionModelData.QuestionPartAnswerItem_SelectedOptionItems;

        Assert.That(result, Is.EqualTo(testSelectedOptionItems));
    }

    [Test]
    public void GivenAQuestionPartAnswerResponseItemOptionSelectionModelData_WhenISetSelectedOptionItems_ThenSelectedOptionItemsIsSet()
    {
        var testQuestionPartAnswerResponseItemOptionSelectionModelData = new QuestionPartAnswerResponseItemOptionSelectionModelData();

        var testSelectedOptionItems = new List<QuestionPartAnswerItemSelectionOptionItemModelData> {new(), new(), new()};

        testQuestionPartAnswerResponseItemOptionSelectionModelData.QuestionPartAnswerItem_SelectedOptionItems = testSelectedOptionItems;

        var result = testQuestionPartAnswerResponseItemOptionSelectionModelData.QuestionPartAnswerItem_SelectedOptionItems;

        Assert.That(result, Is.EqualTo(testSelectedOptionItems));
    }
}