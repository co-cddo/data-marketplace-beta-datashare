using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.Answers;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Questions;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Questions;

[TestFixture]
public class DataShareRequestQuestionPartTests
{
    [Test]
    public void GivenADataShareRequestQuestionPart_WhenISetQuestionPartQuestion_ThenQuestionPartQuestionIsSet()
    {
        var testDataShareRequestQuestionPart = new DataShareRequestQuestionPart();

        var testQuestionPartQuestion = new QuestionPart();

        testDataShareRequestQuestionPart.QuestionPartQuestion = testQuestionPartQuestion;

        var result = testDataShareRequestQuestionPart.QuestionPartQuestion;

        Assert.That(result, Is.SameAs(testQuestionPartQuestion));
    }

    [Test]
    public void GivenADataShareRequestQuestionPart_WhenISetANullQuestionPartAnswer_ThenQuestionPartAnswerIsSet()
    {
        var testDataShareRequestQuestionPart = new DataShareRequestQuestionPart();

        var testQuestionPartAnswer = (QuestionPartAnswer?) null;

        testDataShareRequestQuestionPart.QuestionPartAnswer = testQuestionPartAnswer;

        var result = testDataShareRequestQuestionPart.QuestionPartAnswer;

        Assert.That(result, Is.EqualTo(testQuestionPartAnswer));
    }

    [Test]
    public void GivenADataShareRequestQuestionPart_WhenISetQuestionPartAnswer_ThenQuestionPartAnswerIsSet()
    {
        var testDataShareRequestQuestionPart = new DataShareRequestQuestionPart();

        var testQuestionPartAnswer = new QuestionPartAnswer();

        testDataShareRequestQuestionPart.QuestionPartAnswer = testQuestionPartAnswer;

        var result = testDataShareRequestQuestionPart.QuestionPartAnswer;

        Assert.That(result, Is.EqualTo(testQuestionPartAnswer));
    }
}