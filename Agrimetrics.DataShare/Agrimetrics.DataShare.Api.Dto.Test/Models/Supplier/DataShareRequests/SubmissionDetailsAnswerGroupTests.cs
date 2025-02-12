using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests;

[TestFixture]
public class SubmissionDetailsAnswerGroupTests
{
    [Test]
    public void GivenASubmissionDetailsAnswerGroup_WhenISetMainQuestionHeader_ThenMainQuestionHeaderIsSet(
        [Values("", "  ", "abc")] string testMainQuestionHeader)
    {
        var testSubmissionDetailsAnswerGroup = new SubmissionDetailsAnswerGroup();

        testSubmissionDetailsAnswerGroup.MainQuestionHeader = testMainQuestionHeader;

        var result = testSubmissionDetailsAnswerGroup.MainQuestionHeader;

        Assert.That(result, Is.EqualTo(testMainQuestionHeader));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerGroup_WhenISetOrderWithinSubmission_ThenOrderWithinSubmissionIsSet(
        [Values(-1, 0, 999)] int testOrderWithinSubmission)
    {
        var testSubmissionDetailsAnswerGroup = new SubmissionDetailsAnswerGroup();

        testSubmissionDetailsAnswerGroup.OrderWithinSubmission = testOrderWithinSubmission;

        var result = testSubmissionDetailsAnswerGroup.OrderWithinSubmission;

        Assert.That(result, Is.EqualTo(testOrderWithinSubmission));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerGroup_WhenISetMainEntry_ThenMainEntryIsSet()
    {
        var testSubmissionDetailsAnswerGroup = new SubmissionDetailsAnswerGroup();

        var testMainEntry = new SubmissionAnswerGroupEntry();

        testSubmissionDetailsAnswerGroup.MainEntry = testMainEntry;

        var result = testSubmissionDetailsAnswerGroup.MainEntry;

        Assert.That(result, Is.SameAs(testMainEntry));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerGroup_WhenISetAnEmptySetOfBackingEntries_ThenBackingEntriesIsSet()
    {
        var testSubmissionDetailsAnswerGroup = new SubmissionDetailsAnswerGroup();

        var testBackingEntries = new List<SubmissionAnswerGroupEntry>();

        testSubmissionDetailsAnswerGroup.BackingEntries = testBackingEntries;

        var result = testSubmissionDetailsAnswerGroup.BackingEntries;

        Assert.That(result, Is.EqualTo(testBackingEntries));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerGroup_WhenISetBackingEntries_ThenBackingEntriesIsSet()
    {
        var testSubmissionDetailsAnswerGroup = new SubmissionDetailsAnswerGroup();

        var testBackingEntries = new List<SubmissionAnswerGroupEntry> {new(), new(), new()};

        testSubmissionDetailsAnswerGroup.BackingEntries = testBackingEntries;

        var result = testSubmissionDetailsAnswerGroup.BackingEntries;

        Assert.That(result, Is.EqualTo(testBackingEntries));
    }
}