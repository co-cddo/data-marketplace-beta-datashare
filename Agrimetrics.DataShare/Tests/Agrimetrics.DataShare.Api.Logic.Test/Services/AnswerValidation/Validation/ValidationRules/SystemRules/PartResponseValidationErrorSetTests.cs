using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules.SystemValidationRules;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AnswerValidation.Validation.ValidationRules.SystemValidationRules;

[TestFixture]
public class QuestionAnswerPartResponseValidationErrorSetTests
{
    [Test]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(int.MaxValue)]
    public void GivenAResponseOrderWithinAnswerPart_WhenIConstructAnInstanceOfQuestionAnswerPartResponseValidationErrorSet_ThenResponseOrderWithinAnswerPartIsSetToTheGivenValue(
        int testResponseOrderWithinAnswerPart)
    {
        var questionAnswerPartResponseValidationErrorSet = new QuestionAnswerPartResponseValidationErrorSet
        {
            ResponseOrderWithinAnswerPart = testResponseOrderWithinAnswerPart,
            ValidationErrors = It.IsAny<IEnumerable<string>>()
        };

        Assert.That(questionAnswerPartResponseValidationErrorSet.ResponseOrderWithinAnswerPart, Is.EqualTo(testResponseOrderWithinAnswerPart));
    }

    [Test]
    [TestCaseSource(nameof(ValidationErrorsTestCaseData))]
    public void GivenACollectionOfValidationErrors_WhenIConstructAnInstanceOfQuestionAnswerPartResponseValidationErrorSet_ThenValidationErrorsIsSetToTheGivenValue(
        IEnumerable<string> testValidationErrors)
    {
        var questionAnswerPartResponseValidationErrorSet = new QuestionAnswerPartResponseValidationErrorSet
        {
            ResponseOrderWithinAnswerPart = It.IsAny<int>(),
            ValidationErrors = testValidationErrors
        };

        Assert.That(questionAnswerPartResponseValidationErrorSet.ValidationErrors, Is.EqualTo(testValidationErrors));
    }

    private static IEnumerable<TestCaseData> ValidationErrorsTestCaseData()
    {
        yield return new TestCaseData(Enumerable.Empty<string>());
        yield return new TestCaseData(new List<string>{"test validation error"});
        yield return new TestCaseData(new List<string>{ "test validation error 1", "test validation error 2", "test validation error 3" });
    }
}