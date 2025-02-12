using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;

[TestFixture]
public class DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionTests
{
    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption_WhenISetOrderWithinAnswerPart_ThenOrderWithinAnswerPartIsSet(
        [Values(-1, 0, 999)] int testOrderWithinAnswerPart)
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption = new DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption();

        testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption.OrderWithinAnswerPart = testOrderWithinAnswerPart;

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption.OrderWithinAnswerPart;

        Assert.That(result, Is.EqualTo(testOrderWithinAnswerPart));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption_WhenISetSelectionOptionText_ThenSelectionOptionTextIsSet(
        [Values("", "  ", "abc")] string testSelectionOptionText)
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption = new DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption();

        testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption.SelectionOptionText = testSelectionOptionText;

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption.SelectionOptionText;

        Assert.That(result, Is.EqualTo(testSelectionOptionText));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption_WhenISetSupplementaryAnswerText_ThenSupplementaryAnswerTextIsSet(
        [Values(null, "", "  ", "abc")] string? testSupplementaryAnswerText)
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption = new DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption();

        testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption.SupplementaryAnswerText = testSupplementaryAnswerText;

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption.SupplementaryAnswerText;

        Assert.That(result, Is.EqualTo(testSupplementaryAnswerText));
    }
}