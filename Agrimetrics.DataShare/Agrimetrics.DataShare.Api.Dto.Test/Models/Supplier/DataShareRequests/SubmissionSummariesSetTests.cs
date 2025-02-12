using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests;

[TestFixture]
public class SubmissionSummariesSetTests
{
    [Test]
    public void GivenASubmissionSummariesSet_WhenISetAnEmptySetOfPendingSubmissionSummaries_ThenPendingSubmissionSummariesIsSet()
    {
        var testSubmissionSummariesSet = new SubmissionSummariesSet();

        var testPendingSubmissionSummaries = new List<PendingSubmissionSummary>();

        testSubmissionSummariesSet.PendingSubmissionSummaries = testPendingSubmissionSummaries;

        var result = testSubmissionSummariesSet.PendingSubmissionSummaries;

        Assert.That(result, Is.EqualTo(testPendingSubmissionSummaries));
    }

    [Test]
    public void GivenASubmissionSummariesSet_WhenISetPendingSubmissionSummaries_ThenPendingSubmissionSummariesIsSet()
    {
        var testSubmissionSummariesSet = new SubmissionSummariesSet();

        var testPendingSubmissionSummaries = new List<PendingSubmissionSummary> {new(), new(), new()};

        testSubmissionSummariesSet.PendingSubmissionSummaries = testPendingSubmissionSummaries;

        var result = testSubmissionSummariesSet.PendingSubmissionSummaries;

        Assert.That(result, Is.EqualTo(testPendingSubmissionSummaries));
    }

    [Test]
    public void GivenASubmissionSummariesSet_WhenISetAnEmptySetOfCompletedSubmissionSummaries_ThenCompletedSubmissionSummariesIsSet()
    {
        var testSubmissionSummariesSet = new SubmissionSummariesSet();

        var testCompletedSubmissionSummaries = new List<CompletedSubmissionSummary>();

        testSubmissionSummariesSet.CompletedSubmissionSummaries = testCompletedSubmissionSummaries;

        var result = testSubmissionSummariesSet.CompletedSubmissionSummaries;

        Assert.That(result, Is.EqualTo(testCompletedSubmissionSummaries));
    }

    [Test]
    public void GivenASubmissionSummariesSet_WhenISetCompletedSubmissionSummaries_ThenCompletedSubmissionSummariesIsSet()
    {
        var testSubmissionSummariesSet = new SubmissionSummariesSet();

        var testCompletedSubmissionSummaries = new List<CompletedSubmissionSummary> { new(), new(), new() };

        testSubmissionSummariesSet.CompletedSubmissionSummaries = testCompletedSubmissionSummaries;

        var result = testSubmissionSummariesSet.CompletedSubmissionSummaries;

        Assert.That(result, Is.EqualTo(testCompletedSubmissionSummaries));
    }
}