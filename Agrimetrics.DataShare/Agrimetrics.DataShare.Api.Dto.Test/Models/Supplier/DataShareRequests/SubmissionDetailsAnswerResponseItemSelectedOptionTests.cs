using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests;

[TestFixture]
public class SubmissionDetailsAnswerResponseItemSelectedOptionTests
{
    [Test]
    public void GivenASubmissionDetailsAnswerResponseItemSelectedOption_WhenISetOrderWithinSelectedOptions_ThenOrderWithinSelectedOptionsIsSet(
        [Values(-1, 0, 999)] int testOrderWithinSelectedOptions)
    {
        var testSubmissionDetailsAnswerResponseItemSelectedOption = new SubmissionDetailsAnswerResponseItemSelectedOption();

        testSubmissionDetailsAnswerResponseItemSelectedOption.OrderWithinSelectedOptions = testOrderWithinSelectedOptions;

        var result = testSubmissionDetailsAnswerResponseItemSelectedOption.OrderWithinSelectedOptions;

        Assert.That(result, Is.EqualTo(testOrderWithinSelectedOptions));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerResponseItemSelectedOption_WhenISetSelectionOptionText_ThenSelectionOptionTextIsSet(
        [Values("", "  ", "abc")] string testSelectionOptionText)
    {
        var testSubmissionDetailsAnswerResponseItemSelectedOption = new SubmissionDetailsAnswerResponseItemSelectedOption();

        testSubmissionDetailsAnswerResponseItemSelectedOption.SelectionOptionText = testSelectionOptionText;

        var result = testSubmissionDetailsAnswerResponseItemSelectedOption.SelectionOptionText;

        Assert.That(result, Is.EqualTo(testSelectionOptionText));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerResponseItemSelectedOption_WhenISetSupplementaryAnswerText_ThenSupplementaryAnswerTextIsSet(
        [Values(null, "", "  ", "abc")] string? testSupplementaryAnswerText)
    {
        var testSubmissionDetailsAnswerResponseItemSelectedOption = new SubmissionDetailsAnswerResponseItemSelectedOption();

        testSubmissionDetailsAnswerResponseItemSelectedOption.SupplementaryAnswerText = testSupplementaryAnswerText;

        var result = testSubmissionDetailsAnswerResponseItemSelectedOption.SupplementaryAnswerText;

        Assert.That(result, Is.EqualTo(testSupplementaryAnswerText));
    }
}