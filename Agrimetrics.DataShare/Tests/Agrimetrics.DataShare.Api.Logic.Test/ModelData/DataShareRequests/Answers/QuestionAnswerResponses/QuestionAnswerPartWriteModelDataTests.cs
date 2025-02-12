using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswerResponses;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.Answers.DsrQuestionAnswerResponses;

[TestFixture]
public class DsrQuestionAnswerPartWriteModelDataTests
{
    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWriteModelData_WhenISetQuestionPartId_ThenQuestionPartIdIsSet()
    {
        var testQuestionPartId = new Guid("6035BEEA-F1DD-4659-B488-E5918FA64DBA");

        var testDataShareRequestQuestionAnswerPartWriteModelData = new DataShareRequestQuestionAnswerPartWriteModelData
        {
            QuestionPartId = testQuestionPartId,
            AnswerPartResponses = []
        };

        var result = testDataShareRequestQuestionAnswerPartWriteModelData.QuestionPartId;

        Assert.That(result, Is.EqualTo(testQuestionPartId));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWriteModelData_WhenISetAnEmptySetOfAnswerPartResponses_ThenAnswerPartResponsesIsSet()
    {
        var testAnswerPartResponses = new List<DataShareRequestQuestionAnswerPartResponseWriteModelData>();

        var testDataShareRequestQuestionAnswerPartWriteModelData = new DataShareRequestQuestionAnswerPartWriteModelData
        {
            QuestionPartId = It.IsAny<Guid>(),
            AnswerPartResponses = testAnswerPartResponses
        };

        var result = testDataShareRequestQuestionAnswerPartWriteModelData.AnswerPartResponses;

        Assert.That(result, Is.EqualTo(testAnswerPartResponses));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWriteModelData_WhenISetAnswerPartResponses_ThenAnswerPartResponsesIsSet()
    {
        var testAnswerPartResponses = new List<DataShareRequestQuestionAnswerPartResponseWriteModelData>
        {
            new TestDataShareRequestQuestionAnswerPartResponseWriteModelData
            {
                OrderWithinAnswerPart = It.IsAny<int>()
            },
            new TestDataShareRequestQuestionAnswerPartResponseWriteModelData
            {
                OrderWithinAnswerPart = It.IsAny<int>()
            }
        };

        var testDataShareRequestQuestionAnswerPartWriteModelData = new DataShareRequestQuestionAnswerPartWriteModelData
        {
            QuestionPartId = It.IsAny<Guid>(),
            AnswerPartResponses = testAnswerPartResponses
        };

        var result = testDataShareRequestQuestionAnswerPartWriteModelData.AnswerPartResponses;

        Assert.That(result, Is.EqualTo(testAnswerPartResponses));
    }

    private class TestDataShareRequestQuestionAnswerPartResponseWriteModelData : DataShareRequestQuestionAnswerPartResponseWriteModelData;
}