using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules.SystemValidationRules;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AnswerValidation.Validation.ValidationRules.SystemValidationRules;

[TestFixture]
public class QuestionAnswerPartValidationErrorSetTests
{
    [Test]
    [TestCaseSource(nameof(ValidationErrorsTestCaseData))]
    public void GivenACollectionOfQuestionAnswerPartResponseValidationErrorSets_WhenIConstructAnInstanceOfQuestionAnswerPartValidationErrorSet_ThenResponseValidationErrorsIsSetToTheGivenValue(
        IEnumerable<IQuestionAnswerPartResponseValidationErrorSet> testResponseValidationErrors)
    {
        var questionAnswerPartValidationErrorSet = new QuestionAnswerPartValidationErrorSet
        {
            ResponseValidationErrors = testResponseValidationErrors
        };

        Assert.That(questionAnswerPartValidationErrorSet.ResponseValidationErrors, Is.EqualTo(testResponseValidationErrors));
    }

    private static IEnumerable<TestCaseData> ValidationErrorsTestCaseData()
    {
        yield return new TestCaseData(Enumerable.Empty<IQuestionAnswerPartResponseValidationErrorSet>());
        
        yield return new TestCaseData(new List<IQuestionAnswerPartResponseValidationErrorSet>
        {
            Mock.Of<IQuestionAnswerPartResponseValidationErrorSet>()
        });

        yield return new TestCaseData(new List<IQuestionAnswerPartResponseValidationErrorSet>
        {
            Mock.Of<IQuestionAnswerPartResponseValidationErrorSet>(),
            Mock.Of<IQuestionAnswerPartResponseValidationErrorSet>(),
            Mock.Of<IQuestionAnswerPartResponseValidationErrorSet>(),
        });
    }
}