using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Submissions;

[TestFixture]
public class SubmissionDetailsAnswerPartResponseItemModelDataTests
{
    [Test]
    public void GivenASubmissionDetailsAnswerPartResponseItemModelData_WhenISetId_ThenIdIsSet()
    {
        var testSubmissionDetailsAnswerPartResponseItemModelData = new SubmissionDetailsAnswerPartResponseItemModelData();

        var testId = new Guid("ABBE6BCD-183B-4239-8924-49FA2CF789C5");

        testSubmissionDetailsAnswerPartResponseItemModelData.SubmissionDetailsAnswerResponseItem_Id = testId;

        var result = testSubmissionDetailsAnswerPartResponseItemModelData.SubmissionDetailsAnswerResponseItem_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerPartResponseItemModelData_WhenISetANullFreeFormData_ThenFreeFormDataIsSet()
    {
        var testSubmissionDetailsAnswerPartResponseItemModelData = new SubmissionDetailsAnswerPartResponseItemModelData();

        var testFreeFormData = (SubmissionDetailsAnswerPartResponseItemFreeFormModelData?) null;

        testSubmissionDetailsAnswerPartResponseItemModelData.SubmissionDetailsAnswerResponseItem_FreeFormData = testFreeFormData;

        var result = testSubmissionDetailsAnswerPartResponseItemModelData.SubmissionDetailsAnswerResponseItem_FreeFormData;

        Assert.That(result, Is.EqualTo(testFreeFormData));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerPartResponseItemModelData_WhenISetFreeFormData_ThenFreeFormDataIsSet()
    {
        var testSubmissionDetailsAnswerPartResponseItemModelData = new SubmissionDetailsAnswerPartResponseItemModelData();

        var testFreeFormData = new SubmissionDetailsAnswerPartResponseItemFreeFormModelData();

        testSubmissionDetailsAnswerPartResponseItemModelData.SubmissionDetailsAnswerResponseItem_FreeFormData = testFreeFormData;

        var result = testSubmissionDetailsAnswerPartResponseItemModelData.SubmissionDetailsAnswerResponseItem_FreeFormData;

        Assert.That(result, Is.EqualTo(testFreeFormData));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerPartResponseItemModelData_WhenISetANullSelectionOptionData_ThenSelectionOptionDataIsSet()
    {
        var testSubmissionDetailsAnswerPartResponseItemModelData = new SubmissionDetailsAnswerPartResponseItemModelData();

        var testSelectionOptionData = (SubmissionDetailsAnswerPartResponseItemSelectionOptionModelData?) null;

        testSubmissionDetailsAnswerPartResponseItemModelData.SubmissionDetailsAnswerResponseItem_SelectionOptionData = testSelectionOptionData;

        var result = testSubmissionDetailsAnswerPartResponseItemModelData.SubmissionDetailsAnswerResponseItem_SelectionOptionData;

        Assert.That(result, Is.EqualTo(testSelectionOptionData));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerPartResponseItemModelData_WhenISetSelectionOptionData_ThenSelectionOptionDataIsSet()
    {
        var testSubmissionDetailsAnswerPartResponseItemModelData = new SubmissionDetailsAnswerPartResponseItemModelData();

        var testSelectionOptionData = new SubmissionDetailsAnswerPartResponseItemSelectionOptionModelData();

        testSubmissionDetailsAnswerPartResponseItemModelData.SubmissionDetailsAnswerResponseItem_SelectionOptionData = testSelectionOptionData;

        var result = testSubmissionDetailsAnswerPartResponseItemModelData.SubmissionDetailsAnswerResponseItem_SelectionOptionData;

        Assert.That(result, Is.EqualTo(testSelectionOptionData));
    }
}