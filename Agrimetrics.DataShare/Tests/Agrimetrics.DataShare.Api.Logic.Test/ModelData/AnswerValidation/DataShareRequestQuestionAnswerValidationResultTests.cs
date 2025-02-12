using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.AnswerValidation;

[TestFixture]
public class DataShareRequestQuestionAnswerValidationResultTests
{
    [Test]
    public void GivenADataShareRequestQuestionAnswerValidationResult_WhenISetAnEmptySetOfValidationErrors_ThenAnswerIsValidIsTrue()
    {
        var testValidationErrors = new List<SetDataShareRequestQuestionAnswerPartResponseValidationError>();

        var testDataShareRequestQuestionAnswerValidationResult = new DataShareRequestQuestionAnswerValidationResult
        {
            ValidationErrors = testValidationErrors
        };

        var result = testDataShareRequestQuestionAnswerValidationResult.AnswerIsValid;

        Assert.That(result, Is.True);
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerValidationResult_WhenISetANonEmptySetOfValidationErrors_ThenAnswerIsValidIsFalse()
    {
        var testValidationErrors = new List<SetDataShareRequestQuestionAnswerPartResponseValidationError> {new(), new(), new()};

        var testDataShareRequestQuestionAnswerValidationResult = new DataShareRequestQuestionAnswerValidationResult
        {
            ValidationErrors = testValidationErrors
        };

        var result = testDataShareRequestQuestionAnswerValidationResult.AnswerIsValid;

        Assert.That(result, Is.False);
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerValidationResult_WhenISetAnEmptySetOfValidationErrors_ThenValidationErrorsIsSet()
    {
        var testValidationErrors = new List<SetDataShareRequestQuestionAnswerPartResponseValidationError>();

        var testDataShareRequestQuestionAnswerValidationResult = new DataShareRequestQuestionAnswerValidationResult
        {
            ValidationErrors = testValidationErrors
        };

        var result = testDataShareRequestQuestionAnswerValidationResult.ValidationErrors;

        Assert.That(result, Is.EqualTo(testValidationErrors));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerValidationResult_WhenISetANonEmptySetOfValidationErrors_ThenValidationErrorsIsSet()
    {
        var testValidationErrors = new List<SetDataShareRequestQuestionAnswerPartResponseValidationError> { new(), new(), new() };

        var testDataShareRequestQuestionAnswerValidationResult = new DataShareRequestQuestionAnswerValidationResult
        {
            ValidationErrors = testValidationErrors
        };

        var result = testDataShareRequestQuestionAnswerValidationResult.ValidationErrors;

        Assert.That(result, Is.EqualTo(testValidationErrors));
    }
}

