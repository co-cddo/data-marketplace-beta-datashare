using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.FreeFormItems;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatFreeFormModelDataTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatFreeFormModelData_WhenIGetInputType_ThenInputTypeIsFreeForm()
    {
        var testQuestionPartResponseFormatFreeFormModelData = new TestQuestionPartResponseFormatFreeFormModelData();

        var result = testQuestionPartResponseFormatFreeFormModelData.InputType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseInputType.FreeForm));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatFreeFormModelData_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testQuestionPartResponseFormatFreeFormModelData = new TestQuestionPartResponseFormatFreeFormModelData();

        testQuestionPartResponseFormatFreeFormModelData.InputType = testInputType;

        var result = testQuestionPartResponseFormatFreeFormModelData.InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }

    [Test]
    public void GivenAQuestionPartResponseFormatFreeFormModelData_WhenISetANullFreeFormOptions_ThenFreeFormOptionsIsSet()
    {
        var testQuestionPartResponseFormatFreeFormModelData = new TestQuestionPartResponseFormatFreeFormModelData();

        var testFreeFormOptions = (QuestionPartResponseFormatFreeFormOptionsModelData?) null;

        testQuestionPartResponseFormatFreeFormModelData.FreeFormOptions = testFreeFormOptions;

        var result = testQuestionPartResponseFormatFreeFormModelData.FreeFormOptions;

        Assert.That(result, Is.EqualTo(testFreeFormOptions));
    }

    [Test]
    public void GivenAQuestionPartResponseFormatFreeFormModelData_WhenISetFreeFormOptions_ThenFreeFormOptionsIsSet()
    {
        var testQuestionPartResponseFormatFreeFormModelData = new TestQuestionPartResponseFormatFreeFormModelData();

        var testFreeFormOptions = new QuestionPartResponseFormatFreeFormOptionsModelData();

        testQuestionPartResponseFormatFreeFormModelData.FreeFormOptions = testFreeFormOptions;

        var result = testQuestionPartResponseFormatFreeFormModelData.FreeFormOptions;

        Assert.That(result, Is.EqualTo(testFreeFormOptions));
    }

    private class TestQuestionPartResponseFormatFreeFormModelData : QuestionPartResponseFormatFreeFormModelData;
}