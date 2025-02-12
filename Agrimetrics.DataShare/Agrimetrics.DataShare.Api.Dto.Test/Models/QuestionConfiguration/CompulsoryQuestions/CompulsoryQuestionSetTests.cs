using Agrimetrics.DataShare.Api.Dto.Models.QuestionConfiguration.CompulsoryQuestions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.QuestionConfiguration.CompulsoryQuestions;

[TestFixture]
public class CompulsoryQuestionSetTests
{
    [Test]
    public void GivenACompulsoryQuestionSet_WhenISetAnEmptySetOfCompulsoryQuestions_ThenXXXIsSet()
    {
        var testCompulsoryQuestion = new CompulsoryQuestionSet();

        var testCompulsoryQuestions = new List<CompulsoryQuestion>();

        testCompulsoryQuestion.CompulsoryQuestions = testCompulsoryQuestions;

        var result = testCompulsoryQuestion.CompulsoryQuestions;

        Assert.That(result, Is.EqualTo(testCompulsoryQuestions));
    }

    [Test]
    public void GivenACompulsoryQuestionSet_WhenISetCompulsoryQuestions_ThenXXXIsSet()
    {
        var testCompulsoryQuestion = new CompulsoryQuestionSet();

        var testCompulsoryQuestions = new List<CompulsoryQuestion> {new(), new(), new()};

        testCompulsoryQuestion.CompulsoryQuestions = testCompulsoryQuestions;

        var result = testCompulsoryQuestion.CompulsoryQuestions;

        Assert.That(result, Is.EqualTo(testCompulsoryQuestions));
    }
}