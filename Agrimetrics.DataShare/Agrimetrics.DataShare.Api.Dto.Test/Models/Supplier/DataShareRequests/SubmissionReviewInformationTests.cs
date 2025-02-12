using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests;

[TestFixture]
public class SubmissionReviewInformationTests
{
    [Test]
    public void GivenASubmissionReviewInformation_WhenISetSubmissionDetails_ThenSubmissionDetailsIsSet()
    {
        var testSubmissionReviewInformation = new SubmissionReviewInformation();

        var testSubmissionDetails = new SubmissionDetails();

        testSubmissionReviewInformation.SubmissionDetails = testSubmissionDetails;

        var result = testSubmissionReviewInformation.SubmissionDetails;

        Assert.That(result, Is.EqualTo(testSubmissionDetails));
    }

    [Test]
    public void GivenASubmissionReviewInformation_WhenISetSupplierNotes_ThenSupplierNotesIsSet(
        [Values("", "  ", "abc")] string testSupplierNotes)
    {
        var testSubmissionReviewInformation = new SubmissionReviewInformation();

        testSubmissionReviewInformation.SupplierNotes = testSupplierNotes;

        var result = testSubmissionReviewInformation.SupplierNotes;

        Assert.That(result, Is.EqualTo(testSupplierNotes));
    }
}