using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Submissions;

[TestFixture]
public class SubmissionReturnCommentsModelDataTests
{
    [Test]
    public void GivenASubmissionReturnCommentsModelData_WhenISetReturnedOnUtc_ThenReturnedOnUtcIsSet()
    {
        var testSubmissionReturnCommentsModelData = new SubmissionReturnCommentsModelData();

        var testReturnedOnUtc = new DateTime(2025, 12, 25, 13, 45, 59);

        testSubmissionReturnCommentsModelData.ReturnedOnUtc = testReturnedOnUtc;

        var result = testSubmissionReturnCommentsModelData.ReturnedOnUtc;

        Assert.That(result, Is.EqualTo(testReturnedOnUtc));
    }

    [Test]
    public void GivenASubmissionReturnCommentsModelData_WhenISetComments_ThenCommentsIsSet(
        [Values("", "  ", "abc")] string testComments)
    {
        var testSubmissionReturnCommentsModelData = new SubmissionReturnCommentsModelData();

        testSubmissionReturnCommentsModelData.Comments = testComments;

        var result = testSubmissionReturnCommentsModelData.Comments;

        Assert.That(result, Is.EqualTo(testComments));
    }
}