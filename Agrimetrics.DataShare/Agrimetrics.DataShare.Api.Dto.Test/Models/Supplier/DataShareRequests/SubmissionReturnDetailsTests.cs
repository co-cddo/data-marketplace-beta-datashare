using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests;

[TestFixture]
public class SubmissionReturnDetailsTests
{
    [Test]
    public void GivenASubmissionReturnDetails_WhenISetReturnedOnUtc_ThenReturnedOnUtcIsSet()
    {
        var testSubmissionReturnDetails = new SubmissionReturnDetails();

        var testReturnedOnUtc = new DateTime(2025, 12, 25, 14, 45, 59);

        testSubmissionReturnDetails.ReturnedOnUtc = testReturnedOnUtc;

        var result = testSubmissionReturnDetails.ReturnedOnUtc;

        Assert.That(result, Is.EqualTo(testReturnedOnUtc));
    }

    [Test]
    public void GivenASubmissionReturnDetails_WhenISetReturnComments_ThenReturnCommentsIsSet(
        [Values("", "  ", "abc")] string testReturnComments)
    {
        var testSubmissionReturnDetails = new SubmissionReturnDetails();

        testSubmissionReturnDetails.ReturnComments = testReturnComments;

        var result = testSubmissionReturnDetails.ReturnComments;

        Assert.That(result, Is.EqualTo(testReturnComments));
    }
}