using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.AnswerValidation;

[TestFixture]
public class QuestionPartAnswerValidationRuleSetModelDataTests
{
    [Test]
    public void GivenAQuestionPartAnswerValidationRuleSetModelData_WhenISetQuestionPartId_ThenQuestionPartIdIsSet()
    {
        var testQuestionPartAnswerValidationRuleSetModelData = new QuestionPartAnswerValidationRuleSetModelData();

        var testQuestionPartId = new Guid("0F827D1E-CA80-4B03-9B7E-950C5564E591");

        testQuestionPartAnswerValidationRuleSetModelData.QuestionPartAnswerValidationRuleSet_QuestionPartId = testQuestionPartId;

        var result = testQuestionPartAnswerValidationRuleSetModelData.QuestionPartAnswerValidationRuleSet_QuestionPartId;

        Assert.That(result, Is.EqualTo(testQuestionPartId));
    }

    [Theory]
    public void GivenAQuestionPartAnswerValidationRuleSetModelData_WhenISetAnswerIsOptional_ThenAnswerIsOptionalIsSet(
        bool testAnswerIsOptional)
    {
        var testQuestionPartAnswerValidationRuleSetModelData = new QuestionPartAnswerValidationRuleSetModelData();

        testQuestionPartAnswerValidationRuleSetModelData.QuestionPartAnswerValidationRuleSet_AnswerIsOptional = testAnswerIsOptional;

        var result = testQuestionPartAnswerValidationRuleSetModelData.QuestionPartAnswerValidationRuleSet_AnswerIsOptional;

        Assert.That(result, Is.EqualTo(testAnswerIsOptional));
    }

    [Theory]
    public void GivenAQuestionPartAnswerValidationRuleSetModelData_WhenISetResponseFormatType_ThenResponseFormatTypeIsSet(
        QuestionPartResponseFormatType testResponseFormatType)
    {
        var testQuestionPartAnswerValidationRuleSetModelData = new QuestionPartAnswerValidationRuleSetModelData();

        testQuestionPartAnswerValidationRuleSetModelData.QuestionPartAnswerValidationRuleSet_ResponseFormatType = testResponseFormatType;

        var result = testQuestionPartAnswerValidationRuleSetModelData.QuestionPartAnswerValidationRuleSet_ResponseFormatType;

        Assert.That(result, Is.EqualTo(testResponseFormatType));
    }

    [Test]
    public void GivenAQuestionPartAnswerValidationRuleSetModelData_WhenISetAnEmptySetOfValidationRules_ThenValidationRulesIsSet()
    {
        var testQuestionPartAnswerValidationRuleSetModelData = new QuestionPartAnswerValidationRuleSetModelData();

        var testValidationRules = new List<QuestionPartAnswerValidationRuleModelData>();

        testQuestionPartAnswerValidationRuleSetModelData.QuestionPartAnswerValidationRuleSet_ValidationRules = testValidationRules;

        var result = testQuestionPartAnswerValidationRuleSetModelData.QuestionPartAnswerValidationRuleSet_ValidationRules;

        Assert.That(result, Is.EqualTo(testValidationRules));
    }

    [Test]
    public void GivenAQuestionPartAnswerValidationRuleSetModelData_WhenISetValidationRules_ThenValidationRulesIsSet()
    {
        var testQuestionPartAnswerValidationRuleSetModelData = new QuestionPartAnswerValidationRuleSetModelData();

        var testValidationRules = new List<QuestionPartAnswerValidationRuleModelData> {new(), new(), new()};

        testQuestionPartAnswerValidationRuleSetModelData.QuestionPartAnswerValidationRuleSet_ValidationRules = testValidationRules;

        var result = testQuestionPartAnswerValidationRuleSetModelData.QuestionPartAnswerValidationRuleSet_ValidationRules;

        Assert.That(result, Is.EqualTo(testValidationRules));
    }
}