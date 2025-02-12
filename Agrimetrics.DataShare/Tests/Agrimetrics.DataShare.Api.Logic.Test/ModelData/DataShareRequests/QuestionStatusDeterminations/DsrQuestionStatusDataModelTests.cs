using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.QuestionStatusDeterminations;

[TestFixture]
public class DataShareRequestQuestionStatusDataModelTests
{
    [Test]
    public void GivenADataShareRequestQuestionStatusDataModel_WhenISetQuestionId_ThenQuestionIdIsSet()
    {
        var testQuestionId = new Guid("4ED4C86E-9569-472E-A1CB-DF923E10EAEA");

        var testDataShareRequestQuestionStatusDataModel = new DataShareRequestQuestionStatusDataModel
        {
            QuestionId = testQuestionId,
            QuestionStatus = It.IsAny<QuestionStatusType>()
        };

        var result = testDataShareRequestQuestionStatusDataModel.QuestionId;

        Assert.That(result, Is.EqualTo(testQuestionId));
    }

    [Theory]
    public void GivenADataShareRequestQuestionStatusDataModel_WhenISetQuestionStatus_ThenQuestionStatusIsSet(
        QuestionStatusType testQuestionStatus)
    {
        var testDataShareRequestQuestionStatusDataModel = new DataShareRequestQuestionStatusDataModel
        {
            QuestionId = It.IsAny<Guid>(),
            QuestionStatus = testQuestionStatus
        };

        var result = testDataShareRequestQuestionStatusDataModel.QuestionStatus;

        Assert.That(result, Is.EqualTo(testQuestionStatus));
    }
}