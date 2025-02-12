using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatReadOnlyModelDataTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatReadOnlyModelData_WhenIGetFormatType_ThenFormatTypeIsReadOnly()
    {
        var testQuestionPartResponseFormatReadOnlyModelData = new QuestionPartResponseFormatReadOnlyModelData();

        var result = testQuestionPartResponseFormatReadOnlyModelData.FormatType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseFormatType.ReadOnly));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatReadOnlyModelData_WhenISetFormatType_ThenFormatTypeIsSet(
        QuestionPartResponseFormatType testFormatType)
    {
        var testQuestionPartResponseFormatReadOnlyModelData = new QuestionPartResponseFormatReadOnlyModelData();

        testQuestionPartResponseFormatReadOnlyModelData.FormatType = testFormatType;

        var result = testQuestionPartResponseFormatReadOnlyModelData.FormatType;

        Assert.That(result, Is.EqualTo(testFormatType));
    }
}