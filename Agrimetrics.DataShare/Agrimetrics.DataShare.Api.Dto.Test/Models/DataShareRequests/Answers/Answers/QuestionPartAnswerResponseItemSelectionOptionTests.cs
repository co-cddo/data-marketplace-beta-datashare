using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.Answers;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Answers.Answers;

[TestFixture]
public class QuestionPartAnswerResponseItemSelectionOptionTests
{
    [Theory]
    public void QuestionPartAnswerResponseItemSelectionOption_WhenIGetInputType_ThenInputTypeIsOptionSelection(
        QuestionPartResponseInputType testInputType)
    {
        var testQuestionPartAnswerResponseItemSelectionOption = new QuestionPartAnswerResponseItemSelectionOption();

        var result = testQuestionPartAnswerResponseItemSelectionOption.InputType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseInputType.OptionSelection));
    }

    [Theory]
    public void QuestionPartAnswerResponseItemSelectionOption_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testQuestionPartAnswerResponseItemSelectionOption = new QuestionPartAnswerResponseItemSelectionOption();

        testQuestionPartAnswerResponseItemSelectionOption.InputType = testInputType;

        var result = testQuestionPartAnswerResponseItemSelectionOption.InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }

    [Test]
    public void QuestionPartAnswerResponseItemSelectionOption_WhenISetAnEmptySetOfSelectedOptions_ThenSelectedOptionsIsSet()
    {
        var testQuestionPartAnswerResponseItemSelectionOption = new QuestionPartAnswerResponseItemSelectionOption();

        var testSelectedOptions = new List<QuestionPartAnswerItemSelectionOptionItem>();

        testQuestionPartAnswerResponseItemSelectionOption.SelectedOptions = testSelectedOptions;

        var result = testQuestionPartAnswerResponseItemSelectionOption.SelectedOptions;

        Assert.That(result, Is.EqualTo(testSelectedOptions));
    }

    [Test]
    public void QuestionPartAnswerResponseItemSelectionOption_WhenISetSelectedOptions_ThenSelectedOptionsIsSet()
    {
        var testQuestionPartAnswerResponseItemSelectionOption = new QuestionPartAnswerResponseItemSelectionOption();

        var testSelectedOptions = new List<QuestionPartAnswerItemSelectionOptionItem> {new(), new(), new()};

        testQuestionPartAnswerResponseItemSelectionOption.SelectedOptions = testSelectedOptions;

        var result = testQuestionPartAnswerResponseItemSelectionOption.SelectedOptions;

        Assert.That(result, Is.SameAs(testSelectedOptions));
    }
}