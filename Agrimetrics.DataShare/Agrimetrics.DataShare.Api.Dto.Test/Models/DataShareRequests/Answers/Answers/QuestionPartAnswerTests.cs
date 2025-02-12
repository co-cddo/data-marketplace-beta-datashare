using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.Answers;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Answers.Answers;

[TestFixture]
public class QuestionPartAnswerTests
{
    [Test]
    public void GivenAQuestionPartAnswer_WhenISetQuestionPartId_ThenQuestionPartIdIsSet()
    {
        var testQuestionPartAnswer = new QuestionPartAnswer();

        var testQuestionPartId = new Guid("C483EAD0-CA65-43C8-8563-758BC611D592");

        testQuestionPartAnswer.QuestionPartId = testQuestionPartId;

        var result = testQuestionPartAnswer.QuestionPartId;

        Assert.That(result, Is.EqualTo(testQuestionPartId));
    }

    [Test]
    public void GivenAQuestionPartAnswer_WhenISetAnEmptySetOfAnswerPartResponses_ThenAnswerPartResponsesIsSet()
    {
        var testQuestionPartAnswer = new QuestionPartAnswer();

        var testAnswerPartResponses = new List<QuestionPartAnswerResponse>();

        testQuestionPartAnswer.AnswerPartResponses = testAnswerPartResponses;

        var result = testQuestionPartAnswer.AnswerPartResponses;

        Assert.That(result, Is.EqualTo(testAnswerPartResponses));
    }

    [Test]
    public void GivenAQuestionPartAnswer_WhenISetAnswerPartResponses_ThenAnswerPartResponsesIsSet()
    {
        var testQuestionPartAnswer = new QuestionPartAnswer();

        var testAnswerPartResponses = new List<QuestionPartAnswerResponse>{ new(), new(), new() };

        testQuestionPartAnswer.AnswerPartResponses = testAnswerPartResponses;

        var result = testQuestionPartAnswer.AnswerPartResponses;

        Assert.That(result, Is.EqualTo(testAnswerPartResponses));
    }
}
