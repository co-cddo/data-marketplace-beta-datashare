using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Submissions;

[TestFixture]
public class SubmissionDetailsAnswerPartResponseItemSelectionOptionModelDataTests
{
    [Test]
    public void GivenASubmissionDetailsAnswerPartResponseItemSelectionOptionModelData_WhenISetId_ThenIdIsSet()
    {
        var testSubmissionDetailsAnswerPartResponseItemSelectionOptionModelData = new SubmissionDetailsAnswerPartResponseItemSelectionOptionModelData();

        var testId = new Guid("B2F67561-B5FC-4FD8-952D-5269097251BD");

        testSubmissionDetailsAnswerPartResponseItemSelectionOptionModelData.SubmissionDetailsAnswerPartResponseItemSelectionOption_Id = testId;

        var result = testSubmissionDetailsAnswerPartResponseItemSelectionOptionModelData.SubmissionDetailsAnswerPartResponseItemSelectionOption_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerPartResponseItemSelectionOptionModelData_WhenISetAnEmptySetOfSelectedOptions_ThenSelectedOptionsIsSet()
    {
        var testSubmissionDetailsAnswerPartResponseItemSelectionOptionModelData = new SubmissionDetailsAnswerPartResponseItemSelectionOptionModelData();

        var testSelectedOptions = new List<SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData>();

        testSubmissionDetailsAnswerPartResponseItemSelectionOptionModelData.SubmissionDetailsAnswerPartResponseItemSelectionOption_SelectedOptions = testSelectedOptions;

        var result = testSubmissionDetailsAnswerPartResponseItemSelectionOptionModelData.SubmissionDetailsAnswerPartResponseItemSelectionOption_SelectedOptions;

        Assert.That(result, Is.EqualTo(testSelectedOptions));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerPartResponseItemSelectionOptionModelData_WhenISetSelectedOptions_ThenSelectedOptionsIsSet()
    {
        var testSubmissionDetailsAnswerPartResponseItemSelectionOptionModelData = new SubmissionDetailsAnswerPartResponseItemSelectionOptionModelData();

        var testSelectedOptions = new List<SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData>{ new(), new(), new()};

        testSubmissionDetailsAnswerPartResponseItemSelectionOptionModelData.SubmissionDetailsAnswerPartResponseItemSelectionOption_SelectedOptions = testSelectedOptions;

        var result = testSubmissionDetailsAnswerPartResponseItemSelectionOptionModelData.SubmissionDetailsAnswerPartResponseItemSelectionOption_SelectedOptions;

        Assert.That(result, Is.EqualTo(testSelectedOptions));
    }
}