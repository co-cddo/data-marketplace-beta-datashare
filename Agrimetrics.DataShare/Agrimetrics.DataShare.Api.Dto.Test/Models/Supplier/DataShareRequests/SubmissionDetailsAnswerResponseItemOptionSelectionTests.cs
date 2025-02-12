using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests;

[TestFixture]
public class SubmissionDetailsAnswerResponseItemOptionSelectionTests
{
    [Test]
    public void GivenASubmissionDetailsAnswerResponseItemOptionSelection_WhenISetAnEmptySetOfSelectedOptions_ThenSelectedOptionsIsSet()
    {
        var testSubmissionDetailsAnswerResponseItemOptionSelection = new SubmissionDetailsAnswerResponseItemOptionSelection();

        var testSelectedOptions = new List<SubmissionDetailsAnswerResponseItemSelectedOption>();

        testSubmissionDetailsAnswerResponseItemOptionSelection.SelectedOptions = testSelectedOptions;

        var result = testSubmissionDetailsAnswerResponseItemOptionSelection.SelectedOptions;

        Assert.That(result, Is.EqualTo(testSelectedOptions));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerResponseItemOptionSelection_WhenISetSelectedOptions_ThenSelectedOptionsIsSet()
    {
        var testSubmissionDetailsAnswerResponseItemOptionSelection = new SubmissionDetailsAnswerResponseItemOptionSelection();

        var testSelectedOptions = new List<SubmissionDetailsAnswerResponseItemSelectedOption> {new(), new(), new()};

        testSubmissionDetailsAnswerResponseItemOptionSelection.SelectedOptions = testSelectedOptions;

        var result = testSubmissionDetailsAnswerResponseItemOptionSelection.SelectedOptions;

        Assert.That(result, Is.EqualTo(testSelectedOptions));
    }
}