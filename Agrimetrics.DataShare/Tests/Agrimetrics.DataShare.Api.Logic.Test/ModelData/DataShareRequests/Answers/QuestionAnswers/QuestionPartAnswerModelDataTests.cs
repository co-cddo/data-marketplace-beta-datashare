using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.Answers.DsrRequestQuestionAnswers;

[TestFixture]
public class QuestionPartAnswerModelDataTests
{
    [Test]
    public void GivenAQuestionPartAnswerModelData_WhenISetId_ThenIdIsSet()
    {
        var testQuestionPartAnswerModelData = new QuestionPartAnswerModelData();

        var testId = new Guid("41966CBC-A017-4DEC-9CA1-38AED5569FA2");

        testQuestionPartAnswerModelData.QuestionPartAnswer_Id = testId;

        var result = testQuestionPartAnswerModelData.QuestionPartAnswer_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenAQuestionPartAnswerModelData_WhenISetQuestionPartId_ThenQuestionPartIdIsSet()
    {
        var testQuestionPartAnswerModelData = new QuestionPartAnswerModelData();

        var testQuestionPartId = new Guid("41966CBC-A017-4DEC-9CA1-38AED5569FA2");

        testQuestionPartAnswerModelData.QuestionPartAnswer_QuestionPartId = testQuestionPartId;

        var result = testQuestionPartAnswerModelData.QuestionPartAnswer_QuestionPartId;

        Assert.That(result, Is.EqualTo(testQuestionPartId));
    }

    [Test]
    public void GivenAQuestionPartAnswerModelData_WhenISetAnEmptySetOfAnswerPartResponseInformations_ThenAnswerPartResponseInformationsIsSet()
    {
        var testQuestionPartAnswerModelData = new QuestionPartAnswerModelData();

        var testAnswerPartResponseInformations = new List<QuestionPartAnswerResponseInformationModelData>();

        testQuestionPartAnswerModelData.QuestionPartAnswer_AnswerPartResponseInformations = testAnswerPartResponseInformations;

        var result = testQuestionPartAnswerModelData.QuestionPartAnswer_AnswerPartResponseInformations;

        Assert.That(result, Is.EqualTo(testAnswerPartResponseInformations));
    }

    [Test]
    public void GivenAQuestionPartAnswerModelData_WhenISetAnswerPartResponseInformations_ThenAnswerPartResponseInformationsIsSet()
    {
        var testQuestionPartAnswerModelData = new QuestionPartAnswerModelData();

        var testAnswerPartResponseInformations = new List<QuestionPartAnswerResponseInformationModelData> {new(), new(), new()};

        testQuestionPartAnswerModelData.QuestionPartAnswer_AnswerPartResponseInformations = testAnswerPartResponseInformations;

        var result = testQuestionPartAnswerModelData.QuestionPartAnswer_AnswerPartResponseInformations;

        Assert.That(result, Is.EqualTo(testAnswerPartResponseInformations));
    }

    [Test]
    public void GivenAQuestionPartAnswerModelData_WhenISetAnEmptySetOfAnswerPartResponses_ThenAnswerPartResponsesIsSet()
    {
        var testQuestionPartAnswerModelData = new QuestionPartAnswerModelData();

        var testAnswerPartResponses = new List<QuestionPartAnswerResponseModelData>();

        testQuestionPartAnswerModelData.QuestionPartAnswer_AnswerPartResponses = testAnswerPartResponses;

        var result = testQuestionPartAnswerModelData.QuestionPartAnswer_AnswerPartResponses;

        Assert.That(result, Is.EqualTo(testAnswerPartResponses));
    }

    [Test]
    public void GivenAQuestionPartAnswerModelData_WhenISetAnswerPartResponses_ThenAnswerPartResponsesIsSet()
    {
        var testQuestionPartAnswerModelData = new QuestionPartAnswerModelData();

        var testAnswerPartResponses = new List<QuestionPartAnswerResponseModelData> {new(), new(), new()};

        testQuestionPartAnswerModelData.QuestionPartAnswer_AnswerPartResponses = testAnswerPartResponses;

        var result = testQuestionPartAnswerModelData.QuestionPartAnswer_AnswerPartResponses;

        Assert.That(result, Is.EqualTo(testAnswerPartResponses));
    }
}