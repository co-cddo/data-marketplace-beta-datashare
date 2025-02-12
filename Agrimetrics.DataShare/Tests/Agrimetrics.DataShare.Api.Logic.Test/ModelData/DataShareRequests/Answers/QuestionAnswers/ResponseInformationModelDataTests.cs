using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.Answers.DsrRequestQuestionAnswers;

[TestFixture]
public class QuestionPartAnswerResponseInformationModelDataTests
{
    [Test]
    public void GivenAQuestionPartAnswerResponseInformationModelData_WhenISetId_ThenIdIsSet()
    {
        var testQuestionPartAnswerResponseInformationModelData = new QuestionPartAnswerResponseInformationModelData();

        var testId = new Guid("3FE2C5B9-4D8E-438E-B0C7-57F5D892F341");

        testQuestionPartAnswerResponseInformationModelData.QuestionPartAnswerResponse_Id = testId;

        var result = testQuestionPartAnswerResponseInformationModelData.QuestionPartAnswerResponse_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenAQuestionPartAnswerResponseInformationModelData_WhenISetOrderWithinAnswerPart_ThenOrderWithinAnswerPartIsSet(
        [Values(-1, 0, 999)] int testOrderWithinAnswerPart)
    {
        var testQuestionPartAnswerResponseInformationModelData = new QuestionPartAnswerResponseInformationModelData();

        testQuestionPartAnswerResponseInformationModelData.QuestionPartAnswerItem_OrderWithinAnswerPart = testOrderWithinAnswerPart;

        var result = testQuestionPartAnswerResponseInformationModelData.QuestionPartAnswerItem_OrderWithinAnswerPart;

        Assert.That(result, Is.EqualTo(testOrderWithinAnswerPart));
    }

    [Theory]
    public void GivenAQuestionPartAnswerResponseInformationModelData_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testQuestionPartAnswerResponseInformationModelData = new QuestionPartAnswerResponseInformationModelData();

        testQuestionPartAnswerResponseInformationModelData.QuestionPartAnswerItem_InputType = testInputType;

        var result = testQuestionPartAnswerResponseInformationModelData.QuestionPartAnswerItem_InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }
}