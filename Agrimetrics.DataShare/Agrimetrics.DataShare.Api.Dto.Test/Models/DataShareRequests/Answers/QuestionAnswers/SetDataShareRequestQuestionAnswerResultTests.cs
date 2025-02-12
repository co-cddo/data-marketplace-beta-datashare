using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Questions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;

[TestFixture]
public class SetDataShareRequestQuestionAnswerResultTests
{
    [Test]
    public void GivenASetDataShareRequestQuestionAnswerResult_WhenISetANullNextQuestionId_ThenNextQuestionIdIsSet()
    {
        var testSetDataShareRequestQuestionAnswerResult = new SetDataShareRequestQuestionAnswerResult();

        var testNextQuestionId = (Guid?) null;

        testSetDataShareRequestQuestionAnswerResult.NextQuestionId = testNextQuestionId;

        var result = testSetDataShareRequestQuestionAnswerResult.NextQuestionId;

        Assert.That(result, Is.EqualTo(testNextQuestionId));
    }

    [Test]
    public void GivenASetDataShareRequestQuestionAnswerResult_WhenISetNextQuestionId_ThenNextQuestionIdIsSet()
    {
        var testSetDataShareRequestQuestionAnswerResult = new SetDataShareRequestQuestionAnswerResult();

        var testNextQuestionId = new Guid("47F4013C-06AD-4D22-B644-D62FA4CB7208");

        testSetDataShareRequestQuestionAnswerResult.NextQuestionId = testNextQuestionId;

        var result = testSetDataShareRequestQuestionAnswerResult.NextQuestionId;

        Assert.That(result, Is.EqualTo(testNextQuestionId));
    }

    [Theory]
    public void GivenASetDataShareRequestQuestionAnswerResult_WhenISetAnswerIsValid_ThenAnswerIsValidIsSet(
        bool testAnswerIsValid)
    {
        var testSetDataShareRequestQuestionAnswerResult = new SetDataShareRequestQuestionAnswerResult();

        testSetDataShareRequestQuestionAnswerResult.AnswerIsValid = testAnswerIsValid;

        var result = testSetDataShareRequestQuestionAnswerResult.AnswerIsValid;

        Assert.That(result, Is.EqualTo(testAnswerIsValid));
    }

    [Test]
    public void GivenASetDataShareRequestQuestionAnswerResult_WhenISetQuestionInformation_ThenQuestionInformationIsSet()
    {
        var testSetDataShareRequestQuestionAnswerResult = new SetDataShareRequestQuestionAnswerResult();

        var testQuestionInformation = new DataShareRequestQuestion();

        testSetDataShareRequestQuestionAnswerResult.QuestionInformation = testQuestionInformation;

        var result = testSetDataShareRequestQuestionAnswerResult.QuestionInformation;

        Assert.That(result, Is.SameAs(testQuestionInformation));
    }

    [Theory]
    public void GivenASetDataShareRequestQuestionAnswerResult_WhenISetDataShareRequestQuestionsRemainThatRequireAResponse_ThenDataShareRequestQuestionsRemainThatRequireAResponseIsSet(
        bool testDataShareRequestQuestionsRemainThatRequireAResponse)
    {
        var testSetDataShareRequestQuestionAnswerResult = new SetDataShareRequestQuestionAnswerResult();

        testSetDataShareRequestQuestionAnswerResult.DataShareRequestQuestionsRemainThatRequireAResponse = testDataShareRequestQuestionsRemainThatRequireAResponse;

        var result = testSetDataShareRequestQuestionAnswerResult.DataShareRequestQuestionsRemainThatRequireAResponse;

        Assert.That(result, Is.EqualTo(testDataShareRequestQuestionsRemainThatRequireAResponse));
    }
}