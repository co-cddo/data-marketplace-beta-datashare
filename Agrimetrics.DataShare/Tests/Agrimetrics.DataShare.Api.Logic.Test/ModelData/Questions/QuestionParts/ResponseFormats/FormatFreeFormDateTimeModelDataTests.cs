using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatFreeFormDateTimeModelDataTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatFreeFormDateTimeModelData_WhenIGetFormatType_ThenFormatTypeIsDateTime()
    {
        var testQuestionPartResponseFormatFreeFormDateTimeModelData = new QuestionPartResponseFormatFreeFormDateTimeModelData();

        var result = testQuestionPartResponseFormatFreeFormDateTimeModelData.FormatType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseFormatType.DateTime));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatFreeFormDateTimeModelData_WhenISetFormatType_ThenFormatTypeIsSet(
        QuestionPartResponseFormatType testFormatType)
    {
        var testQuestionPartResponseFormatFreeFormDateTimeModelData = new QuestionPartResponseFormatFreeFormDateTimeModelData();

        testQuestionPartResponseFormatFreeFormDateTimeModelData.FormatType = testFormatType;

        var result = testQuestionPartResponseFormatFreeFormDateTimeModelData.FormatType;

        Assert.That(result, Is.EqualTo(testFormatType));
    }
}