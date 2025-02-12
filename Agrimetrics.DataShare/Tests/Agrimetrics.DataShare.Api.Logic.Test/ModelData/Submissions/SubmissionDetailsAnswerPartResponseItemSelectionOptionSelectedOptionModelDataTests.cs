using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Submissions;

[TestFixture]
public class SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelDataTests
{
    [Test]
    public void GivenASubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData_WhenISetId_ThenIdIsSet()
    {
        var testSubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData = new SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData();

        var testId = new Guid("F8936010-F3F4-4AC4-9B05-37F835793D04");

        testSubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData.SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData_Id = testId;

        var result = testSubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData.SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData_WhenISetOrderWithinSelectedOptions_ThenOrderWithinSelectedOptionsIsSet(
        [Values(-1, 0, 999)] int testOrderWithinSelectedOptions)
    {
        var testSubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData = new SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData();

        testSubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData.SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData_OrderWithinSelectedOptions = testOrderWithinSelectedOptions;

        var result = testSubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData.SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData_OrderWithinSelectedOptions;

        Assert.That(result, Is.EqualTo(testOrderWithinSelectedOptions));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData_WhenISetOptionValueText_ThenOptionValueTextIsSet(
        [Values("", "  ", "abc")] string testOptionValueText)
    {
        var testSubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData = new SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData();

        testSubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData.SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData_OptionValueText = testOptionValueText;

        var result = testSubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData.SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData_OptionValueText;

        Assert.That(result, Is.EqualTo(testOptionValueText));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData_WhenISetSupplementaryAnswerText_ThenSupplementaryAnswerTextIsSet(
        [Values(null, "", "  ", "abc")] string? testSupplementaryAnswerText)
    {
        var testSubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData = new SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData();

        testSubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData.SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData_SupplementaryAnswerText = testSupplementaryAnswerText;

        var result = testSubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData.SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData_SupplementaryAnswerText;

        Assert.That(result, Is.EqualTo(testSupplementaryAnswerText));
    }
}