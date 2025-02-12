using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Submissions;

[TestFixture]
public class SubmissionDetailsAnswerPartResponseItemFreeFormModelDataTests
{
    [Test]
    public void GivenASubmissionDetailsAnswerPartResponseItemFreeFormModelData_WhenISetId_ThenIdIsSet()
    {
        var testSubmissionDetailsAnswerPartResponseItemFreeFormModelData = new SubmissionDetailsAnswerPartResponseItemFreeFormModelData();

        var testId = new Guid("F8936010-F3F4-4AC4-9B05-37F835793D04");

        testSubmissionDetailsAnswerPartResponseItemFreeFormModelData.SubmissionDetailsAnswerPartResponseItemFreeForm_Id = testId;

        var result = testSubmissionDetailsAnswerPartResponseItemFreeFormModelData.SubmissionDetailsAnswerPartResponseItemFreeForm_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerPartResponseItemFreeFormModelData_WhenISetAnswerValue_ThenAnswerValueIsSet(
        [Values("", "  ", "abc")] string testAnswerValue)
    {
        var testSubmissionDetailsAnswerPartResponseItemFreeFormModelData = new SubmissionDetailsAnswerPartResponseItemFreeFormModelData();

        testSubmissionDetailsAnswerPartResponseItemFreeFormModelData.SubmissionDetailsAnswerPartResponseItemFreeForm_AnswerValue = testAnswerValue;

        var result = testSubmissionDetailsAnswerPartResponseItemFreeFormModelData.SubmissionDetailsAnswerPartResponseItemFreeForm_AnswerValue;

        Assert.That(result, Is.EqualTo(testAnswerValue));
    }

    [Theory]
    public void GivenASubmissionDetailsAnswerPartResponseItemFreeFormModelData_WhenISetValueEntryDeclined_ThenValueEntryDeclinedIsSet(
        bool testValueEntryDeclined)
    {
        var testSubmissionDetailsAnswerPartResponseItemFreeFormModelData = new SubmissionDetailsAnswerPartResponseItemFreeFormModelData();

        testSubmissionDetailsAnswerPartResponseItemFreeFormModelData.SubmissionDetailsAnswerPartResponseItemFreeForm_ValueEntryDeclined = testValueEntryDeclined;

        var result = testSubmissionDetailsAnswerPartResponseItemFreeFormModelData.SubmissionDetailsAnswerPartResponseItemFreeForm_ValueEntryDeclined;

        Assert.That(result, Is.EqualTo(testValueEntryDeclined));
    }
}