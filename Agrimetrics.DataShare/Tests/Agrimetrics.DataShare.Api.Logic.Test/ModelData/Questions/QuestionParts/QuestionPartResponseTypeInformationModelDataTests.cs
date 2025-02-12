using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionParts;

[TestFixture]
public class QuestionPartResponseTypeInformationModelDataTests
{
    [Test]
    public void GivenAQuestionPartResponseTypeInformationModelData_WhenISetQuestionPartId_ThenQuestionPartIdIsSet()
    {
        var testQuestionPartResponseTypeInformationModelData = new QuestionPartResponseTypeInformationModelData();

        var testQuestionPartId = new Guid("2C6DA030-3B62-4566-B47F-1F2356A6FD89");

        testQuestionPartResponseTypeInformationModelData.QuestionPartResponseTypeInformation_QuestionPartId = testQuestionPartId;

        var result = testQuestionPartResponseTypeInformationModelData.QuestionPartResponseTypeInformation_QuestionPartId;

        Assert.That(result, Is.EqualTo(testQuestionPartId));
    }

    [Theory]
    public void GivenAQuestionPartResponseTypeInformationModelData_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testQuestionPartResponseTypeInformationModelData = new QuestionPartResponseTypeInformationModelData();

        testQuestionPartResponseTypeInformationModelData.QuestionPartResponseTypeInformation_InputType = testInputType;

        var result = testQuestionPartResponseTypeInformationModelData.QuestionPartResponseTypeInformation_InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }

    [Theory]
    public void GivenAQuestionPartResponseTypeInformationModelData_WhenISetFormatType_ThenFormatTypeIsSet(
        QuestionPartResponseFormatType testFormatType)
    {
        var testQuestionPartResponseTypeInformationModelData = new QuestionPartResponseTypeInformationModelData();

        testQuestionPartResponseTypeInformationModelData.QuestionPartResponseTypeInformation_FormatType = testFormatType;

        var result = testQuestionPartResponseTypeInformationModelData.QuestionPartResponseTypeInformation_FormatType;

        Assert.That(result, Is.EqualTo(testFormatType));
    }
}