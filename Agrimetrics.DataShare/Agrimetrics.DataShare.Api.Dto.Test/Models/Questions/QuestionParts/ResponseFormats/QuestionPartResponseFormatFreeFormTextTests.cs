using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatFreeFormTextTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatFreeFormText_WhenIGetFormatType_ThenFormatTypeIsText()
    {
        var testQuestionPartResponseFormatFreeFormText = new QuestionPartResponseFormatFreeFormText();

        var result = testQuestionPartResponseFormatFreeFormText.FormatType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseFormatType.Text));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatFreeFormText_WhenISetFormatType_ThenFormatTypeIsSet(
        QuestionPartResponseFormatType testFormatType)
    {
        var testQuestionPartResponseFormatFreeFormText = new QuestionPartResponseFormatFreeFormText();

        testQuestionPartResponseFormatFreeFormText.FormatType = testFormatType;

        var result = testQuestionPartResponseFormatFreeFormText.FormatType;

        Assert.That(result, Is.EqualTo(testFormatType));
    }

    [Test]
    public void GivenAQuestionPartResponseFormatFreeFormText_WhenISetMaximumResponseLength_ThenMaximumResponseLengthIsText(
        [Values(-1, 0, 999)] int testMaximumResponseLength)
    {
        var testQuestionPartResponseFormatFreeFormText = new QuestionPartResponseFormatFreeFormText();

        testQuestionPartResponseFormatFreeFormText.MaximumResponseLength = testMaximumResponseLength;

        var result = testQuestionPartResponseFormatFreeFormText.MaximumResponseLength;

        Assert.That(result, Is.EqualTo(testMaximumResponseLength));
    }
}