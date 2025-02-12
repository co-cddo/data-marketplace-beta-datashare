using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.QuestionStatusDeterminations;

[TestFixture]
public class DataShareRequestQuestionSetQuestionStatusDataModelTests
{
    [Test]
    public void GivenADataShareRequestQuestionSetQuestionStatusDataModel_WhenISetQuestionId_ThenQuestionIdIsSet()
    {
        var testQuestionId = new Guid("842A3BFD-B427-4ED1-9DF0-0D2F17EFA889");

        var testDataShareRequestQuestionSetQuestionStatusDataModel = new DataShareRequestQuestionSetQuestionStatusDataModel
        {
            QuestionId = testQuestionId,
            SectionNumber = It.IsAny<int>(),
            QuestionOrderWithinSection = It.IsAny<int>(),
            QuestionStatus = It.IsAny<QuestionStatusType>()
        };

        var result = testDataShareRequestQuestionSetQuestionStatusDataModel.QuestionId;

        Assert.That(result, Is.EqualTo(testQuestionId));
    }

    [Test]
    public void GivenADataShareRequestQuestionSetQuestionStatusDataModel_WhenISetSectionNumber_ThenSectionNumberIsSet(
        [Values(-1, 0, 999)] int testSectionNumber)
    {
        var testDataShareRequestQuestionSetQuestionStatusDataModel = new DataShareRequestQuestionSetQuestionStatusDataModel
        {
            QuestionId = It.IsAny<Guid>(),
            SectionNumber = testSectionNumber,
            QuestionOrderWithinSection = It.IsAny<int>(),
            QuestionStatus = It.IsAny<QuestionStatusType>()
        };

        var result = testDataShareRequestQuestionSetQuestionStatusDataModel.SectionNumber;

        Assert.That(result, Is.EqualTo(testSectionNumber));
    }

    [Test]
    public void GivenADataShareRequestQuestionSetQuestionStatusDataModel_WhenISetQuestionOrderWithinSection_ThenQuestionOrderWithinSectionIsSet(
        [Values(-1, 0, 999)] int testQuestionOrderWithinSection)
    {
        var testDataShareRequestQuestionSetQuestionStatusDataModel = new DataShareRequestQuestionSetQuestionStatusDataModel
        {
            QuestionId = It.IsAny<Guid>(),
            SectionNumber = It.IsAny<int>(),
            QuestionOrderWithinSection = testQuestionOrderWithinSection,
            QuestionStatus = It.IsAny<QuestionStatusType>()
        };

        var result = testDataShareRequestQuestionSetQuestionStatusDataModel.QuestionOrderWithinSection;

        Assert.That(result, Is.EqualTo(testQuestionOrderWithinSection));
    }

    [Theory]
    public void GivenADataShareRequestQuestionSetQuestionStatusDataModel_WhenISetQuestionStatus_ThenQuestionStatusIsSet(
        QuestionStatusType testQuestionStatus)
    {
        var testDataShareRequestQuestionSetQuestionStatusDataModel = new DataShareRequestQuestionSetQuestionStatusDataModel
        {
            QuestionId = It.IsAny<Guid>(),
            SectionNumber = It.IsAny<int>(),
            QuestionOrderWithinSection = It.IsAny<int>(),
            QuestionStatus = testQuestionStatus
        };

        var result = testDataShareRequestQuestionSetQuestionStatusDataModel.QuestionStatus;

        Assert.That(result, Is.EqualTo(testQuestionStatus));
    }
}

