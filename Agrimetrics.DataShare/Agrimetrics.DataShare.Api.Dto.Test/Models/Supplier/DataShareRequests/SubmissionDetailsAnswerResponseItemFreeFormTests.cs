using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests;

[TestFixture]
public class SubmissionDetailsAnswerResponseItemFreeFormTests
{
    [Test]
    public void GivenASubmissionDetailsAnswerResponseItemFreeForm_WhenISetAnswerValue_ThenAnswerValueIsSet(
        [Values("", "  ", "abc")] string testAnswerValue)
    {
        var testSubmissionDetailsAnswerResponseItemFreeForm = new SubmissionDetailsAnswerResponseItemFreeForm();

        testSubmissionDetailsAnswerResponseItemFreeForm.AnswerValue = testAnswerValue;

        var result = testSubmissionDetailsAnswerResponseItemFreeForm.AnswerValue;

        Assert.That(result, Is.EqualTo(testAnswerValue));
    }

    [Theory]
    public void GivenASubmissionDetailsAnswerResponseItemFreeForm_WhenISetValueEntryDeclined_ThenValueEntryDeclinedIsSet(
        bool testValueEntryDeclined)
    {
        var testSubmissionDetailsAnswerResponseItemFreeForm = new SubmissionDetailsAnswerResponseItemFreeForm();

        testSubmissionDetailsAnswerResponseItemFreeForm.ValueEntryDeclined = testValueEntryDeclined;

        var result = testSubmissionDetailsAnswerResponseItemFreeForm.ValueEntryDeclined;

        Assert.That(result, Is.EqualTo(testValueEntryDeclined));
    }
}