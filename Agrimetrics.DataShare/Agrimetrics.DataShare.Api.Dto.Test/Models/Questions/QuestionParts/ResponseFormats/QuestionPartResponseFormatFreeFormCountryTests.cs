using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatFreeFormCountryTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatFreeFormCountry_WhenIGetFormatType_ThenFormatTypeIsCountry()
    {
        var testQuestionPartResponseFormatFreeFormCountry = new QuestionPartResponseFormatFreeFormCountry();

        var result = testQuestionPartResponseFormatFreeFormCountry.FormatType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseFormatType.Country));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatFreeFormCountry_WhenISetFormatType_ThenFormatTypeIsSet(
        QuestionPartResponseFormatType testFormatType)
    {
        var testQuestionPartResponseFormatFreeFormCountry = new QuestionPartResponseFormatFreeFormCountry();

        testQuestionPartResponseFormatFreeFormCountry.FormatType = testFormatType;

        var result = testQuestionPartResponseFormatFreeFormCountry.FormatType;

        Assert.That(result, Is.EqualTo(testFormatType));
    }
}