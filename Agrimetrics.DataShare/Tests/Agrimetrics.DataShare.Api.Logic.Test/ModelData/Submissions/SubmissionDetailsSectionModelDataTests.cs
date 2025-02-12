using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Submissions;

[TestFixture]
public class SubmissionDetailsSectionModelDataTests
{
    [Test]
    public void GivenASubmissionDetailsSectionModelData_WhenISetSectionId_ThenSectionIdIsSet()
    {
        var testSubmissionDetailsSectionModelData = new SubmissionDetailsSectionModelData();

        var testSectionId = new Guid("F8936010-F3F4-4AC4-9B05-37F835793D04");

        testSubmissionDetailsSectionModelData.SubmissionDetailsSection_SectionId = testSectionId;

        var result = testSubmissionDetailsSectionModelData.SubmissionDetailsSection_SectionId;

        Assert.That(result, Is.EqualTo(testSectionId));
    }

    [Test]
    public void GivenASubmissionDetailsSectionModelData_WhenISetSectionNumber_ThenSectionNumberIsSet(
        [Values(-1, 0, 999)] int testSectionNumber)
    {
        var testSubmissionDetailsSectionModelData = new SubmissionDetailsSectionModelData();

        testSubmissionDetailsSectionModelData.SubmissionDetailsSection_SectionNumber = testSectionNumber;

        var result = testSubmissionDetailsSectionModelData.SubmissionDetailsSection_SectionNumber;

        Assert.That(result, Is.EqualTo(testSectionNumber));
    }

    [Test]
    public void GivenASubmissionDetailsSectionModelData_WhenISetSectionHeader_ThenSectionHeaderIsSet(
        [Values("", "  ", "abc")] string testSectionHeader)
    {
        var testSubmissionDetailsSectionModelData = new SubmissionDetailsSectionModelData();

        testSubmissionDetailsSectionModelData.SubmissionDetailsSection_SectionHeader = testSectionHeader;

        var result = testSubmissionDetailsSectionModelData.SubmissionDetailsSection_SectionHeader;

        Assert.That(result, Is.EqualTo(testSectionHeader));
    }

    [Test]
    public void GivenASubmissionDetailsSectionModelData_WhenISetAnEmptySetOfQuestions_ThenQuestionsIsSet()
    {
        var testSubmissionDetailsSectionModelData = new SubmissionDetailsSectionModelData();

        var testQuestions = new List<SubmissionDetailsMainQuestionModelData>();

        testSubmissionDetailsSectionModelData.SubmissionDetailsSection_Questions = testQuestions;

        var result = testSubmissionDetailsSectionModelData.SubmissionDetailsSection_Questions;

        Assert.That(result, Is.EqualTo(testQuestions));
    }

    [Test]
    public void GivenASubmissionDetailsSectionModelData_WhenISetQuestions_ThenQuestionsIsSet()
    {
        var testSubmissionDetailsSectionModelData = new SubmissionDetailsSectionModelData();

        var testQuestions = new List<SubmissionDetailsMainQuestionModelData> {new(), new(), new()};

        testSubmissionDetailsSectionModelData.SubmissionDetailsSection_Questions = testQuestions;

        var result = testSubmissionDetailsSectionModelData.SubmissionDetailsSection_Questions;

        Assert.That(result, Is.EqualTo(testQuestions));
    }
}