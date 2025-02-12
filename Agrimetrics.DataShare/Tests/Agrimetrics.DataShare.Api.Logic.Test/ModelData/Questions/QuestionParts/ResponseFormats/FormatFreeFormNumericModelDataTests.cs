using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatFreeFormNumericModelDataTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatFreeFormNumericModelData_WhenIGetFormatType_ThenFormatTypeIsNumeric()
    {
        var testQuestionPartResponseFormatFreeFormNumericModelData = new QuestionPartResponseFormatFreeFormNumericModelData();

        var result = testQuestionPartResponseFormatFreeFormNumericModelData.FormatType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseFormatType.Numeric));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatFreeFormNumericModelData_WhenISetFormatType_ThenFormatTypeIsSet(
        QuestionPartResponseFormatType testFormatType)
    {
        var testQuestionPartResponseFormatFreeFormNumericModelData = new QuestionPartResponseFormatFreeFormNumericModelData();

        testQuestionPartResponseFormatFreeFormNumericModelData.FormatType = testFormatType;

        var result = testQuestionPartResponseFormatFreeFormNumericModelData.FormatType;

        Assert.That(result, Is.EqualTo(testFormatType));
    }
}