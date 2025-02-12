using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.QuestionStatusDeterminations;

[TestFixture]
public class QuestionPartResponseDataModelTests
{
    [Theory]
    public void GivenAQuestionPartResponseDataModel_WhenISetResponseInputType_ThenResponseInputTypeIsSet(
        QuestionPartResponseInputType testResponseInputType)
    {
        var testQuestionPartResponseDataModel = new QuestionPartResponseDataModel();

        testQuestionPartResponseDataModel.QuestionPartResponse_ResponseInputType = testResponseInputType;

        var result = testQuestionPartResponseDataModel.QuestionPartResponse_ResponseInputType;

        Assert.That(result, Is.EqualTo(testResponseInputType));
    }

    [Test]
    public void GivenAQuestionPartResponseDataModel_WhenISetANullAnswerPartResponseId_ThenAnswerPartResponseIdIsSet()
    {
        var testQuestionPartResponseDataModel = new QuestionPartResponseDataModel();

        var testAnswerPartResponseId = (Guid?) null;

        testQuestionPartResponseDataModel.QuestionPartResponse_AnswerPartResponseId = testAnswerPartResponseId;

        var result = testQuestionPartResponseDataModel.QuestionPartResponse_AnswerPartResponseId;

        Assert.That(result, Is.EqualTo(testAnswerPartResponseId));
    }

    [Test]
    public void GivenAQuestionPartResponseDataModel_WhenISetAnswerPartResponseId_ThenAnswerPartResponseIdIsSet()
    {
        var testQuestionPartResponseDataModel = new QuestionPartResponseDataModel();

        var testAnswerPartResponseId = new Guid("E308E58A-8920-4048-9D4D-A2633E4A1C08");

        testQuestionPartResponseDataModel.QuestionPartResponse_AnswerPartResponseId = testAnswerPartResponseId;

        var result = testQuestionPartResponseDataModel.QuestionPartResponse_AnswerPartResponseId;

        Assert.That(result, Is.EqualTo(testAnswerPartResponseId));
    }
}