using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests;

[TestFixture]
public class SubmissionAnswerGroupEntryTests
{
    [Test]
    public void GivenASubmissionAnswerGroupEntry_WhenISetAnEmptySetOfEntryParts_ThenEntryPartsIsSet()
    {
        var testSubmissionAnswerGroupEntry = new SubmissionAnswerGroupEntry();

        var testEntryParts = new List<SubmissionAnswerGroupEntryPart>();

        testSubmissionAnswerGroupEntry.EntryParts = testEntryParts;

        var result = testSubmissionAnswerGroupEntry.EntryParts;

        Assert.That(result, Is.EqualTo(testEntryParts));
    }

    [Test]
    public void GivenASubmissionAnswerGroupEntry_WhenISetEntryParts_ThenEntryPartsIsSet()
    {
        var testSubmissionAnswerGroupEntry = new SubmissionAnswerGroupEntry();

        var testEntryParts = new List<SubmissionAnswerGroupEntryPart> {new(), new(), new()};

        testSubmissionAnswerGroupEntry.EntryParts = testEntryParts;

        var result = testSubmissionAnswerGroupEntry.EntryParts;

        Assert.That(result, Is.EqualTo(testEntryParts));
    }
}