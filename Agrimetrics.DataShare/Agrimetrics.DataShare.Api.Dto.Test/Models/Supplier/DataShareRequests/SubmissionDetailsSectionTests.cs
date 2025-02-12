using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests;

[TestFixture]
public class SubmissionDetailsSectionTests
{
    [Test]
    public void GivenASubmissionDetailsSection_WhenISetSectionNumber_ThenSectionNumberIsSet(
        [Values(-1, 0, 999)] int testSectionNumber)
    {
        var testSubmissionDetailsSection = new SubmissionDetailsSection();

        testSubmissionDetailsSection.SectionNumber = testSectionNumber;

        var result = testSubmissionDetailsSection.SectionNumber;

        Assert.That(result, Is.EqualTo(testSectionNumber));
    }

    [Test]
    public void GivenASubmissionDetailsSection_WhenISetSectionHeader_ThenSectionHeaderIsSet(
        [Values("", "  ", "abc")] string testSectionHeader)
    {
        var testSubmissionDetailsSection = new SubmissionDetailsSection();

        testSubmissionDetailsSection.SectionHeader = testSectionHeader;

        var result = testSubmissionDetailsSection.SectionHeader;

        Assert.That(result, Is.EqualTo(testSectionHeader));
    }

    [Test]
    public void GivenASubmissionDetailsSection_WhenISetAnEmptySetOfAnswerGroups_ThenAnswerGroupsIsSet()
    {
        var testSubmissionDetailsSection = new SubmissionDetailsSection();

        var testAnswerGroups = new List<SubmissionDetailsAnswerGroup>();

        testSubmissionDetailsSection.AnswerGroups = testAnswerGroups;

        var result = testSubmissionDetailsSection.AnswerGroups;

        Assert.That(result, Is.EqualTo(testAnswerGroups));
    }

    [Test]
    public void GivenASubmissionDetailsSection_WhenISetAnswerGroups_ThenAnswerGroupsIsSet()
    {
        var testSubmissionDetailsSection = new SubmissionDetailsSection();

        var testAnswerGroups = new List<SubmissionDetailsAnswerGroup> {new(), new(), new()};

        testSubmissionDetailsSection.AnswerGroups = testAnswerGroups;

        var result = testSubmissionDetailsSection.AnswerGroups;

        Assert.That(result, Is.EqualTo(testAnswerGroups));
    }
}