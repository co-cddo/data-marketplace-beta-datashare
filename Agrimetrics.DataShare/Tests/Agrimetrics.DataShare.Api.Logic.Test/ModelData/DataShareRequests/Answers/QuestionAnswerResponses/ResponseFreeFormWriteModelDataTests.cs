using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswerResponses;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.Answers.DsrQuestionAnswerResponses;

[TestFixture]
public class DsrQuestionAnswerPartResponseFreeFormWriteModelDataTests
{
    [Test]
    public void GivenADataShareRequestQuestionAnswerPartResponseFreeFormWriteModelData_WhenIGetInputType_ThenInputTypeIsFreeForm()
    {
        var testDataShareRequestQuestionAnswerPartResponseFreeFormWriteModelData = new DataShareRequestQuestionAnswerPartResponseFreeFormWriteModelData
        {
            OrderWithinAnswerPart = It.IsAny<int>(),
            EnteredValue = It.IsAny<string>(),
            ValueEntryDeclined = It.IsAny<bool>()
        };

        var result = testDataShareRequestQuestionAnswerPartResponseFreeFormWriteModelData.InputType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseInputType.FreeForm));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartResponseFreeFormWriteModelData_WhenISetEnteredValue_ThenEnteredValueIsSet(
        [Values("", "  ", "abc")] string testEnteredValue)
    {
        var testDataShareRequestQuestionAnswerPartResponseFreeFormWriteModelData = new DataShareRequestQuestionAnswerPartResponseFreeFormWriteModelData
        {
            OrderWithinAnswerPart = It.IsAny<int>(),
            EnteredValue = testEnteredValue,
            ValueEntryDeclined = It.IsAny<bool>()
        };

        var result = testDataShareRequestQuestionAnswerPartResponseFreeFormWriteModelData.EnteredValue;

        Assert.That(result, Is.EqualTo(testEnteredValue));
    }

    [Theory]
    public void GivenADataShareRequestQuestionAnswerPartResponseFreeFormWriteModelData_WhenISetValueEntryDeclined_ThenValueEntryDeclinedIsSet(
        bool testValueEntryDeclined)
    {
        var testDataShareRequestQuestionAnswerPartResponseFreeFormWriteModelData = new DataShareRequestQuestionAnswerPartResponseFreeFormWriteModelData
        {
            OrderWithinAnswerPart = It.IsAny<int>(),
            EnteredValue = It.IsAny<string>(),
            ValueEntryDeclined = testValueEntryDeclined
        };

        var result = testDataShareRequestQuestionAnswerPartResponseFreeFormWriteModelData.ValueEntryDeclined;

        Assert.That(result, Is.EqualTo(testValueEntryDeclined));
    }
}
