using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;

[TestFixture]
public class DataShareRequestQuestionAnswerPartTests
{
    [Test]
    public void GivenADataShareRequestQuestionAnswerPart_WhenISetQuestionPartId_ThenQuestionPartIdIsSet()
    {
        var testDataShareRequestQuestionAnswerPart = new DataShareRequestQuestionAnswerPart();

        var testQuestionPartId = new Guid("0D162A42-E39E-40AE-BEB0-7BF9CA0D8EE6");

        testDataShareRequestQuestionAnswerPart.QuestionPartId = testQuestionPartId;

        var result = testDataShareRequestQuestionAnswerPart.QuestionPartId;

        Assert.That(result, Is.EqualTo(testQuestionPartId));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPart_WhenISetAnEmptySetOfAnswerPartResponses_ThenAnswerPartResponsesIsSet()
    {
        var testDataShareRequestQuestionAnswerPart = new DataShareRequestQuestionAnswerPart();

        var testAnswerPartResponses = new List<DataShareRequestQuestionAnswerPartResponseBase>();

        testDataShareRequestQuestionAnswerPart.AnswerPartResponses = testAnswerPartResponses;

        var result = testDataShareRequestQuestionAnswerPart.AnswerPartResponses;

        Assert.That(result, Is.EqualTo(testAnswerPartResponses));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPart_WhenISetAnswerPartResponses_ThenAnswerPartResponsesIsSet()
    {
        var testDataShareRequestQuestionAnswerPart = new DataShareRequestQuestionAnswerPart();

        var testAnswerPartResponses = new List<DataShareRequestQuestionAnswerPartResponseBase> {new(), new(), new()};

        testDataShareRequestQuestionAnswerPart.AnswerPartResponses = testAnswerPartResponses;

        var result = testDataShareRequestQuestionAnswerPart.AnswerPartResponses;

        Assert.That(result, Is.EqualTo(testAnswerPartResponses));
    }
}