using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatFreeFormTimeTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatFreeFormTime_WhenIGetFormatType_ThenFormatTypeIsTime()
    {
        var testQuestionPartResponseFormatFreeFormTime = new QuestionPartResponseFormatFreeFormTime();

        var result = testQuestionPartResponseFormatFreeFormTime.FormatType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseFormatType.Time));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatFreeFormTime_WhenISetFormatType_ThenFormatTypeIsSet(
        QuestionPartResponseFormatType testFormatType)
    {
        var testQuestionPartResponseFormatFreeFormTime = new QuestionPartResponseFormatFreeFormTime();

        testQuestionPartResponseFormatFreeFormTime.FormatType = testFormatType;

        var result = testQuestionPartResponseFormatFreeFormTime.FormatType;

        Assert.That(result, Is.EqualTo(testFormatType));
    }
}