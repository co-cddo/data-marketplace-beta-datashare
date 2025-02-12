using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatFreeFormDateTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatFreeFormDate_WhenIGetFormatType_ThenFormatTypeIsDate()
    {
        var testQuestionPartResponseFormatFreeFormDate = new QuestionPartResponseFormatFreeFormDate();

        var result = testQuestionPartResponseFormatFreeFormDate.FormatType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseFormatType.Date));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatFreeFormDate_WhenISetFormatType_ThenFormatTypeIsSet(
        QuestionPartResponseFormatType testFormatType)
    {
        var testQuestionPartResponseFormatFreeFormDate = new QuestionPartResponseFormatFreeFormDate();

        testQuestionPartResponseFormatFreeFormDate.FormatType = testFormatType;

        var result = testQuestionPartResponseFormatFreeFormDate.FormatType;

        Assert.That(result, Is.EqualTo(testFormatType));
    }
}