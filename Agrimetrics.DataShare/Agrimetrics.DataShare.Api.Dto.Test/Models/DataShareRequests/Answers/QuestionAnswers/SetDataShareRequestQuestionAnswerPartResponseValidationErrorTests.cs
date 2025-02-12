using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;

[TestFixture]
public class SetDataShareRequestQuestionAnswerPartResponseValidationErrorTests
{
    [Test]
    public void GivenASetDataShareRequestQuestionAnswerPartResponseValidationError_WhenISetQuestionPartId_ThenQuestionPartIdIsSet()
    {
        var testSetDataShareRequestQuestionAnswerPartResponseValidationError = new SetDataShareRequestQuestionAnswerPartResponseValidationError();

        var testQuestionPartId = new Guid("0D6DE5A2-D78A-4003-8472-2254762F2358");

        testSetDataShareRequestQuestionAnswerPartResponseValidationError.QuestionPartId = testQuestionPartId;

        var result = testSetDataShareRequestQuestionAnswerPartResponseValidationError.QuestionPartId;

        Assert.That(result, Is.EqualTo(testQuestionPartId));
    }

    [Test]
    public void GivenASetDataShareRequestQuestionAnswerPartResponseValidationError_WhenISetResponseOrderWithinAnswerPart_ThenResponseOrderWithinAnswerPartIsSet(
        [Values(-1, 0, 999)] int testResponseOrderWithinAnswerPart)
    {
        var testSetDataShareRequestQuestionAnswerPartResponseValidationError = new SetDataShareRequestQuestionAnswerPartResponseValidationError();

        testSetDataShareRequestQuestionAnswerPartResponseValidationError.ResponseOrderWithinAnswerPart = testResponseOrderWithinAnswerPart;

        var result = testSetDataShareRequestQuestionAnswerPartResponseValidationError.ResponseOrderWithinAnswerPart;

        Assert.That(result, Is.EqualTo(testResponseOrderWithinAnswerPart));
    }

    [Test]
    public void GivenASetDataShareRequestQuestionAnswerPartResponseValidationError_WhenISetAnEmptySetOfValidationErrors_ThenValidationErrorsIsSet()
    {
        var testSetDataShareRequestQuestionAnswerPartResponseValidationError = new SetDataShareRequestQuestionAnswerPartResponseValidationError();

        var testValidationErrors = new List<string>();

        testSetDataShareRequestQuestionAnswerPartResponseValidationError.ValidationErrors = testValidationErrors;

        var result = testSetDataShareRequestQuestionAnswerPartResponseValidationError.ValidationErrors;

        Assert.That(result, Is.EqualTo(testValidationErrors));
    }

    [Test]
    public void GivenASetDataShareRequestQuestionAnswerPartResponseValidationError_WhenISetValidationErrors_ThenValidationErrorsIsSet()
    {
        var testSetDataShareRequestQuestionAnswerPartResponseValidationError = new SetDataShareRequestQuestionAnswerPartResponseValidationError();

        var testValidationErrors = new List<string> {"aaa", "bbb", "ccc"};

        testSetDataShareRequestQuestionAnswerPartResponseValidationError.ValidationErrors = testValidationErrors;

        var result = testSetDataShareRequestQuestionAnswerPartResponseValidationError.ValidationErrors;

        Assert.That(result, Is.EqualTo(testValidationErrors));
    }
}