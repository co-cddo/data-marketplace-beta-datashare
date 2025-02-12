using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;

[TestFixture]
public class DataShareRequestQuestionAnswerPartResponseSelectionOptionTests
{
    [Test]
    public void GivenADataShareRequestQuestionAnswerPartResponseSelectionOption_WhenIGetInputType_ThenInputTypeIsOptionSelection()
    {
        var testDataShareRequestQuestionAnswerPartResponseSelectionOption = new DataShareRequestQuestionAnswerPartResponseSelectionOption();

        var result = testDataShareRequestQuestionAnswerPartResponseSelectionOption.InputType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseInputType.OptionSelection));
    }

    [Theory]
    public void GivenADataShareRequestQuestionAnswerPartResponseSelectionOption_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testDataShareRequestQuestionAnswerPartResponseSelectionOption = new DataShareRequestQuestionAnswerPartResponseSelectionOption();

        testDataShareRequestQuestionAnswerPartResponseSelectionOption.InputType = testInputType;

        var result = testDataShareRequestQuestionAnswerPartResponseSelectionOption.InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartResponseSelectionOption_WhenISetAnEmptySetOfSelectedOptionItems_ThenSelectedOptionItemsIsSet()
    {
        var testDataShareRequestQuestionAnswerPartResponseSelectionOption = new DataShareRequestQuestionAnswerPartResponseSelectionOption();

        var testSelectedOptionItems = new List<DataShareRequestQuestionAnswerPartResponseSelectionOptionItem>();

        testDataShareRequestQuestionAnswerPartResponseSelectionOption.SelectedOptionItems = testSelectedOptionItems;

        var result = testDataShareRequestQuestionAnswerPartResponseSelectionOption.SelectedOptionItems;

        Assert.That(result, Is.EqualTo(testSelectedOptionItems));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartResponseSelectionOption_WhenISetSelectedOptionItems_ThenSelectedOptionItemsIsSet()
    {
        var testDataShareRequestQuestionAnswerPartResponseSelectionOption = new DataShareRequestQuestionAnswerPartResponseSelectionOption();

        var testSelectedOptionItems = new List<DataShareRequestQuestionAnswerPartResponseSelectionOptionItem> {new(), new(), new()};

        testDataShareRequestQuestionAnswerPartResponseSelectionOption.SelectedOptionItems = testSelectedOptionItems;

        var result = testDataShareRequestQuestionAnswerPartResponseSelectionOption.SelectedOptionItems;

        Assert.That(result, Is.EqualTo(testSelectedOptionItems));
    }
}