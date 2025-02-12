using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatFreeFormTextModelDataTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatFreeFormTextModelData_WhenIGetFormatType_ThenFormatTypeIsText()
    {
        var testQuestionPartResponseFormatFreeFormTextModelData = new QuestionPartResponseFormatFreeFormTextModelData();

        var result = testQuestionPartResponseFormatFreeFormTextModelData.FormatType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseFormatType.Text));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatFreeFormTextModelData_WhenISetFormatType_ThenFormatTypeIsSet(
        QuestionPartResponseFormatType testFormatType)
    {
        var testQuestionPartResponseFormatFreeFormTextModelData = new QuestionPartResponseFormatFreeFormTextModelData();

        testQuestionPartResponseFormatFreeFormTextModelData.FormatType = testFormatType;

        var result = testQuestionPartResponseFormatFreeFormTextModelData.FormatType;

        Assert.That(result, Is.EqualTo(testFormatType));
    }

    [Test]
    public void GivenAQuestionPartResponseFormatFreeFormTextModelData_WhenISetMaximumResponseLength_ThenMaximumResponseLengthIsSet(
        [Values(-1, 0, 999)] int testMaximumResponseLength)
    {
        var testQuestionPartResponseFormatFreeFormTextModelData = new QuestionPartResponseFormatFreeFormTextModelData();

        testQuestionPartResponseFormatFreeFormTextModelData.QuestionPartResponseFormatFreeFormText_MaximumResponseLength = testMaximumResponseLength;

        var result = testQuestionPartResponseFormatFreeFormTextModelData.QuestionPartResponseFormatFreeFormText_MaximumResponseLength;

        Assert.That(result, Is.EqualTo(testMaximumResponseLength));
    }
}