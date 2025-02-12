using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Submissions;

[TestFixture]
public class SubmissionReviewInformationModelDataTests
{
    [Test]
    public void GivenASubmissionReviewInformationModelData_WhenISetSubmissionDetails_ThenSubmissionDetailsIsSet()
    {
        var testSubmissionReviewInformationModelData = new SubmissionReviewInformationModelData();

        var testSubmissionDetails = new SubmissionDetailsModelData();

        testSubmissionReviewInformationModelData.SubmissionReviewInformation_SubmissionDetails = testSubmissionDetails;

        var result = testSubmissionReviewInformationModelData.SubmissionReviewInformation_SubmissionDetails;

        Assert.That(result, Is.SameAs(testSubmissionDetails));
    }

    [Test]
    public void GivenASubmissionReviewInformationModelData_WhenISetSupplierNotes_ThenSupplierNotesIsSet(
        [Values("", "  ", "abc")] string testSupplierNotes)
    {
        var testSubmissionReviewInformationModelData = new SubmissionReviewInformationModelData();

        testSubmissionReviewInformationModelData.SubmissionReviewInformation_SupplierNotes = testSupplierNotes;

        var result = testSubmissionReviewInformationModelData.SubmissionReviewInformation_SupplierNotes;

        Assert.That(result, Is.EqualTo(testSupplierNotes));
    }
}