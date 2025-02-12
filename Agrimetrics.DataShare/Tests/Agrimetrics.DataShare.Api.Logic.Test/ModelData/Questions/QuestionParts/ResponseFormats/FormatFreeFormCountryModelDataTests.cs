using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatFreeFormCountryModelDataTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatFreeFormCountryModelData_WhenIGetFormatType_ThenFormatTypeIsCountry()
    {
        var testQuestionPartResponseFormatFreeFormCountryModelData = new QuestionPartResponseFormatFreeFormCountryModelData();

        var result = testQuestionPartResponseFormatFreeFormCountryModelData.FormatType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseFormatType.Country));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatFreeFormCountryModelData_WhenISetFormatType_ThenFormatTypeIsSet(
        QuestionPartResponseFormatType testFormatType)
    {
        var testQuestionPartResponseFormatFreeFormCountryModelData = new QuestionPartResponseFormatFreeFormCountryModelData();

        testQuestionPartResponseFormatFreeFormCountryModelData.FormatType = testFormatType;

        var result = testQuestionPartResponseFormatFreeFormCountryModelData.FormatType;

        Assert.That(result, Is.EqualTo(testFormatType));
    }
}