using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatFreeFormDateTimeTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatFreeFormDateTime_WhenIGetFormatType_ThenFormatTypeIsDateTime()
    {
        var testQuestionPartResponseFormatFreeFormDateTime = new QuestionPartResponseFormatFreeFormDateTime();

        var result = testQuestionPartResponseFormatFreeFormDateTime.FormatType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseFormatType.DateTime));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatFreeFormDateTime_WhenISetFormatType_ThenFormatTypeIsSet(
        QuestionPartResponseFormatType testFormatType)
    {
        var testQuestionPartResponseFormatFreeFormDateTime = new QuestionPartResponseFormatFreeFormDateTime();

        testQuestionPartResponseFormatFreeFormDateTime.FormatType = testFormatType;

        var result = testQuestionPartResponseFormatFreeFormDateTime.FormatType;

        Assert.That(result, Is.EqualTo(testFormatType));
    }
}