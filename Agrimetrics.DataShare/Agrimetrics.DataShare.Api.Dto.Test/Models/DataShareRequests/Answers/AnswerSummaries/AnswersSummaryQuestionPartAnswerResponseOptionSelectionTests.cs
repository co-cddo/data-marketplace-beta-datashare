using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;

[TestFixture]
public class DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionTests
{
    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelection_WhenIGetInputType_ThenInputTypeIsOptionSelection()
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelection = new DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelection();

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelection.InputType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseInputType.OptionSelection));
    }

    [Theory]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelection_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelection = new DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelection();

        testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelection.InputType = testInputType;

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelection.InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelection_WhenISetAnEmptySetOfSelectedOptions_ThenSelectedOptionsIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelection = new DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelection();

        var testSelectedOptions = new List<DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption>();

        testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelection.SelectedOptions = testSelectedOptions;

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelection.SelectedOptions;

        Assert.That(result, Is.EqualTo(testSelectedOptions));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelection_WhenISetSelectedOptions_ThenSelectedOptionsIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelection = new DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelection();

        var testSelectedOptions = new List<DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption>{ new(), new(), new() };

        testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelection.SelectedOptions = testSelectedOptions;

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelection.SelectedOptions;

        Assert.That(result, Is.EqualTo(testSelectedOptions));
    }
}