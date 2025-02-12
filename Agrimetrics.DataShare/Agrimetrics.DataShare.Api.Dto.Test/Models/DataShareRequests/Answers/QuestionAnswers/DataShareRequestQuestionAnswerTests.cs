using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;

[TestFixture]
public class DataShareRequestQuestionAnswerTests
{
    [Test]
    public void GivenADataShareRequestQuestionAnswer_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testDataShareRequestQuestionAnswer = new DataShareRequestQuestionAnswer();

        var testDataShareRequestId = new Guid("0BCD1266-A070-4A2A-AE3C-4D72F892E506");

        testDataShareRequestQuestionAnswer.DataShareRequestId = testDataShareRequestId;

        var result = testDataShareRequestQuestionAnswer.DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswer_WhenISetQuestionId_ThenQuestionIdIsSet()
    {
        var testDataShareRequestQuestionAnswer = new DataShareRequestQuestionAnswer();

        var testQuestionId = new Guid("2EDC4C0F-39B5-43BC-AF72-FB8D09EC2479");

        testDataShareRequestQuestionAnswer.QuestionId = testQuestionId;

        var result = testDataShareRequestQuestionAnswer.QuestionId;

        Assert.That(result, Is.EqualTo(testQuestionId));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswer_WhenISetAnEmptySetOfAnswerParts_ThenAnswerPartsIsSet()
    {
        var testDataShareRequestQuestionAnswer = new DataShareRequestQuestionAnswer();

        var testAnswerParts = new List<DataShareRequestQuestionAnswerPart>();

        testDataShareRequestQuestionAnswer.AnswerParts = testAnswerParts;

        var result = testDataShareRequestQuestionAnswer.AnswerParts;

        Assert.That(result, Is.EqualTo(testAnswerParts));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswer_WhenISetAnswerParts_ThenAnswerPartsIsSet()
    {
        var testDataShareRequestQuestionAnswer = new DataShareRequestQuestionAnswer();

        var testAnswerParts = new List<DataShareRequestQuestionAnswerPart> {new(), new(), new()};

        testDataShareRequestQuestionAnswer.AnswerParts = testAnswerParts;

        var result = testDataShareRequestQuestionAnswer.AnswerParts;

        Assert.That(result, Is.EqualTo(testAnswerParts));
    }
}

