using Agrimetrics.DataShare.Api.Dto.Models.QuestionConfiguration.CompulsoryQuestions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.QuestionConfiguration.CompulsoryQuestions;

[TestFixture]
public class CompulsoryQuestionTests
{
    [Test]
    public void GivenACompulsoryQuestion_WhenISetQuestionId_ThenQuestionIdIsSet()
    {
        var testCompulsoryQuestion = new CompulsoryQuestion();

        var testQuestionId = new Guid("441F7848-F23A-4A4C-9A6D-52934A8F4F5E");

        testCompulsoryQuestion.QuestionId = testQuestionId;

        var result = testCompulsoryQuestion.QuestionId;

        Assert.That(result, Is.EqualTo(testQuestionId));
    }
}

