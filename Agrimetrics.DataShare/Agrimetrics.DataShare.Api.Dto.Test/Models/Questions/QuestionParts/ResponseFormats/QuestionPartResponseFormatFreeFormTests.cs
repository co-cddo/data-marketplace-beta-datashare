using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.FreeFormItems;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatFreeFormTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatFreeForm_WhenIGetInputType_ThenInputTypeIsFreeForm()
    {
        var testQuestionPartResponseFormatFreeForm = new QuestionPartResponseFormatFreeForm();

        var result = testQuestionPartResponseFormatFreeForm.InputType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseInputType.FreeForm));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatFreeForm_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testQuestionPartResponseFormatFreeForm = new QuestionPartResponseFormatFreeForm();

        testQuestionPartResponseFormatFreeForm.InputType = testInputType;

        var result = testQuestionPartResponseFormatFreeForm.InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }

    [Test]
    public void GivenAQuestionPartResponseFormatFreeForm_WhenISetANullFreeFormOptions_ThenFreeFormOptionsIsSet()
    {
        var testQuestionPartResponseFormatFreeForm = new QuestionPartResponseFormatFreeForm();

        var testFreeFormOptions = (QuestionPartFreeFormOptions?) null;

        testQuestionPartResponseFormatFreeForm.FreeFormOptions = testFreeFormOptions;

        var result = testQuestionPartResponseFormatFreeForm.FreeFormOptions;

        Assert.That(result, Is.EqualTo(testFreeFormOptions));
    }

    [Test]
    public void GivenAQuestionPartResponseFormatFreeForm_WhenISetFreeFormOptions_ThenFreeFormOptionsIsSet()
    {
        var testQuestionPartResponseFormatFreeForm = new QuestionPartResponseFormatFreeForm();

        var testFreeFormOptions = new QuestionPartFreeFormOptions();

        testQuestionPartResponseFormatFreeForm.FreeFormOptions = testFreeFormOptions;

        var result = testQuestionPartResponseFormatFreeForm.FreeFormOptions;

        Assert.That(result, Is.EqualTo(testFreeFormOptions));
    }
}