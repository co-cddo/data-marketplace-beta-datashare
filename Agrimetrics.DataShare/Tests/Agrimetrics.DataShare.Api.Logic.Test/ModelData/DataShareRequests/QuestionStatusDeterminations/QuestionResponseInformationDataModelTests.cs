using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.QuestionStatusDeterminations;

[TestFixture]
public class QuestionResponseInformationDataModelTests
{
    [Test]
    public void GivenAQuestionResponseInformationDataModel_WhenISetQuestionId_ThenQuestionIdIsSet()
    {
        var testQuestionResponseInformationDataModel = new QuestionResponseInformationDataModel();

        var testQuestionId = new Guid("E308E58A-8920-4048-9D4D-A2633E4A1C08");

        testQuestionResponseInformationDataModel.QuestionResponseInformation_QuestionId = testQuestionId;

        var result = testQuestionResponseInformationDataModel.QuestionResponseInformation_QuestionId;

        Assert.That(result, Is.EqualTo(testQuestionId));
    }

    [Test]
    public void GivenAQuestionResponseInformationDataModel_WhenISetAnswerId_ThenAnswerIdIsSet()
    {
        var testQuestionResponseInformationDataModel = new QuestionResponseInformationDataModel();

        var testAnswerId = new Guid("E308E58A-8920-4048-9D4D-A2633E4A1C08");

        testQuestionResponseInformationDataModel.QuestionResponseInformation_AnswerId = testAnswerId;

        var result = testQuestionResponseInformationDataModel.QuestionResponseInformation_AnswerId;

        Assert.That(result, Is.EqualTo(testAnswerId));
    }

    [Theory]
    public void GivenAQuestionResponseInformationDataModel_WhenISetQuestionStatusType_ThenQuestionStatusTypeIsSet(
        QuestionStatusType testQuestionStatusType)
    {
        var testQuestionResponseInformationDataModel = new QuestionResponseInformationDataModel();

        testQuestionResponseInformationDataModel.QuestionResponseInformation_QuestionStatusType = testQuestionStatusType;

        var result = testQuestionResponseInformationDataModel.QuestionResponseInformation_QuestionStatusType;

        Assert.That(result, Is.EqualTo(testQuestionStatusType));
    }

    [Test]
    public void GivenAQuestionResponseInformationDataModel_WhenISetAnEmptySetOfQuestionPartResponses_ThenQuestionPartResponsesIsSet()
    {
        var testQuestionResponseInformationDataModel = new QuestionResponseInformationDataModel();

        var testQuestionPartResponses = new List<QuestionPartResponseDataModel>();

        testQuestionResponseInformationDataModel.QuestionResponseInformation_QuestionPartResponses = testQuestionPartResponses;

        var result = testQuestionResponseInformationDataModel.QuestionResponseInformation_QuestionPartResponses;

        Assert.That(result, Is.EqualTo(testQuestionPartResponses));
    }

    [Test]
    public void GivenAQuestionResponseInformationDataModel_WhenISetQuestionPartResponses_ThenQuestionPartResponsesIsSet()
    {
        var testQuestionResponseInformationDataModel = new QuestionResponseInformationDataModel();

        var testQuestionPartResponses = new List<QuestionPartResponseDataModel> {new(), new(), new()};

        testQuestionResponseInformationDataModel.QuestionResponseInformation_QuestionPartResponses = testQuestionPartResponses;

        var result = testQuestionResponseInformationDataModel.QuestionResponseInformation_QuestionPartResponses;

        Assert.That(result, Is.EqualTo(testQuestionPartResponses));
    }
}